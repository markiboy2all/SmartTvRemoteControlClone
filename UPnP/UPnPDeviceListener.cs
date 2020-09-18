// Decompiled with JetBrains decompiler
// Type: UPnP.UPnPDeviceListener
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UPnP
{
  public class UPnPDeviceListener : IDeviceListener, IDisposable
  {
    private const string UPnPAddress = "http://239.255.255.250:1900";
    private readonly INetworkTransportFactory transportFactory;
    private readonly INetworkInfoProvider networkInfoProvider;
    private readonly INetworkTransport multicastTransport;
    private IDictionary<string, INetworkTransport> endpointTransports = (IDictionary<string, INetworkTransport>) new Dictionary<string, INetworkTransport>();
    private readonly Uri UPnPUri = new Uri("http://239.255.255.250:1900");
    private readonly HttpMessage SearchMessage = new HttpMessage(new Uri("http://239.255.255.250:1900"), "M-SEARCH", "*")
    {
      {
        "HOST",
        "239.255.255.250:1900"
      },
      {
        "ST",
        "upnp:rootdevice"
      },
      {
        "MAN",
        "\"ssdp:discover\""
      },
      {
        "MX",
        "5"
      }
    };

    public UPnPDeviceListener(
      INetworkInfoProvider networkInfoProvider,
      INetworkTransportFactory transportFactory)
    {
      if (networkInfoProvider == null)
        throw new ArgumentNullException(nameof (networkInfoProvider));
      if (transportFactory == null)
        throw new ArgumentNullException(nameof (transportFactory));
      this.networkInfoProvider = networkInfoProvider;
      this.transportFactory = transportFactory;
      this.multicastTransport = this.transportFactory.CreateMulticastListener(this.UPnPUri.Host, this.UPnPUri.Port);
      this.multicastTransport.MessageReceived += new EventHandler<HttpMessageEventArgs>(this.multicastTransport_MessageReceived);
      this.UpdateNetworkInterfaces();
    }

    public void BroadcastMessage()
    {
      foreach (INetworkTransport<HttpMessage> networkTransport in (IEnumerable<INetworkTransport>) this.endpointTransports.Values)
        networkTransport.SendRequest(this.SearchMessage);
    }

    public void UpdateNetworkInterfaces()
    {
      IEnumerable<string> networkInterfaces = this.networkInfoProvider.GetIPv4NetworkInterfaces();
      IEnumerable<string> keys = (IEnumerable<string>) this.endpointTransports.Keys;
      IEnumerable<string> array1 = (IEnumerable<string>) networkInterfaces.Except<string>(keys).ToArray<string>();
      IEnumerable<string> array2 = (IEnumerable<string>) keys.Except<string>(networkInterfaces).ToArray<string>();
      foreach (string str in array1)
      {
        INetworkTransport endpointTransport = this.transportFactory.CreateEndpointTransport(str, 0);
        this.AddTransport(str, endpointTransport);
      }
      foreach (string ip in array2)
        this.RemoveTransport(ip);
      if (array1.Count<string>() <= 0 && array2.Count<string>() <= 0)
        return;
      this.OnNetworkInterfacesUpdated((object) this, EventArgs.Empty);
    }

    private void AddTransport(string ip, INetworkTransport transport)
    {
      transport.MessageReceived += new EventHandler<HttpMessageEventArgs>(this.endpointTransport_MessageReceived);
      this.endpointTransports.Add(ip, transport);
    }

    private void RemoveTransport(string ip)
    {
      INetworkTransport endpointTransport = this.endpointTransports[ip];
      endpointTransport.MessageReceived -= new EventHandler<HttpMessageEventArgs>(this.endpointTransport_MessageReceived);
      endpointTransport.Dispose();
      this.endpointTransports.Remove(ip);
    }

    private void endpointTransport_MessageReceived(object sender, HttpMessageEventArgs e) => this.OnAnswerReceived((object) this, e);

    private void multicastTransport_MessageReceived(object sender, HttpMessageEventArgs e) => this.OnNotificationReceived((object) this, e);

    public void Dispose()
    {
      this.multicastTransport.MessageReceived -= new EventHandler<HttpMessageEventArgs>(this.multicastTransport_MessageReceived);
      this.multicastTransport.Dispose();
      foreach (string ip in this.endpointTransports.Keys.ToArray<string>())
        this.RemoveTransport(ip);
    }

    public event EventHandler<HttpMessageEventArgs> AnswerReceived;

    private void OnAnswerReceived(object sender, HttpMessageEventArgs e)
    {
      EventHandler<HttpMessageEventArgs> answerReceived = this.AnswerReceived;
      if (answerReceived == null)
        return;
      answerReceived(sender, e);
    }

    public event EventHandler<HttpMessageEventArgs> NotificationReceived;

    private void OnNotificationReceived(object sender, HttpMessageEventArgs e)
    {
      EventHandler<HttpMessageEventArgs> notificationReceived = this.NotificationReceived;
      if (notificationReceived == null)
        return;
      notificationReceived(sender, e);
    }

    public event EventHandler<EventArgs> NetworkInterfacesUpdated;

    private void OnNetworkInterfacesUpdated(object sender, EventArgs e)
    {
      EventHandler<EventArgs> interfacesUpdated = this.NetworkInterfacesUpdated;
      if (interfacesUpdated == null)
        return;
      interfacesUpdated(sender, e);
    }
  }
}
