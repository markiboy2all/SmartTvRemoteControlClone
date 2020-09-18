using Newtonsoft.Json;
using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using SocketIOClient;
using SocketIOClient.Messages;
using System;
using System.Threading.Tasks;

namespace Networking.Native
{
	public abstract class SecondTvTransportBase : ISecondTvTransport, INetworkTransport<object>, IDisposable
	{
		private Client client;

		private IEndPointClient socket;

		private ISecondTvSecurityProvider securityProvider;

		public event EventHandler<SecondTvNotificationEventArgs> NotificationReceived;

		public SecondTvTransportBase()
			: this(new NoSecurityProvider())
		{
		}

		public SecondTvTransportBase(ISecondTvSecurityProvider securityProvider)
		{
			if (securityProvider == null)
			{
				throw new ArgumentNullException("securityProvider");
			}
			this.securityProvider = securityProvider;
		}

		public void Connect(Uri address)
		{
			UriBuilder uriBuilder = new UriBuilder(address);
			uriBuilder.Port = 8000;
			uriBuilder.Path = "com.samsung.companion";
			client = new Client(uriBuilder.Uri.ToString());
			socket = client.Connect("/com.samsung.companion");
			Subsribe("receivePush", OnPushMessage);
			Send("registerPush", new
			{
				eventType = "EMP",
				plugin = "SecondTV"
			});
			Send("registerPush", new
			{
				eventType = "EMP",
				plugin = "RemoteControl"
			});
		}

		public abstract object SendRequest(object message);

		public abstract Task<object> SendRequestAsync(object message);

		protected void Send(string methodName, dynamic argument)
		{
			argument = securityProvider.EncryptData(argument);
			Logger.Instance.LogMessageFormat("Sending  Message: {0}({1})", methodName, JsonConvert.SerializeObject(argument).ToString());
			socket.Emit(methodName, argument);
		}

		protected void Subsribe(string eventName, Action<IMessage> handler)
		{
			socket.On(eventName, handler);
		}

		protected string DecodeMessage(IMessage message)
		{
			return securityProvider.DecryptData(message.Json.Args[0]);
		}

		private void OnPushMessage(IMessage message)
		{
			Logger.Instance.LogMessageFormat("Push Message received: {0}", message.Encoded);
			string value = DecodeMessage(message);
			var anonymousTypeObject = new
			{
				Plugin = string.Empty,
				Event = 0,
				Data1 = string.Empty,
				Data2 = string.Empty
			};
			var anon = JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject);
			OnNotificationReceived(this, new SecondTvNotificationEventArgs
			{
				PluginName = anon.Plugin,
				NotificationType = anon.Event,
				NotificationText = anon.Data1,
				NotificationArgument = anon.Data2
			});
		}

		public virtual void Dispose()
		{
			if (client != null)
			{
				client.Dispose();
				client = null;
			}
			socket = null;
			securityProvider = null;
		}

		protected void OnNotificationReceived(object sender, SecondTvNotificationEventArgs e)
		{
			this.NotificationReceived?.Invoke(sender, e);
		}
	}
}
