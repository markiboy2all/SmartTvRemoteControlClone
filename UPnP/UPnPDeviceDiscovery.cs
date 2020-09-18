// Decompiled with JetBrains decompiler
// Type: UPnP.UPnPDeviceDiscovery
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UPnP.DataContracts;

namespace UPnP
{
  public class UPnPDeviceDiscovery : IDeviceDiscovery, IDisposable
  {
    private readonly INetworkTransportFactory transportFactory;
    private readonly IDeviceListener deviceListener;
    private readonly DevicePool devicePool = new DevicePool();

    public UPnPDeviceDiscovery(
      INetworkTransportFactory transportFactory,
      IDeviceListener deviceListener)
    {
      if (transportFactory == null)
        throw new ArgumentNullException(nameof (transportFactory));
      if (deviceListener == null)
        throw new ArgumentNullException(nameof (deviceListener));
      this.transportFactory = transportFactory;
      this.deviceListener = deviceListener;
      this.devicePool.DeviceAdded += new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceAdded);
      this.devicePool.DeviceRemoved += new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceRemoved);
      this.deviceListener.AnswerReceived += new EventHandler<HttpMessageEventArgs>(this.deviceListener_AnswerReceived);
      this.deviceListener.NotificationReceived += new EventHandler<HttpMessageEventArgs>(this.deviceListener_NotificationReceived);
      this.deviceListener.NetworkInterfacesUpdated += new EventHandler<EventArgs>(this.deviceListener_NetworkInterfacesUpdated);
    }

    public void Scan()
    {
      this.deviceListener.UpdateNetworkInterfaces();
      this.deviceListener.BroadcastMessage();
      if (Debugger.IsAttached)
        return;
      this.devicePool.RemoveInactiveDevices(DateTime.Now, TimeSpan.FromSeconds(15.0));
    }

    public void Refresh() => this.devicePool.Clear();

    private void deviceListener_AnswerReceived(object sender, HttpMessageEventArgs e)
    {
      if (!e.Message.HasUniqueServiceName())
        return;
      if (this.devicePool.Contains(e.Message.UniqueServiceName()))
        this.UpdateDevice(e.Message.UniqueServiceName(), e.Message.ReceiveTime);
      else
        this.AddDevice(e.Message);
    }

    private void deviceListener_NotificationReceived(object sender, HttpMessageEventArgs e)
    {
      if (!e.Message.HasUniqueServiceName())
        return;
      switch (e.Message.NotificationSubType())
      {
        case "ssdp:alive":
          this.UpdateDevice(e.Message.UniqueServiceName(), e.Message.ReceiveTime);
          break;
        case "ssdp:byebye":
          this.OnInvalidateDevice(e.Message.UniqueServiceName());
          break;
      }
    }

    private void deviceListener_NetworkInterfacesUpdated(object sender, EventArgs e) => this.Refresh();

    private async void AddDevice(HttpMessage message)
    {
      if (!message.HasLocation())
        return;
      DeviceInfo device = await this.GetDeviceInfoAsync(new Uri(message.Location()));
      if (device == null)
        return;
      device.UniqueServiceName = message.UniqueServiceName();
      device.DeviceAddress = message.RemoteAddress;
      device.LocalAddress = message.LocalAddress;
      device.LastActive = message.ReceiveTime;
      this.devicePool.Add(device);
    }

    private async void UpdateDevice(string usn, DateTime lastActive)
    {
      DeviceInfo existingDevice = this.devicePool[usn];
      if (existingDevice == null)
        return;
      if (existingDevice.IsValid)
      {
        this.devicePool.Update(usn, lastActive);
      }
      else
      {
        DeviceInfo reloadedInfo = await this.GetDeviceInfoAsync(existingDevice.InfoAddress);
        if (reloadedInfo == null)
          return;
        this.devicePool.Update(usn, (Action<DeviceInfo>) (device =>
        {
          device.FriendlyName = reloadedInfo.FriendlyName;
          device.LastActive = lastActive;
          device.IsValid = true;
        }));
      }
    }

    private void OnInvalidateDevice(string usn) => this.devicePool.Update(usn, (Action<DeviceInfo>) (device => device.IsValid = false));

    private async Task<DeviceInfo> GetDeviceInfoAsync(Uri deviceLocation)
    {
      try
      {
        using (INetworkTransport transport = this.transportFactory.CreateTcpTransport())
        {
          HttpMessage request = new HttpMessage(deviceLocation);
          HttpMessage response = await transport.SendRequestAsync(request);
          string rootDeviceDescription = response.Content;
          RootDeviceInfo rootDevice = RootDeviceInfo.Parse(rootDeviceDescription);
          if (rootDevice.Device != null)
          {
            rootDevice.Device.InfoAddress = deviceLocation;
            rootDevice.Device.SourceXml = response.Content;
            rootDevice.Device.IsValid = true;
          }
          return rootDevice.Device;
        }
      }
      catch (IOException ex)
      {
        return (DeviceInfo) null;
      }
      catch (WebException ex)
      {
        return (DeviceInfo) null;
      }
      catch (InvalidOperationException ex)
      {
        return (DeviceInfo) null;
      }
    }

    private void devicePool_DeviceAdded(object sender, DeviceInfoEventArgs e) => this.OnDeviceConnected((object) this, e);

    private void devicePool_DeviceRemoved(object sender, DeviceInfoEventArgs e) => this.OnDeviceDisconnected((object) this, e);

    public void Dispose()
    {
      this.devicePool.DeviceAdded -= new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceAdded);
      this.devicePool.DeviceRemoved -= new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceRemoved);
      this.deviceListener.AnswerReceived -= new EventHandler<HttpMessageEventArgs>(this.deviceListener_AnswerReceived);
      this.deviceListener.NotificationReceived -= new EventHandler<HttpMessageEventArgs>(this.deviceListener_NotificationReceived);
      this.deviceListener.Dispose();
    }

    public event EventHandler<DeviceInfoEventArgs> DeviceConnected;

    private void OnDeviceConnected(object sender, DeviceInfoEventArgs e)
    {
      EventHandler<DeviceInfoEventArgs> deviceConnected = this.DeviceConnected;
      if (deviceConnected == null)
        return;
      deviceConnected(sender, e);
    }

    public event EventHandler<DeviceInfoEventArgs> DeviceUpdated;

    private void OnDeviceUpdated(object sender, DeviceInfoEventArgs e)
    {
      EventHandler<DeviceInfoEventArgs> deviceUpdated = this.DeviceUpdated;
      if (deviceUpdated == null)
        return;
      deviceUpdated(sender, e);
    }

    public event EventHandler<DeviceInfoEventArgs> DeviceDisconnected;

    private void OnDeviceDisconnected(object sender, DeviceInfoEventArgs e)
    {
      EventHandler<DeviceInfoEventArgs> deviceDisconnected = this.DeviceDisconnected;
      if (deviceDisconnected == null)
        return;
      deviceDisconnected(sender, e);
    }
  }
}
