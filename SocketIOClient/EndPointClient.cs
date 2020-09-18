using SocketIOClient.Messages;
using System;

namespace SocketIOClient
{
	internal class EndPointClient : IEndPointClient
	{
		public IClient Client
		{
			get;
			private set;
		}

		public string EndPoint
		{
			get;
			private set;
		}

		public EndPointClient(IClient client, string endPoint)
		{
			validateNameSpace(endPoint);
			Client = client;
			EndPoint = endPoint;
		}

		private void validateNameSpace(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("nameSpace", "Parameter cannot be null");
			}
			if (name.Contains(':'))
			{
				throw new ArgumentException("Parameter cannot contain ':' characters", "nameSpace");
			}
		}

		public void On(string eventName, Action<IMessage> action)
		{
			Client.On(eventName, EndPoint, action);
		}

		public void Emit(string eventName, dynamic payload, Action<dynamic> callBack = null)
		{
			Client.Emit(eventName, payload, EndPoint, (Action<object>)callBack);
		}

		public void Send(IMessage msg)
		{
			msg.Endpoint = EndPoint;
			Client.Send(msg);
		}
	}
}
