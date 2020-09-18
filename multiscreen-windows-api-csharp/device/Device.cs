// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.Device
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.application;
using com.samsung.multiscreen.application.requests;
using com.samsung.multiscreen.channel;
using com.samsung.multiscreen.device.requests;
using com.samsung.multiscreen.net.dial;
using com.samsung.multiscreen.util;
using multiscreen_windows_api_csharp.device.requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.samsung.multiscreen.device
{
  public class Device
  {
    protected internal const string CLOUD_SERVICE_URL = "https://multiscreen.samsung.com";
    protected internal const string ATTRIB_DEVICE_NAME = "DeviceName";
    protected internal const string ATTRIB_ID = "UDN";
    protected internal const string ATTRIB_IPADDRESS = "IP";
    protected internal const string ATTRIB_NETWORKTYPE = "NetworkType";
    protected internal const string ATTRIB_SSID = "SSID";
    protected internal const string ATTRIB_SERVICEURI = "ServiceURI";
    protected internal const string ATTRIB_DIALURI = "DialURI";
    protected internal const string DEVICE_CAPABILITY_VERSION = "samsung:multiscreen:1";
    protected internal static readonly string CLOUD_DISCOVERY_URL = "https://multiscreen.samsung.com/discovery/reservations";
    private IDictionary<string, string> attributesMap;

    protected internal Device(IDictionary<string, string> attributes) => this.attributesMap = attributes;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[Device]").Append("\nid: ").Append(this.Id).Append("\nname: ").Append(this.Name).Append("\nnetworkType: ").Append(this.NetworkType).Append("\nssid: ").Append(this.SSID).Append("\nipAddress: ").Append(this.IPAddress).Append("\nserviceURI: ").Append((object) this.ServiceURI);
      return stringBuilder.ToString();
    }

    public virtual string GetAttribute(string key) => this.attributesMap != null ? this.attributesMap[key] : "";

    public virtual string Id => this.GetAttribute("UDN");

    public virtual string Name => this.GetAttribute("DeviceName");

    public virtual string NetworkType => this.GetAttribute(nameof (NetworkType));

    public virtual string SSID => this.GetAttribute(nameof (SSID));

    public virtual string IPAddress => this.GetAttribute("IP");

    public virtual Uri ServiceURI
    {
      get
      {
        string attribute = this.GetAttribute(nameof (ServiceURI));
        return attribute == null ? new Uri("") : new Uri(attribute);
      }
    }

    protected internal virtual Uri DialURI
    {
      get
      {
        string attribute = this.GetAttribute(nameof (DialURI));
        return attribute == null ? new Uri("") : new Uri(attribute);
      }
    }

    public virtual void ShowPinCode(DeviceAsyncResult<bool> callback) => new PinCodeRequest(this.ServiceURI, callback, true).run();

    public virtual void HidePinCode(DeviceAsyncResult<bool> callback) => new PinCodeRequest(this.ServiceURI, callback, false).run();

    public static void Search(DeviceAsyncResult<IList<Device>> callback) => new FindLocalDialDevicesRequest(3000, "samsung:multiscreen:1", callback).run();

    public static void GetByCode(string pinCode, DeviceAsyncResult<Device> callback) => new FindDeviceByCodeRequest(new Uri(Device.CLOUD_DISCOVERY_URL + "?code=" + pinCode), "samsung:multiscreen:1", callback).run();

    public virtual void GetApplication(
      string runTitle,
      ApplicationAsyncResult<Application> callback)
    {
      DialClient dialClient = new DialClient(this.DialURI.ToString());
      new GetApplicationRequest(runTitle, this.DialURI, this, dialClient, callback).run();
    }

    private static void GetDevice(Uri serviceUri, DeviceAsyncResult<Device> callback) => new GetDeviceRequest(serviceUri, callback).run();

    public virtual void ConnectToChannel(string channelId, DeviceAsyncResult<Channel> callback) => this.ConnectToChannel(channelId, (IDictionary<string, string>) null, callback);

    public virtual void ConnectToChannel(
      string channelId,
      IDictionary<string, string> clientAttributes,
      DeviceAsyncResult<Channel> callback) => this.GetChannel(channelId, (DeviceAsyncResult<Channel>) new Device.GetChannelCallback(this, clientAttributes, callback));

    protected internal virtual void GetChannel(string id, DeviceAsyncResult<Channel> callback)
    {
      int attempts = 5;
      int delayMS = 2000;
      new PollConnectedHostRequest(this.ServiceURI, id, attempts, delayMS, callback).run();
    }

    private class GetChannelCallback : DeviceAsyncResult<Channel>
    {
      private readonly Device outerInstance;
      private IDictionary<string, string> clientAttributes;
      private DeviceAsyncResult<Channel> callback;

      public GetChannelCallback(
        Device outerInstance,
        IDictionary<string, string> clientAttributes,
        DeviceAsyncResult<Channel> callback)
      {
        this.outerInstance = outerInstance;
        this.clientAttributes = clientAttributes;
        this.callback = callback;
      }

      public virtual void OnResult(Channel channel)
      {
        Logger.Debug("Device.getChannel() onResult() channel:\n" + (object) channel);
        ChannelAsyncResult<bool> callback = (ChannelAsyncResult<bool>) new Device.GetChannelCallback.ChannelAsyncResultCallback(this, channel);
        if (this.clientAttributes == null)
          channel.Connect(callback);
        else
          channel.Connect(this.clientAttributes, callback);
      }

      public virtual void OnError(DeviceError error) => this.callback.OnError(error);

      private class ChannelAsyncResultCallback : ChannelAsyncResult<bool>
      {
        private readonly Device.GetChannelCallback outerInstance;
        private Channel channel;

        public ChannelAsyncResultCallback(Device.GetChannelCallback outerInstance, Channel channel)
        {
          this.outerInstance = outerInstance;
          this.channel = channel;
        }

        public virtual void OnResult(bool result)
        {
          if (result)
            this.outerInstance.callback.OnResult(this.channel);
          else
            this.outerInstance.callback.OnError(new DeviceError("Unknown Channel Connection failure"));
        }

        public virtual void OnError(ChannelError error) => this.outerInstance.callback.OnError(new DeviceError(error.Message));
      }
    }
  }
}
