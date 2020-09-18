// Decompiled with JetBrains decompiler
// Type: Networking.Native.SecondTvSyncTransport
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using SocketIOClient.Messages;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Native
{
  public class SecondTvSyncTransport : SecondTvTransportBase
  {
    private AutoResetEvent transportLock = new AutoResetEvent(true);

    public SecondTvSyncTransport(ISecondTvSecurityProvider securityProvider): base(securityProvider)
    {
    }

	public override object SendRequest(object message)
	{
		transportLock.WaitOne();
		try
		{
			dynamic incomingMessage = null;
			AutoResetEvent sync = new AutoResetEvent(initialState: false);
			try
			{
				Subsribe("receiveCommon", delegate (IMessage data)
				{
					try
					{
						Logger.Instance.LogMessage($"Common Message received: {data.Encoded}");
						incomingMessage = DecodeMessage(data);
					}
					finally
					{
						sync.Set();
					}
				});
				Send("callCommon", message);
				if (!sync.WaitOne(TimeSpan.FromSeconds(15.0)))
				{
					Logger.Instance.LogErrorFormat("Send request timeout. Input message: {0}", message.ToString());
					throw new TimeoutException("Timeout in SecondTV Transport");
				}
			}
			finally
			{
				if (sync != null)
				{
					((IDisposable)sync).Dispose();
				}
			}
			var anon = new
			{
				Plugin = string.Empty,
				Api = string.Empty,
				Result = string.Empty
			};
			dynamic val = JsonConvert.DeserializeAnonymousType(incomingMessage, anon);
			return val.Result;
		}
		finally
		{
			transportLock.Set();
		}
	}

	public override async Task<object> SendRequestAsync(object message) => await Task.Run<object>((Func<object>) (() => this.SendRequest(message)));

    public override void Dispose()
    {
      if (this.transportLock != null)
      {
        this.transportLock.Dispose();
        this.transportLock = (AutoResetEvent) null;
      }
      base.Dispose();
    }
  }
}
