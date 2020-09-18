// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvStateController.TvControllerState
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System.Threading.Tasks;

namespace SmartView2.Devices.TvStateController
{
  public abstract class TvControllerState
  {
    protected ControllerState Context { get; private set; }

    public TvControllerState(ControllerState context) => this.Context = context;

    public abstract Task OnEnter();

    public abstract Task ChannelUpAsync();

    public abstract Task ChannelDownAsync();

    public abstract Task SetChannelAsync(Channel newChannel);

    public abstract Task OnChannelChanged();

    public abstract Task OnChannelListChanged();

    public abstract Task OnSourceChanged();
  }
}
