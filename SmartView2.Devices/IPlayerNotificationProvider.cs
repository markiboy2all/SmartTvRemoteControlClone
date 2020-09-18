// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.IPlayerNotificationProvider
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;

namespace SmartView2.Devices
{
  public interface IPlayerNotificationProvider
  {
    event EventHandler<EventArgs> StreamResolutionChanged;

    event EventHandler<EventArgs> StreamAudioSampleRateChanged;

    event EventHandler<EventArgs> StreamVersionChanged;

    event EventHandler<EventArgs> StreamPmtInfoChanged;

    event EventHandler<StreamMediaEventArgs> StreamMediaChanged;

    event EventHandler<CCDataEventArgs> CCDataReceived;

    event EventHandler VideoShutDown;

    event EventHandler<string> StreamVideoStatusChanged;
  }
}
