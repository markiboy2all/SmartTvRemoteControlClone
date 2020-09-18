// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.Channel
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.channel.connection;
using com.samsung.multiscreen.channel.info;
using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using System.Collections.Generic;
using System.Text;

namespace com.samsung.multiscreen.channel
{
  public class Channel
  {
    private ChannelInfo channelInfo;
    private IChannelListener channelListener;
    private ChannelConnection connection;
    private ConnectionFactory factory;
    private ChannelClients clients;
    private ChannelAsyncResult<bool> connectCallback;
    private ChannelAsyncResult<bool> disconnectCallback;

    public Channel(ChannelInfo channelInfo, ConnectionFactory factory)
    {
      this.channelInfo = channelInfo;
      this.factory = factory;
      this.clients = new ChannelClients();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[Channel]").Append("\nconnected: ").Append(this.IsConnected).Append("\nchannelInfo: ").Append((object) this.channelInfo).Append("\nconnection: ").Append((object) this.connection).Append("\nclients: ").Append((object) this.clients);
      return stringBuilder.ToString();
    }

    private void shutdown()
    {
      this.clients.Clear();
      this.connection = (ChannelConnection) null;
    }

    public virtual IChannelListener Listener
    {
      set => this.channelListener = value;
      get => this.channelListener;
    }

    public virtual ChannelClients Clients => this.clients;

    protected internal virtual ChannelConnection Connection => this.connection;

    public virtual bool IsConnected => this.connection != null && this.connection.IsConnected();

    public virtual void Connect() => this.Connect((IDictionary<string, string>) null, (ChannelAsyncResult<bool>) null);

    public virtual void Connect(IDictionary<string, string> clientAttributes) => this.Connect(clientAttributes, (ChannelAsyncResult<bool>) null);

    public virtual void Connect(ChannelAsyncResult<bool> callback) => this.Connect((IDictionary<string, string>) null, callback);

    public virtual void Connect(
      IDictionary<string, string> clientAttributes,
      ChannelAsyncResult<bool> callback)
    {
      Logger.Debug("Channel.connect() connected: " + (object) this.IsConnected + ", clientAttributes: " + (object) clientAttributes);
      if (this.IsConnected)
      {
        callback?.OnError(new ChannelError(-1, "Already Connected"));
      }
      else
      {
        this.connectCallback = callback;
        this.connection = this.factory.getConnection(this, this.channelInfo, clientAttributes);
        this.connection.Listener = (IChannelConnectionListener) new ChannelConnectionListener(this);
        this.connection.connect();
      }
    }

    public virtual void Disconnect() => this.Disconnect((ChannelAsyncResult<bool>) null);

    public virtual void Disconnect(ChannelAsyncResult<bool> callback)
    {
      Logger.Debug("Channel.disconnect() connected: " + (object) this.IsConnected);
      if (!this.IsConnected)
      {
        callback?.OnError(new ChannelError(-1, "Not Connected"));
      }
      else
      {
        this.disconnectCallback = callback;
        this.connection.disconnect();
      }
    }

    public virtual void Broadcast(string message) => this.Broadcast(message, false);

    public virtual void Broadcast(string message, bool encryptMessage)
    {
      Logger.Debug("Channel.broadcast() message: " + message);
      if (!this.IsConnected)
        return;
      this.connection.send(JSONRPCMessage.CreateSendMessage((object) JSONRPCMessage.KEY_BROADCAST, message), encryptMessage);
    }

    public virtual void SendToHost(string message) => this.SendToHost(message, false);

    public virtual void SendToHost(string message, bool encryptMessage)
    {
      Logger.Debug("Channel.sendToHost() message: " + message);
      if (!this.IsConnected)
        return;
      this.connection.send(JSONRPCMessage.CreateSendMessage((object) JSONRPCMessage.KEY_HOST, message), encryptMessage);
    }

    public virtual void SendToClient(ChannelClient client, string message) => this.SendToClient(client, message, false);

    public virtual void SendToClient(ChannelClient client, string message, bool encryptMessage)
    {
      Logger.Debug("Channel.sendToClient() client: " + client.GetId() + ", message: " + message);
      if (!this.IsConnected)
        return;
      this.connection.send(JSONRPCMessage.CreateSendMessage((object) client.GetId(), message), encryptMessage);
    }

    public virtual void SendToAll(string message) => this.SendToAll(message, false);

    public virtual void SendToAll(string message, bool encryptMessage)
    {
      Logger.Debug("Channel.sendToAll() message: " + message);
      if (!this.IsConnected)
        return;
      this.connection.send(JSONRPCMessage.CreateSendMessage((object) JSONRPCMessage.KEY_ALL, message), encryptMessage);
    }

    public virtual void SendToClientList(IList<ChannelClient> clientList, string message) => this.SendToClientList(clientList, message, false);

    public virtual void SendToClientList(
      IList<ChannelClient> clientList,
      string message,
      bool encryptMessage)
    {
      Logger.Debug("Channel.sendToClientList() message: " + message);
      if (!this.IsConnected)
        return;
      IList<string> stringList = (IList<string>) new List<string>();
      foreach (ChannelClient client in (IEnumerable<ChannelClient>) clientList)
        stringList.Add(client.GetId());
      this.connection.send(JSONRPCMessage.CreateSendMessage((object) stringList, message), encryptMessage);
    }

    protected internal virtual ChannelInfo ChannelInfo => this.channelInfo;

    protected internal virtual void HandleConnect(
      string myClientId,
      IList<ChannelClient> clientList)
    {
      Logger.Debug("Channel.handleConnect()");
      this.clients.Reset(myClientId, clientList);
      if (this.connectCallback != null)
      {
        this.connectCallback.OnResult(true);
        this.connectCallback = (ChannelAsyncResult<bool>) null;
      }
      if (this.channelListener == null)
        return;
      this.channelListener.OnConnect();
    }

    protected internal virtual void HandleConnectError(ChannelError error)
    {
      Logger.Debug("Channel.handleConnectError() error: " + (object) error);
      this.shutdown();
      if (this.connectCallback == null)
        return;
      this.connectCallback.OnError(error);
      this.connectCallback = (ChannelAsyncResult<bool>) null;
    }

    protected internal virtual void HandleDisconnect()
    {
      Logger.Debug("Channel.handleDisconnect() channelListener: " + (object) this.channelListener);
      if (this.disconnectCallback != null)
        this.disconnectCallback.OnResult(true);
      if (this.channelListener != null)
      {
        Logger.Debug("call channelListener.OnDisconnect()");
        this.channelListener.OnDisconnect();
      }
      this.shutdown();
    }

    protected internal virtual void HandleDisconnectError(ChannelError error)
    {
      Logger.Debug("Channel.handleDisconnectError() " + (object) error);
      if (this.disconnectCallback == null)
        return;
      this.disconnectCallback.OnError(error);
      this.disconnectCallback = (ChannelAsyncResult<bool>) null;
    }
  }
}
