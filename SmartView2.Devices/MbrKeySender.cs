// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.MbrKeySender
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Devices.SecondTv;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices
{
  public class MbrKeySender : IKeySender
  {
    private ISecondTvTransport transport;
    private Uri localEndpoint;

    public MbrKeySender(ISecondTvTransport transport, Uri localEndpoint)
    {
      this.transport = transport != null ? transport : throw new ArgumentNullException(nameof (transport));
      this.localEndpoint = localEndpoint;
    }

    public void SendKey(object keyCode) => this.transport.SendRequest((object) new
    {
      method = "POST",
      body = new
      {
        api = "ExecuteSecondTVEMP",
        param1 = "SendMBRIRKey",
        param2 = this.localEndpoint.Host,
        param3 = keyCode,
        plugin = "SecondTV",
        version = "1.000"
      }
    });

    public async Task SendKeyAsync(object keyCode)
    {
      object obj = await this.transport.SendRequestAsync((object) new
      {
        method = "POST",
        body = new
        {
          api = "ExecuteSecondTVEMP",
          param1 = "SendMBRIRKey",
          param2 = this.localEndpoint.Host,
          param3 = keyCode,
          plugin = "SecondTV",
          version = "1.000"
        }
      });
    }
  }
}
