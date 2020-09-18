// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvStateController.NoVideoViewState
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices.TvStateController
{
  public class NoVideoViewState : TvControllerState
  {
    public NoVideoViewState(ControllerState context)
      : base(context)
    {
    }

    public override async Task OnEnter() => Logger.Instance.LogMessage("SecondTV State entered.");

    public override Task ChannelUpAsync() => throw new NotImplementedException();

    public override Task ChannelDownAsync() => throw new NotImplementedException();

    public override async Task SetChannelAsync(Channel newChannel) => throw new NotImplementedException();

    public override async Task OnChannelChanged() => throw new NotImplementedException();

    public override async Task OnChannelListChanged() => throw new NotImplementedException();

    public override Task OnSourceChanged() => throw new NotImplementedException();
  }
}
