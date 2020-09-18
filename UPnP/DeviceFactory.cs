// Decompiled with JetBrains decompiler
// Type: UPnP.DeviceFactory
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;
using System;
using UPnP.DataContracts;

namespace UPnP
{
  public static class DeviceFactory
  {
    public static IUPnPDevice CreateDevice(
      DeviceInfo deviceInfo,
      INetworkTransportFactory transportFactory)
    {
      INetworkTransport tcpTransport = transportFactory.CreateTcpTransport();
      INetworkTransport tcpServer = transportFactory.CreateTcpServer(deviceInfo.LocalAddress.Host, deviceInfo.LocalAddress.Port);
      return DeviceFactory.CreateDevice(deviceInfo, tcpTransport, tcpServer);
    }

    private static IUPnPDevice CreateDevice(
      DeviceInfo deviceInfo,
      INetworkTransport transport,
      INetworkTransport server)
    {
      UPnPDevice upnPdevice = new UPnPDevice()
      {
        ID = deviceInfo.UniqueDeviceName,
        DeviceType = deviceInfo.DeviceType,
        FriendlyName = deviceInfo.FriendlyName,
        Manufacturer = deviceInfo.Manufacturer,
        ManufacturerUrl = deviceInfo.ManufacturerUrl,
        ModelDescription = deviceInfo.ModelDescription,
        ModelName = deviceInfo.ModelName,
        ModelNumber = deviceInfo.ModelNumber,
        SerialNumber = deviceInfo.SerialNumber,
        LocalAddress = deviceInfo.LocalAddress.Host,
        LocalPort = deviceInfo.LocalAddress.Port
      };
      if (deviceInfo.Services != null)
      {
        foreach (ServiceInfo service in deviceInfo.Services)
          upnPdevice.Services.Add((IUPnPService) DeviceFactory.CreateService(service, deviceInfo.DeviceAddress, transport, server));
      }
      if (deviceInfo.Devices != null)
      {
        foreach (DeviceInfo device in deviceInfo.Devices)
          upnPdevice.SubDevices.Add(DeviceFactory.CreateDevice(deviceInfo, transport, server));
      }
      return (IUPnPDevice) upnPdevice;
    }

    private static UPnPService CreateService(
      ServiceInfo serviceInfo,
      Uri deviceAddress,
      INetworkTransport transport,
      INetworkTransport server) => new UPnPService(transport, server)
    {
      ID = serviceInfo.ServiceId,
      ServiceType = serviceInfo.ServiceType,
      ControlUrl = new UriBuilder(deviceAddress)
      {
        Path = serviceInfo.ControlUrl
      }.Uri,
      EventUrl = new UriBuilder(deviceAddress)
      {
        Path = serviceInfo.EventUrl
      }.Uri
    };
  }
}
