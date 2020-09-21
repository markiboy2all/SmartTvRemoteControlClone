// Decompiled with JetBrains decompiler
// Type: UPnP.IDeviceDiscovery
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;
using UPnP.DataContracts;

namespace UPnP
{
  public interface IDeviceDiscovery : IDisposable
  {
    void Scan();

    void Refresh();

    DeviceInfo[] GetFoundTVDeviceInfo();

    event EventHandler<DeviceInfoEventArgs> DeviceConnected;

    event EventHandler<DeviceInfoEventArgs> DeviceUpdated;

    event EventHandler<DeviceInfoEventArgs> DeviceDisconnected;
  }
}
