// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.connection.ChannelWebsocketConnection
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.channel.info;
using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using Org.BouncyCastle.Crypto;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WebSocket4Net;

namespace com.samsung.multiscreen.channel.connection
{
  public class ChannelWebsocketConnection : ChannelConnection
  {
    private static string TV_PUB_KEY = "-----BEGIN PUBLIC KEY-----\r\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDfaldZKOKdkfvfYiFgX/ZRHdQw\r\nNrb8U8imZ9gNOBXtrDu/hGxHEgyrZ9iMqoIcIhgxBzcKwKBAp4xu6yB3AOZiBwLI\r\n73ajox/CpIzXE9yPevd5wQ+XHctIQazp0qrE9Py5Q5Ox7HB9rmKjSISKQ3A1JtEV\r\nbl0bI0iMf4QCtl/FdQIDAQAB\r\n-----END PUBLIC KEY-----\r\n";
    private AsymmetricCipherKeyPair keyPair;
    private AsymmetricKeyParameter tvPublicKey;
    private string encryptedPublicKey;
    private ChannelWebsocketConnection.WSClient wsClient;
    private bool isConnecting;
    private bool isDisconnecting;

    public ChannelWebsocketConnection(
      Channel channel,
      ChannelInfo channelInfo,
      IDictionary<string, string> attributes)
      : base(channel)
    {
      this.initializePKI();
      if (this.encryptedPublicKey != null)
      {
        if (attributes == null)
          attributes = (IDictionary<string, string>) new Dictionary<string, string>();
        attributes["__pem"] = this.encryptedPublicKey;
      }
      Uri uri = this.createURI(channelInfo.EndPoint, attributes);
      Logger.Debug("new ChannelWebSocketConnection() uri: " + (object) uri);
      this.wsClient = new ChannelWebsocketConnection.WSClient(this, uri);
    }

    private void initializePKI()
    {
      this.keyPair = PKI.generateKeyPair();
      string str = PKI.keyAsPEM(this.keyPair.Public, "PUBLIC KEY");
      Logger.Debug("Key as PEM: \n" + str);
      this.tvPublicKey = PKI.pemAsKey(ChannelWebsocketConnection.TV_PUB_KEY, true);
      if (this.tvPublicKey == null)
        return;
      this.encryptedPublicKey = PKI.encryptStringAsHex(str, this.tvPublicKey);
    }

    private Uri createURI(string endPoint, IDictionary<string, string> attributes)
    {
      UriBuilder uriBuilder = (UriBuilder) null;
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (attributes != null && attributes.Count > 0)
        {
          int num = 1;
          IEnumerator<KeyValuePair<string, string>> enumerator = attributes.GetEnumerator();
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, string> current = enumerator.Current;
            stringBuilder.Append(HttpUtility.UrlEncode(current.Key, Encoding.UTF8));
            stringBuilder.Append("=");
            stringBuilder.Append(HttpUtility.UrlEncode(current.Value, Encoding.UTF8));
            if (num++ < attributes.Count)
              stringBuilder.Append("&");
          }
        }
        uriBuilder = new UriBuilder(endPoint);
        uriBuilder.Query = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
      }
      return uriBuilder.Uri;
    }

    public override bool IsConnected() => this.wsClient.State == WebSocketState.Open;

    public override void connect()
    {
      if (this.IsConnected())
      {
        Logger.Debug("connect() ALREADY CONNECTED");
        if (this.Listener == null)
          return;
        this.Listener.onConnectError(new ChannelError("Already Connected"));
      }
      else
      {
        this.isConnecting = true;
        this.wsClient.Open();
      }
    }

    public override void disconnect()
    {
      if (!this.IsConnected() && this.Listener != null)
        this.Listener.onDisconnectError(new ChannelError("Not Connected"));
      this.wsClient.Close();
    }

    public override void send(JSONRPCMessage message, bool encryptMessage)
    {
      if (encryptMessage)
      {
        Logger.Debug("send() Encrypting outgoing message");
        string str = PKI.encryptStringAsHex((string) message.Params[JSONRPCMessage.KEY_MESSAGE], this.tvPublicKey);
        message.Params[JSONRPCMessage.KEY_MESSAGE] = (object) str;
        message.Params.Add(JSONRPCMessage.KEY_ENCRYPTED, (object) true);
      }
      Logger.Debug("send() - Sending message: " + message.toJSONString());
      this.wsClient.Send(message.toJSONString());
    }

    public virtual void send(string message) => this.wsClient.Send(message);

    private class WSClient : WebSocket
    {
      private readonly ChannelWebsocketConnection outerInstance;
      private Uri serverUri;

      public WSClient(ChannelWebsocketConnection outerInstance, Uri serverURI)
        : base(serverURI.ToString())
      {
        this.outerInstance = outerInstance;
        this.serverUri = serverURI;
        this.Opened += new EventHandler(this.onOpen);
        this.Error += new EventHandler<ErrorEventArgs>(this.onError);
        this.Closed += new EventHandler(this.onClose);
        this.MessageReceived += new EventHandler<MessageReceivedEventArgs>(this.onMessage);
      }

      public void onOpen(object sender, EventArgs e)
      {
        this.outerInstance.isConnecting = false;
        if (this.outerInstance != null && this.outerInstance.Listener != null)
          this.outerInstance.Listener.onConnect();
        Logger.Debug("%%% WSClient Connected %%%");
      }

      public void onMessage(object sender, MessageReceivedEventArgs e)
      {
        if (!e.Message.Contains("video"))
          Logger.Debug("%%% WSClient message: " + e.Message);
        JSONRPCMessage withJsonData = JSONRPCMessage.CreateWithJSONData(e.Message);
        bool flag = false;
        if (withJsonData.Params.ContainsKey(JSONRPCMessage.KEY_ENCRYPTED))
          flag = (bool) withJsonData.Params[JSONRPCMessage.KEY_ENCRYPTED];
        if (flag)
        {
          string str = PKI.decryptHexString((string) withJsonData.Params[JSONRPCMessage.KEY_MESSAGE], this.outerInstance.keyPair.Private);
          withJsonData.Params[JSONRPCMessage.KEY_MESSAGE] = (object) str;
        }
        if (this.outerInstance == null || this.outerInstance.Listener == null)
          return;
        this.outerInstance.Listener.onMessage(withJsonData);
      }

      public void onClose(object sender, EventArgs e)
      {
        this.outerInstance.isDisconnecting = true;
        if (this.outerInstance != null && this.outerInstance.Listener != null)
          this.outerInstance.Listener.onDisconnect();
        Logger.Debug("WSClient onClose: reason: " + e.ToString());
      }

      public void onError(object sender, ErrorEventArgs e)
      {
        string str = "Unexpected connection error";
        if (e != null && e.Exception != null)
          str = e.Exception.ToString();
        if (this.outerInstance.isConnecting)
        {
          this.outerInstance.isConnecting = false;
          if (this.outerInstance != null && this.outerInstance.Listener != null)
            this.outerInstance.Listener.onConnectError(new ChannelError("Connection Failed"));
          Logger.Debug("WSClient: Connect error: " + str);
        }
        else if (this.outerInstance.isDisconnecting)
        {
          this.outerInstance.isDisconnecting = false;
          if (this.outerInstance != null && this.outerInstance.Listener != null)
            this.outerInstance.Listener.onDisconnectError(new ChannelError("Error closing: " + str));
          Logger.Debug("WSClient: Disconnect error: " + str);
        }
        else
          Logger.Debug("WSClient: Other error: " + str);
      }
    }
  }
}
