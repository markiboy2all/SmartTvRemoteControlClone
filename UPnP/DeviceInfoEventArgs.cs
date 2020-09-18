// Decompiled with JetBrains decompiler
// Type: UPnP.DeviceInfoEventArgs
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;
using UPnP.DataContracts;

namespace UPnP
{
  public class DeviceInfoEventArgs : EventArgs
  {
    public DeviceInfoEventArgs()
      : this((DeviceInfo) null)
    {
    }

    public DeviceInfoEventArgs(DeviceInfo device) => this.DeviceInfo = device;

    public DeviceInfo DeviceInfo { get; set; }
  }
}
