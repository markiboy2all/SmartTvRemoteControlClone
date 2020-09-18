// Decompiled with JetBrains decompiler
// Type: Networking.Native.TcpWebTransport
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Native
{
  public class TcpWebTransport : INetworkTransport, INetworkTransport<HttpMessage>, IDisposable
  {
    private readonly TimeSpan timeout = TimeSpan.FromSeconds(10.0);

    public TcpWebTransport()
      : this(TimeSpan.FromSeconds(10.0))
    {
    }

    public TcpWebTransport(TimeSpan timeout) => this.timeout = timeout;

    public HttpMessage SendRequest(HttpMessage message)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(message.RemoteAddress);
      httpWebRequest.Timeout = (int) this.timeout.TotalMilliseconds;
      if (!string.IsNullOrEmpty(message.Method))
        httpWebRequest.Method = message.Method;
      foreach (string str in message)
      {
        if (str.ToLower().Contains("content-type"))
          httpWebRequest.ContentType = message["content-type"];
        else if (str.ToLower().Contains("host"))
          httpWebRequest.Host = message["host"];
        else
          httpWebRequest.Headers.Add(str, message[str]);
      }
      if (!string.IsNullOrEmpty(message.Content))
      {
        byte[] bytes = Encoding.UTF8.GetBytes(message.Content);
        httpWebRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Flush();
      }
      using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
      {
        string end = new StreamReader(response.GetResponseStream()).ReadToEnd();
        HttpMessage message1 = new HttpMessage(message.RemoteAddress, response.Method, string.Empty, response.ContentType, end);
        message1.ReceiveTime = DateTime.Now;
        foreach (string key in response.Headers.Keys)
          message1[key] = response.Headers[key];
        this.OnMessageReceived((object) this, new HttpMessageEventArgs(message1));
        return message1;
      }
    }

    public async Task<HttpMessage> SendRequestAsync(HttpMessage message)
    {
      HttpWebRequest request = (HttpWebRequest) WebRequest.Create(message.RemoteAddress);
      request.Timeout = (int) this.timeout.TotalMilliseconds;
      if (!string.IsNullOrEmpty(message.Method))
        request.Method = message.Method;
      foreach (string str in message)
      {
        if (str.ToLower().Contains("content-type"))
          request.ContentType = message["content-type"];
        else if (str.ToLower().Contains("host"))
          request.Host = message["host"];
        else
          request.Headers.Add(str, message[str]);
      }
      if (!string.IsNullOrEmpty(message.Content))
      {
        byte[] content = Encoding.UTF8.GetBytes(message.Content);
        request.ContentLength = (long) content.Length;
        Stream requestStream = await request.GetRequestStreamAsync();
        await requestStream.WriteAsync(content, 0, content.Length);
        await requestStream.FlushAsync();
      }
      HttpMessage httpMessage;
      using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
      {
        Stream responseStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream);
        string content = await reader.ReadToEndAsync();
        HttpMessage receivedMessage = new HttpMessage(message.RemoteAddress, response.Method, string.Empty, response.ContentType, content);
        receivedMessage.ReceiveTime = DateTime.Now;
        foreach (string key in response.Headers.Keys)
          receivedMessage[key] = response.Headers[key];
        this.OnMessageReceived((object) this, new HttpMessageEventArgs(receivedMessage));
        httpMessage = receivedMessage;
      }
      return httpMessage;
    }

    public void Dispose()
    {
    }

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
