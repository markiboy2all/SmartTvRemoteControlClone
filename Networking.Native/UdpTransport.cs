// Decompiled with JetBrains decompiler
// Type: Networking.Native.UdpTransport
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking.Native
{
  public class UdpTransport : INetworkTransport, INetworkTransport<HttpMessage>, IDisposable
  {
    private readonly UdpClient client;

    public UdpTransport(UdpClient client)
    {
      this.client = client != null ? client : throw new ArgumentNullException(nameof (client));
      this.client.BeginReceive(new AsyncCallback(this.OnReceive), (object) this.client);
    }

    public HttpMessage SendRequest(HttpMessage message)
    {
      byte[] bytes = message.ToBytes();
      this.client.Send(bytes, bytes.Length, message.RemoteAddress.Host, message.RemoteAddress.Port);
      return (HttpMessage) null;
    }

    public async Task<HttpMessage> SendRequestAsync(HttpMessage message)
    {
      byte[] buffer = message.ToBytes();
      int num = await this.client.SendAsync(buffer, buffer.Length, message.RemoteAddress.Host, message.RemoteAddress.Port);
      return (HttpMessage) null;
    }

    private void OnReceive(IAsyncResult ar)
    {
      UdpClient asyncState = (UdpClient) ar.AsyncState;
      IPEndPoint remoteEP = (IPEndPoint) null;
      byte[] source;
      try
      {
        source = asyncState.EndReceive(ar, ref remoteEP);
      }
      catch (ObjectDisposedException ex)
      {
        return;
      }
      if (source == null)
        return;
      IPEndPoint localEndPoint = (IPEndPoint) asyncState.Client.LocalEndPoint;
      HttpMessage message = HttpMessage.Parse(source);
      message.LocalAddress = new Uri(string.Format("http://{0}:{1}", (object) localEndPoint.Address, (object) localEndPoint.Port));
      message.RemoteAddress = new Uri(string.Format("http://{0}:{1}", (object) remoteEP.Address, (object) remoteEP.Port));
      message.ReceiveTime = DateTime.Now;
      this.OnMessageReceived((object) this, new HttpMessageEventArgs(message));
      asyncState.BeginReceive(new AsyncCallback(this.OnReceive), (object) asyncState);
    }

    public void Dispose() => this.client.Close();

    public event EventHandler<HttpMessageEventArgs> MessageReceived;

    private void OnMessageReceived(object sender, HttpMessageEventArgs e)
    {
      EventHandler<HttpMessageEventArgs> messageReceived = this.MessageReceived;
      if (messageReceived == null)
        return;
      messageReceived(sender, e);
    }
  }
}
