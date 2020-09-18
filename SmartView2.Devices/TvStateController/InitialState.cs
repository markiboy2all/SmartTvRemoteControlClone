// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvStateController.InitialState
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices.TvStateController
{
  public class InitialState : TvControllerState
  {
    public InitialState(ControllerState context)
      : base(context)
    {
    }

    public override async Task OnEnter()
    {
      Logger.Instance.LogMessage("Initial State entered.");
      await this.SetState();
    }

    public override async Task ChannelUpAsync() => await this.SetState();

    public override async Task ChannelDownAsync() => await this.SetState();

    public override async Task SetChannelAsync(Channel newChannel) => await this.SetState();

    public override async Task OnChannelChanged() => await this.SetState();

    public override async Task OnChannelListChanged() => await this.SetState();

    public override async Task OnSourceChanged() => await this.SetState();

    private async Task SetState() => throw new NotImplementedException();
  }
}
