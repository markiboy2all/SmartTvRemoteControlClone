// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvStateController.ControllerState
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices.TvStateController
{
  public class ControllerState
  {
    private TvControllerState currentState;

    public TvDevice TargetDevice { get; private set; }

    public ISecondTv SecondTvFeatures { get; private set; }

    public ControllerState(TvDevice targetDevice, ISecondTv secondTvFeatures)
    {
      if (targetDevice == null)
        throw new ArgumentNullException(nameof (targetDevice));
      if (secondTvFeatures == null)
        throw new ArgumentNullException(nameof (secondTvFeatures));
      this.TargetDevice = targetDevice;
      this.SecondTvFeatures = secondTvFeatures;
    }

    internal async Task SetStateAsync(TvControllerState state)
    {
      this.currentState = state;
      await this.currentState.OnEnter();
    }

    public async Task ChannelUpAsync()
    {
      Logger.Instance.LogMessage("Controller.ChannelUpAsync()");
      await this.currentState.ChannelUpAsync();
    }

    public async Task ChannelDownAsync()
    {
      Logger.Instance.LogMessage("Controller.ChannelDownAsync()");
      await this.currentState.ChannelDownAsync();
    }

    public async Task SetChannelAsync(Channel newChannel)
    {
      Logger.Instance.LogMessage("Controller.SetChannelAsync(IChannel )");
      await this.currentState.SetChannelAsync(newChannel);
    }

    public async Task OnChannelChanged()
    {
      Logger.Instance.LogMessage("Controller.OnChannelChanged()");
      await this.currentState.OnChannelChanged();
    }

    public async Task OnChannelListChanged()
    {
      Logger.Instance.LogMessage("Controller.OnChannelListChanged()");
      await this.currentState.OnChannelListChanged();
    }

    public async Task OnSourceChanged()
    {
      Logger.Instance.LogMessage("Controller.OnSourceChanged()");
      await this.currentState.OnSourceChanged();
    }
  }
}
