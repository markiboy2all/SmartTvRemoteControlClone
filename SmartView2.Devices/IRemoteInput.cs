// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.IRemoteInput
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices
{
  public interface IRemoteInput : IDisposable
  {
    Task UpdateTextAsync(string text);

    Task EndInputAsync();

    event EventHandler<EventArgs> ShowInputKeyboard;

    event EventHandler<EventArgs> ShowPasswordKeyboard;

    event EventHandler<UpdateTextEventArgs> TextUpdated;

    event EventHandler<EventArgs> HideKeyboard;
  }
}
