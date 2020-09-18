// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.ssdp.SSDPSearch
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace com.samsung.multiscreen.net.ssdp
{
  internal class SSDPSearch
  {
    private static string DEFAULT_URN = "urn:dial-multiscreen-org:service:dial:1";
    private string searchTarget;
    private bool running;
    private int timeout = 5000;
    private IList<SSDPSearchResult> results;
    private SSDPSearchListener listener;

    public SSDPSearch(string searchTarget)
    {
      Logger.Trace("new SSDPSearch() searchTarget: " + searchTarget);
      this.searchTarget = !string.IsNullOrEmpty(searchTarget) ? searchTarget : SSDPSearch.DEFAULT_URN;
      this.running = false;
      this.results = (IList<SSDPSearchResult>) new List<SSDPSearchResult>();
    }

    public virtual void Start(SSDPSearchListener listener) => this.Start(this.timeout, listener);

    public virtual void Start(int timeout, SSDPSearchListener listener)
    {
      if (this.running)
        return;
      this.running = true;
      if (timeout > 0)
        this.timeout = timeout;
      Logger.Trace("start() running: " + (object) this.running + ", timeout: " + (object) this.timeout);
      this.listener = listener;
      int availableLocalPort = NetworkUtil.GetAvailableLocalPort();
      Console.WriteLine("The local port used is " + (object) availableLocalPort);
      IPEndPoint ipEndPoint1 = new IPEndPoint(IPAddress.Any, availableLocalPort);
      IPEndPoint ipEndPoint2 = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
      socket.Bind((EndPoint) ipEndPoint1);
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 5);
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);
      Console.WriteLine("UDP-Socket setup done...\r\n");
      string request = this.CreateRequest(this.searchTarget);
      Console.WriteLine(request);
      foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        networkInterface.GetIPProperties();
        if (networkInterface.GetIPProperties().MulticastAddresses != null && networkInterface.SupportsMulticast && OperationalStatus.Up == networkInterface.OperationalStatus)
        {
          IPv4InterfaceProperties ipv4Properties = networkInterface.GetIPProperties().GetIPv4Properties();
          if (ipv4Properties != null)
          {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, IPAddress.HostToNetworkOrder(ipv4Properties.Index));
            socket.SendTo(Encoding.UTF8.GetBytes(request), SocketFlags.None, (EndPoint) ipEndPoint2);
            Console.WriteLine("M-Search sent...\r\n");
          }
        }
      }
      Console.WriteLine("Starting the receiver...");
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.StartReceiver), (object) socket);
    }

    private void StartReceiver(object sock)
    {
      if (sock == null)
        return;
      Socket socket = sock as Socket;
      byte[] numArray = new byte[64000];
      Console.WriteLine("Receiver is started");
      int num = 0;
      while (this.running && num++ < this.timeout)
      {
        if (socket.Available > 0)
        {
          int count = socket.Receive(numArray, SocketFlags.None);
          if (count > 0)
          {
            SSDPSearchResult result = SSDPSearchResult.CreateResult(Encoding.UTF8.GetString(numArray, 0, count));
            if (result != null)
            {
              this.results.Add(result);
              if (this.listener != null)
              {
                try
                {
                  this.listener.OnResult(result);
                }
                catch (Exception ex)
                {
                  Console.WriteLine(ex.ToString());
                  Console.Write(ex.StackTrace);
                }
              }
            }
          }
        }
        Thread.Sleep(1);
      }
      this.running = false;
      socket.Close();
      this.NotifyListener();
      Console.WriteLine("TV search is finished");
    }

    public virtual void Stop()
    {
      Logger.Trace("stop() running: " + (object) this.running);
      if (!this.running)
        return;
      this.running = false;
      this.NotifyListener();
    }

    private void NotifyListener()
    {
      if (this.listener == null)
        return;
      try
      {
        this.listener.OnResults(this.results);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      this.listener = (SSDPSearchListener) null;
    }

    private string CreateRequest(string searchTarget) => "M-SEARCH * HTTP/1.1\r\n" + "HOST: 239.255.255.250:" + (object) 1900 + "\r\n" + "MAN: \"ssdp:discover\"\r\n" + "ST: " + searchTarget + "\r\n" + "MX: " + (object) 3 + "\r\n" + "\r\n";
  }
}
