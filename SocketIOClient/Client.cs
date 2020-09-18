using SocketIOClient.Eventing;
using SocketIOClient.Messages;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace SocketIOClient
{
	public class Client : IDisposable, IClient
	{
		private Timer socketHeartBeatTimer;

		private Task dequeuOutBoundMsgTask;

		private BlockingCollection<string> outboundQueue;

		private int retryConnectionCount;

		private int retryConnectionAttempts = 3;

		private static readonly object padLock = new object();

		protected Uri uri;

		protected WebSocket wsClient;

		protected RegistrationManager registrationManager;

		protected WebSocketVersion socketVersion = WebSocketVersion.Rfc6455;

		public ManualResetEvent MessageQueueEmptyEvent = new ManualResetEvent(initialState: true);

		public ManualResetEvent ConnectionOpenEvent = new ManualResetEvent(initialState: false);

		public string LastErrorMessage = "";

		private bool isDisposed;

		public int RetryConnectionAttempts
		{
			get
			{
				return retryConnectionAttempts;
			}
			set
			{
				retryConnectionAttempts = value;
			}
		}

		public SocketIOHandshake HandShake
		{
			get;
			private set;
		}

		public bool IsConnected => ReadyState == WebSocketState.Open;

		public WebSocketState ReadyState
		{
			get
			{
				if (wsClient != null)
				{
					return wsClient.State;
				}
				return WebSocketState.None;
			}
		}

		public event EventHandler Opened;

		public event EventHandler<MessageEventArgs> Message;

		public event EventHandler ConnectionRetryAttempt;

		public event EventHandler HeartBeatTimerEvent;

		public event EventHandler SocketConnectionClosed;

		public event EventHandler<SocketIOClient.ErrorEventArgs> Error;

		public Client(string url)
			: this(url, WebSocketVersion.Rfc6455)
		{
		}

		public Client(string url, WebSocketVersion socketVersion)
		{
			uri = new Uri(url);
			this.socketVersion = socketVersion;
			registrationManager = new RegistrationManager();
			outboundQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
			TaskFactory factory = Task.Factory;
			Action action = delegate
			{
				dequeuOutboundMessages();
			};
			dequeuOutBoundMsgTask = factory.StartNew(action, TaskCreationOptions.LongRunning);
		}

		public void Connect()
		{
			lock (padLock)
			{
				if (ReadyState == WebSocketState.Connecting || ReadyState == WebSocketState.Open)
				{
					return;
				}
				try
				{
					ConnectionOpenEvent.Reset();
					HandShake = requestHandshake(uri);
					if (HandShake == null || string.IsNullOrWhiteSpace(HandShake.SID) || HandShake.HadError)
					{
						LastErrorMessage = $"Error initializing handshake with {uri.ToString()}";
						OnErrorEvent(this, new SocketIOClient.ErrorEventArgs(LastErrorMessage, new Exception()));
						return;
					}
					string text = (uri.Scheme == Uri.UriSchemeHttps) ? "wss" : "ws";
					wsClient = new WebSocket($"{text}://{uri.Host}:{uri.Port}/socket.io/1/websocket/{HandShake.SID}", string.Empty, socketVersion);
					wsClient.EnableAutoSendPing = false;
					wsClient.Opened += wsClient_OpenEvent;
					wsClient.MessageReceived += wsClient_MessageReceived;
					wsClient.Error += wsClient_Error;
					wsClient.Closed += wsClient_Closed;
					wsClient.Open();
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"Connect threw an exception...{ex.Message}");
					OnErrorEvent(this, new SocketIOClient.ErrorEventArgs("SocketIO.Client.Connect threw an exception", ex));
				}
			}
		}

		public IEndPointClient Connect(string endPoint)
		{
			EndPointClient result = new EndPointClient(this, endPoint);
			Connect();
			Send(new ConnectMessage(endPoint));
			return result;
		}

		protected void ReConnect()
		{
			if (!isDisposed)
			{
				retryConnectionCount++;
				OnConnectionRetryAttemptEvent(this, EventArgs.Empty);
				closeHeartBeatTimer();
				closeWebSocketClient();
				Connect();
				bool flag = false;
				try
				{
					flag = ConnectionOpenEvent.WaitOne(4000);
				}
				catch (ObjectDisposedException)
				{
				}
				Trace.WriteLine($"\tRetry-Connection successful: {flag}");
				if (flag)
				{
					retryConnectionCount = 0;
					return;
				}
				if (retryConnectionCount < RetryConnectionAttempts)
				{
					ReConnect();
					return;
				}
				Close();
				OnSocketConnectionClosedEvent(this, EventArgs.Empty);
			}
		}

		public virtual void On(string eventName, Action<SocketIOClient.Messages.IMessage> action)
		{
			registrationManager.AddOnEvent(eventName, action);
		}

		public virtual void On(string eventName, string endPoint, Action<SocketIOClient.Messages.IMessage> action)
		{
			registrationManager.AddOnEvent(eventName, endPoint, action);
		}

		public void Emit(string eventName, dynamic payload, string endPoint = "", Action<dynamic> callback = null)
		{
			string text = eventName.ToLower();
			SocketIOClient.Messages.IMessage message = null;
			switch (text)
			{
				case "message":
					if (payload is string)
					{
						TextMessage textMessage = new TextMessage();
						textMessage.MessageText = payload;
						message = textMessage;
					}
					else
					{
						message = new JSONMessage(payload);
					}
					Send(message);
					return;
				case "connect":
				case "disconnect":
				case "open":
				case "close":
				case "error":
				case "retry":
				case "reconnect":
					throw new ArgumentOutOfRangeException(eventName, "Event name is reserved by socket.io, and cannot be used by clients or servers with this message type");
			}
			if (!string.IsNullOrWhiteSpace(endPoint) && !endPoint.StartsWith("/"))
			{
				endPoint = "/" + endPoint;
			}
			message = new EventMessage(eventName, payload, endPoint, (Action<object>)callback);
			if (callback != null)
			{
				registrationManager.AddCallBack(message);
			}
			Send(message);
		}

		public void Emit(string eventName, dynamic payload)
		{
			this.Emit(eventName, payload, string.Empty, null);
		}

		public void Send(SocketIOClient.Messages.IMessage msg)
		{
			MessageQueueEmptyEvent.Reset();
			if (outboundQueue != null)
			{
				outboundQueue.Add(msg.Encoded);
			}
		}

		private void Send(string rawEncodedMessageText)
		{
			MessageQueueEmptyEvent.Reset();
			if (outboundQueue != null)
			{
				outboundQueue.Add(rawEncodedMessageText);
			}
		}

		protected void OnMessageEvent(SocketIOClient.Messages.IMessage msg)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(msg.Event))
			{
				flag = registrationManager.InvokeOnEvent(msg);
			}
			EventHandler<MessageEventArgs> message = this.Message;
			if (message != null && !flag)
			{
				Trace.WriteLine($"webSocket_OnMessage: {msg.RawMessage}");
				message(this, new MessageEventArgs(msg));
			}
		}

		public void Close()
		{
			retryConnectionCount = 0;
			closeHeartBeatTimer();
			closeOutboundQueue();
			closeWebSocketClient();
			if (registrationManager != null)
			{
				registrationManager.Dispose();
				registrationManager = null;
			}
		}

		protected void closeHeartBeatTimer()
		{
			if (socketHeartBeatTimer != null)
			{
				socketHeartBeatTimer.Change(-1, -1);
				socketHeartBeatTimer.Dispose();
				socketHeartBeatTimer = null;
			}
		}

		protected void closeOutboundQueue()
		{
			if (outboundQueue != null)
			{
				outboundQueue.CompleteAdding();
				dequeuOutBoundMsgTask.Wait(700);
				outboundQueue.Dispose();
				outboundQueue = null;
			}
		}

		protected void closeWebSocketClient()
		{
			if (wsClient == null)
			{
				return;
			}
			wsClient.Closed -= wsClient_Closed;
			wsClient.MessageReceived -= wsClient_MessageReceived;
			wsClient.Error -= wsClient_Error;
			wsClient.Opened -= wsClient_OpenEvent;
			if (wsClient.State == WebSocketState.Connecting || wsClient.State == WebSocketState.Open)
			{
				try
				{
					wsClient.Close();
				}
				catch
				{
					Trace.WriteLine("exception raised trying to close websocket: can safely ignore, socket is being closed");
				}
			}
			wsClient = null;
		}

		private void wsClient_OpenEvent(object sender, EventArgs e)
		{
			socketHeartBeatTimer = new Timer(OnHeartBeatTimerCallback, new object(), HandShake.HeartbeatInterval, HandShake.HeartbeatInterval);
			ConnectionOpenEvent.Set();
			OnMessageEvent(new EventMessage
			{
				Event = "open"
			});
			if (this.Opened != null)
			{
				try
				{
					this.Opened(this, EventArgs.Empty);
				}
				catch (Exception value)
				{
					Trace.WriteLine(value);
				}
			}
		}

		private void wsClient_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			SocketIOClient.Messages.IMessage message = SocketIOClient.Messages.Message.Factory(e.Message);
			if (message.Event == "responseMsg")
			{
				Trace.WriteLine($"InvokeOnEvent: {message.RawMessage}");
			}
			switch (message.MessageType)
			{
				case SocketIOMessageTypes.Disconnect:
					OnMessageEvent(message);
					if (string.IsNullOrWhiteSpace(message.Endpoint))
					{
						Close();
					}
					break;
				case SocketIOMessageTypes.Heartbeat:
					OnHeartBeatTimerCallback(null);
					break;
				case SocketIOMessageTypes.Connect:
				case SocketIOMessageTypes.Message:
				case SocketIOMessageTypes.JSONMessage:
				case SocketIOMessageTypes.Event:
				case SocketIOMessageTypes.Error:
					OnMessageEvent(message);
					break;
				case SocketIOMessageTypes.ACK:
					registrationManager.InvokeCallBack(message.AckId, message.Json);
					break;
				default:
					Trace.WriteLine("unknown wsClient message Received...");
					break;
			}
		}

		private void wsClient_Closed(object sender, EventArgs e)
		{
			if (retryConnectionCount < RetryConnectionAttempts)
			{
				ConnectionOpenEvent.Reset();
				ReConnect();
			}
			else
			{
				Close();
				OnSocketConnectionClosedEvent(this, EventArgs.Empty);
			}
		}

		private void wsClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
		{
			OnErrorEvent(sender, new SocketIOClient.ErrorEventArgs("SocketClient error", e.Exception));
		}

		protected void OnErrorEvent(object sender, SocketIOClient.ErrorEventArgs e)
		{
			LastErrorMessage = e.Message;
			if (this.Error != null)
			{
				try
				{
					this.Error(this, e);
				}
				catch
				{
				}
			}
			Trace.WriteLine($"Error Event: {e.Message}\r\n\t{e.Exception}");
		}

		protected void OnSocketConnectionClosedEvent(object sender, EventArgs e)
		{
			if (this.SocketConnectionClosed != null)
			{
				try
				{
					this.SocketConnectionClosed(sender, e);
				}
				catch
				{
				}
			}
			Trace.WriteLine("SocketConnectionClosedEvent");
		}

		protected void OnConnectionRetryAttemptEvent(object sender, EventArgs e)
		{
			if (this.ConnectionRetryAttempt != null)
			{
				try
				{
					this.ConnectionRetryAttempt(sender, e);
				}
				catch (Exception value)
				{
					Trace.WriteLine(value);
				}
			}
			Trace.WriteLine($"Attempting to reconnect: {retryConnectionCount}");
		}

		protected void OnHeartBeatTimerCallback(object state)
		{
			if (ReadyState != WebSocketState.Open)
			{
				return;
			}
			SocketIOClient.Messages.IMessage message = new Heartbeat();
			try
			{
				if (outboundQueue != null && !outboundQueue.IsAddingCompleted)
				{
					outboundQueue.Add(message.Encoded);
					if (this.HeartBeatTimerEvent != null)
					{
						var task = Task.Run(() =>HeartBeatTimerEvent.Invoke(this, EventArgs.Empty));
					}
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"OnHeartBeatTimerCallback Error Event: {ex.Message}\r\n\t{ex.InnerException}");
			}
		}

		protected void dequeuOutboundMessages()
		{
			while (outboundQueue != null && !outboundQueue.IsAddingCompleted)
			{
				if (ReadyState == WebSocketState.Open)
				{
					try
					{
						if (outboundQueue.TryTake(out string item, 500))
						{
							wsClient.Send(item);
						}
						else
						{
							MessageQueueEmptyEvent.Set();
						}
					}
					catch (Exception)
					{
						Trace.WriteLine("The outboundQueue is no longer open...");
					}
				}
				else
				{
					ConnectionOpenEvent.WaitOne(2000);
				}
			}
		}

		protected SocketIOHandshake requestHandshake(Uri uri)
		{
			string value = string.Empty;
			string text = string.Empty;
			SocketIOHandshake socketIOHandshake = null;
			using (WebClient webClient = new WebClient())
			{
				try
				{
					value = webClient.DownloadString($"{uri.Scheme}://{uri.Host}:{uri.Port}/socket.io/1/{uri.Query}");
					if (string.IsNullOrEmpty(value))
					{
						text = "Did not receive handshake string from server";
					}
				}
				catch (Exception ex)
				{
					text = $"Error getting handsake from Socket.IO host instance: {ex.Message}";
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				socketIOHandshake = SocketIOHandshake.LoadFromString(value);
			}
			else
			{
				socketIOHandshake = new SocketIOHandshake();
				socketIOHandshake.ErrorMessage = text;
			}
			return socketIOHandshake;
		}

		public void Dispose()
		{
			isDisposed = true;
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Close();
				MessageQueueEmptyEvent.Dispose();
				ConnectionOpenEvent.Dispose();
			}
		}
	}
}
