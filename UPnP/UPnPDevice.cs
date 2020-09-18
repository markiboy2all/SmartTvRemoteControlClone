// Decompiled with JetBrains decompiler
// Type: UPnP.UPnPDevice
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;
using System.Threading.Tasks;

namespace UPnP
{
  public class UPnPDevice : IUPnPDevice, IDisposable
  {
    private readonly ServiceCollection services = new ServiceCollection();
    private readonly DeviceCollection subdevices = new DeviceCollection();

    public void Connect()
    {
      foreach (IUPnPService service in this.services)
        service.Connect(this.LocalAddress, this.LocalPort);
    }

    public async Task ConnectAsync()
    {
      foreach (IUPnPService service in this.services)
        await service.ConnectAsync(this.LocalAddress, this.LocalPort);
    }

    public void Dispose()
    {
      foreach (UPnPService service in this.services)
        service.Dispose();
      this.services.Clear();
      foreach (UPnPDevice subdevice in this.subdevices)
        subdevice.Dispose();
      this.subdevices.Clear();
    }

    public string ID { get; set; }

    public string DeviceType { get; set; }

    public string FriendlyName { get; set; }

    public string Manufacturer { get; set; }

    public string ManufacturerUrl { get; set; }

    public string ModelDescription { get; set; }

    public string ModelName { get; set; }

    public string ModelNumber { get; set; }

    public string SerialNumber { get; set; }

    public string LocalAddress { get; set; }

    public int LocalPort { get; set; }

    public ServiceCollection Services => this.services;

    public DeviceCollection SubDevices => this.subdevices;
  }
}
