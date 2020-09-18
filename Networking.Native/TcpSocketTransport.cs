// Decompiled with JetBrains decompiler
// Type: Networking.Native.TcpSocketTransport
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using SmartView2.Core;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Native
{
  internal class TcpSocketTransport : INetworkTransport, INetworkTransport<HttpMessage>, IDisposable
  {
    private const int HttpStatusSuccess = 200;
    private const int HttpStatusCreated = 201;

    public HttpMessage SendRequest(HttpMessage message)
    {
      using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
      {
        socket.Connect(message.RemoteAddress.Host, message.RemoteAddress.Port);
        NetworkStream networkStream = new NetworkStream(socket);
        StreamWriter streamWriter = new StreamWriter((Stream) networkStream);
        StreamReader streamReader = new StreamReader((Stream) networkStream);
        try
        {
          Logger.Instance.LogMessage(message.ToString());
          streamWriter.Write(message.ToString());
          streamWriter.Flush();
          StringBuilder stringBuilder1 = new StringBuilder();
          string str1;
          do
          {
            str1 = streamReader.ReadLine();
            stringBuilder1.AppendLine(str1);
          }
          while (!string.IsNullOrEmpty(str1));
          HttpMessage message1 = HttpMessage.Parse(stringBuilder1.ToString());
          if (message1.Code != 200 && message1.Code != 201)
            throw new Exception(string.Format("Bad response: {0}", (object) message1));
          int result;
          if (int.TryParse(message1["Content-Length"], out result) && result > 0)
          {
            char[] buffer = new char[result];
            streamReader.Read(buffer, 0, buffer.Length);
            message1.Content = new string(buffer);
          }
          else if (message1["Transfer-Encoding"].ToUpper() == "Chunked".ToUpper())
          {
            StringBuilder stringBuilder2 = new StringBuilder();
            string str2;
            do
            {
              str2 = streamReader.ReadLine();
              stringBuilder2.AppendLine(str2);
            }
            while (!string.IsNullOrEmpty(str2));
            message1.Content = stringBuilder2.ToString();
          }
          EventHandler<HttpMessageEventArgs> messageReceived = this.MessageReceived;
          if (messageReceived != null)
            messageReceived((object) this, new HttpMessageEventArgs(message1));
          return message1;
        }
        finally
        {
          streamWriter.Dispose();
          streamReader.Dispose();
          networkStream.Dispose();
        }
      }
    }

    public Task<HttpMessage> SendRequestAsync(HttpMessage message) => Task.Run<HttpMessage>((Func<HttpMessage>) (() => this.SendRequest(message)));

    public event EventHandler<HttpMessageEventArgs> MessageReceived;

    public void Dispose()
    {
    }
  }
}
