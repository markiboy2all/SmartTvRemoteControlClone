// Decompiled with JetBrains decompiler
// Type: Networking.INetworkTransportFactory
// Assembly: Networking, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: E30E8B5A-D94B-466A-9DB8-FA63095FAAFB
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.dll

namespace Networking
{
  public interface INetworkTransportFactory
  {
    INetworkTransport CreateEndpointTransport(string address, int port);

    INetworkTransport CreateMulticastListener(string address, int port);

    INetworkTransport CreatePairingTransport();

    INetworkTransport CreateTcpTransport();

    INetworkTransport CreateTcpServer(string ip, int port);
  }
}
