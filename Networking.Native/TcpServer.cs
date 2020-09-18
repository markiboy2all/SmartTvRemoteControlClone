// Decompiled with JetBrains decompiler
// Type: Networking.Native.TcpServer
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Native
{
  public class TcpServer : INetworkTransport, INetworkTransport<HttpMessage>, IDisposable
  {
    private readonly CancellationTokenSource cancelSource = new CancellationTokenSource();

    public TcpServer(string address, int port)
    {
      TcpServer tcpServer = this;
      if (address == null)
        throw new ArgumentNullException(nameof (address));
      Task.Run((Action) (() => tcpServer.Listen(address, port, tcpServer.cancelSource.Token)), this.cancelSource.Token);
    }

    public HttpMessage SendRequest(HttpMessage message)
    {
      byte[] bytes = message.ToBytes();
      TcpClient tcpClient = new TcpClient(message.RemoteAddress.Host, message.RemoteAddress.Port);
      NetworkStream stream = tcpClient.GetStream();
      stream.Write(bytes, 0, bytes.Length);
      stream.Flush();
      tcpClient.Close();
      return (HttpMessage) null;
    }

    public async Task<HttpMessage> SendRequestAsync(HttpMessage message)
    {
      byte[] buffer = message.ToBytes();
      TcpClient client = new TcpClient(message.RemoteAddress.Host, message.RemoteAddress.Port);
      NetworkStream stream = client.GetStream();
      await stream.WriteAsync(buffer, 0, buffer.Length);
      await stream.FlushAsync();
      client.Close();
      return (HttpMessage) null;
    }

    private async void Listen(string address, int port, CancellationToken cancel)
    {
      IPAddress ip = IPAddress.Parse(address);
      TcpListener listener = new TcpListener(ip, port);
      listener.Start();
      while (!cancel.IsCancellationRequested)
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        await Task.Run((Action) (() => this.ClientThread(client, cancel)), cancel);
      }
      listener.Stop();
    }

    private void ClientThread(TcpClient client, CancellationToken cancel)
    {
      IPEndPoint localEndPoint = (IPEndPoint) client.Client.LocalEndPoint;
      IPEndPoint remoteEndPoint = (IPEndPoint) client.Client.RemoteEndPoint;
      IList<byte> source = (IList<byte>) new List<byte>();
      byte[] numArray = new byte[16]
      {
        (byte) 60,
        (byte) 47,
        (byte) 101,
        (byte) 58,
        (byte) 112,
        (byte) 114,
        (byte) 111,
        (byte) 112,
        (byte) 101,
        (byte) 114,
        (byte) 116,
        (byte) 121,
        (byte) 115,
        (byte) 101,
        (byte) 116,
        (byte) 62
      };
      using (NetworkStream stream = client.GetStream())
      {
        bool flag;
        do
        {
          do
          {
            source.Add((byte) stream.ReadByte());
          }
          while (source.Count < numArray.Length);
          flag = true;
          for (int index = 0; index < numArray.Length; ++index)
          {
            if ((int) source[source.Count - numArray.Length + index] != (int) numArray[index])
            {
              flag = false;
              break;
            }
          }
        }
        while (!flag);
      }
      client.Close();
      HttpMessage message = HttpMessage.Parse(source.ToArray<byte>());
      message.LocalAddress = new Uri(string.Format("http://{0}:{1}", (object) localEndPoint.Address, (object) localEndPoint.Port));
      message.RemoteAddress = new Uri(string.Format("http://{0}:{1}", (object) remoteEndPoint.Address, (object) remoteEndPoint.Port));
      message.ReceiveTime = DateTime.Now;
      this.OnMessageReceived((object) this, new HttpMessageEventArgs(message));
    }

    public void Dispose() => this.cancelSource.Cancel();

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
