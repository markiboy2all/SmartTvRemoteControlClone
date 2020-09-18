// Decompiled with JetBrains decompiler
// Type: Networking.Native.SecondTvAsyncTransport
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using SmartView2.Devices.SecondTv;
using System.Threading.Tasks;

namespace Networking.Native
{
  public class SecondTvAsyncTransport : SecondTvTransportBase
  {
    public SecondTvAsyncTransport(ISecondTvSecurityProvider securityProvider)
      : base(securityProvider)
    {
    }

    public override object SendRequest(object message)
    {
      this.Send("callCommon", message);
      return (object) null;
    }

    public override Task<object> SendRequestAsync(object message)
    {
      this.Send("callCommon", message);
      return Task.FromResult<object>((object) null);
    }
  }
}
