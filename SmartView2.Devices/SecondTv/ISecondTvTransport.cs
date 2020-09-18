// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTv.ISecondTvTransport
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using Networking;
using System;

namespace SmartView2.Devices.SecondTv
{
  public interface ISecondTvTransport : INetworkTransport<object>, IDisposable
  {
    event EventHandler<SecondTvNotificationEventArgs> NotificationReceived;
  }
}
