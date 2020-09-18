// Decompiled with JetBrains decompiler
// Type: Networking.Native.TransportFactory
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using System;
using System.Net;
using System.Net.Sockets;

namespace Networking.Native
{
  public class TransportFactory : INetworkTransportFactory
  {
    public INetworkTransport CreateEndpointTransport(string address, int port)
    {
      IPAddress address1 = IPAddress.Parse(address);
      UdpClient client = new UdpClient(new IPEndPoint(address1, port));
      client.EnableBroadcast = true;
      if (client.Client.AddressFamily == AddressFamily.InterNetwork)
        client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, address1.GetAddressBytes());
      else if (client.Client.AddressFamily == AddressFamily.InterNetworkV6)
        client.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, BitConverter.GetBytes((int) address1.ScopeId));
      return (INetworkTransport) new UdpTransport(client);
    }

    public INetworkTransport CreateMulticastListener(string address, int port)
    {
      IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
      UdpClient client = new UdpClient();
      client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
      client.Client.Bind((EndPoint) ipEndPoint);
      client.JoinMulticastGroup(IPAddress.Parse(address));
      client.MulticastLoopback = true;
      return (INetworkTransport) new UdpTransport(client);
    }

    public INetworkTransport CreatePairingTransport() => (INetworkTransport) new TcpSocketTransport();

    public INetworkTransport CreateTcpTransport() => (INetworkTransport) new TcpWebTransport();

    public INetworkTransport CreateTcpServer(string address, int port) => (INetworkTransport) new TcpServer(address, port);
  }
}
