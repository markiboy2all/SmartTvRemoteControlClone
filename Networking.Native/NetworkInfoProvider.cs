// Decompiled with JetBrains decompiler
// Type: Networking.Native.NetworkInfoProvider
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Networking.Native
{
  public class NetworkInfoProvider : INetworkInfoProvider
  {
    public IEnumerable<string> GetIPv4NetworkInterfaces() => (IEnumerable<string>) ((IEnumerable<IPAddress>) Dns.GetHostEntry(Dns.GetHostName()).AddressList).Where<IPAddress>((Func<IPAddress, bool>) (ip => ip.AddressFamily == AddressFamily.InterNetwork)).Select<IPAddress, string>((Func<IPAddress, string>) (ip => ip.ToString())).ToArray<string>();

    public IEnumerable<string> GetIPv6LinkLocalNetworkInterfaces() => (IEnumerable<string>) ((IEnumerable<IPAddress>) Dns.GetHostEntry(Dns.GetHostName()).AddressList).Where<IPAddress>((Func<IPAddress, bool>) (ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && ip.IsIPv6LinkLocal)).Select<IPAddress, string>((Func<IPAddress, string>) (ip => ip.ToString())).ToArray<string>();

    public IEnumerable<string> GetIPv6SiteLocalNetworkInterfaces() => (IEnumerable<string>) ((IEnumerable<IPAddress>) Dns.GetHostEntry(Dns.GetHostName()).AddressList).Where<IPAddress>((Func<IPAddress, bool>) (ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !ip.IsIPv6LinkLocal)).Select<IPAddress, string>((Func<IPAddress, string>) (ip => ip.ToString())).ToArray<string>();
  }
}
