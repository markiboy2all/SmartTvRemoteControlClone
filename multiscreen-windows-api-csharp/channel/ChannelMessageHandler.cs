// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelMessageHandler
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace com.samsung.multiscreen.channel
{
  internal class ChannelMessageHandler
  {
    private Channel channel;

    internal ChannelMessageHandler(Channel channel) => this.channel = channel;

    protected internal virtual void handleMessage(JSONRPCMessage rpcMessage)
    {
      if (rpcMessage == null)
        return;
      switch (rpcMessage.Method)
      {
        case "ms.channel.onConnect":
          this.handleConnect(rpcMessage);
          break;
        case "ms.channel.onClientConnect":
          this.handleClientConnected(rpcMessage);
          break;
        case "ms.channel.onClientDisconnect":
          this.handleClientDisconnected(rpcMessage);
          break;
        case "ms.channel.onClientMessage":
          this.handleClientMessage(rpcMessage);
          break;
        case null:
          break;
        default:
          Logger.Debug("ChannelMessageHandler.handleMessage() NO HANDLER");
          break;
      }
    }

    protected internal virtual void handleConnect(JSONRPCMessage rpcMessage)
    {
      Logger.Debug("ChannelMessageHandler.handleConnect()");
      string myClientId = (string) rpcMessage.Params[JSONRPCMessage.KEY_CLIENT_ID];
      Logger.Debug("ChannelMessageHandler.handleConnect() clientId: " + myClientId);
      IList<ChannelClient> clientList = (IList<ChannelClient>) new List<ChannelClient>();
      foreach (IDictionary<string, object> @params in (IEnumerable<IDictionary<string, object>>) ((JToken) rpcMessage.Params[JSONRPCMessage.KEY_CLIENTS]).ToObject<IList<IDictionary<string, object>>>())
      {
        ChannelClient channelClient = new ChannelClient(this.channel, @params);
        clientList.Add(channelClient);
      }
      this.channel.HandleConnect(myClientId, clientList);
    }

    protected internal virtual void handleClientConnected(JSONRPCMessage rpcMessage)
    {
      ChannelClient client = new ChannelClient(this.channel, rpcMessage.Params);
      this.channel.Clients.add(client);
      if (this.channel.Listener == null)
        return;
      this.channel.Listener.OnClientConnected(client);
    }

    protected internal virtual void handleClientDisconnected(JSONRPCMessage rpcMessage)
    {
      ChannelClient client = this.channel.Clients.Get((string) rpcMessage.Params[JSONRPCMessage.KEY_ID]);
      if (client == null)
        return;
      this.channel.Clients.Remove(client);
      if (this.channel.Listener == null)
        return;
      this.channel.Listener.OnClientDisconnected(client);
    }

    protected internal virtual void handleClientMessage(JSONRPCMessage rpcMessage)
    {
      ChannelClient client = this.channel.Clients.Get((string) rpcMessage.Params[JSONRPCMessage.KEY_FROM]);
      if (client == null)
        return;
      string message = (string) rpcMessage.Params[JSONRPCMessage.KEY_MESSAGE];
      if (this.channel.Listener == null)
        return;
      this.channel.Listener.OnClientMessage(client, message);
    }

    internal static JSONRPCMessage createWithJSONData(string jsonData) => new JSONRPCMessage(JSONUtil.Parse(jsonData));
  }
}
