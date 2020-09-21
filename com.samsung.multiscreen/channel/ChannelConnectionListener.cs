// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelConnectionListener
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.channel.connection;
using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;

namespace com.samsung.multiscreen.channel
{
  internal class ChannelConnectionListener : IChannelConnectionListener
  {
    private Channel channel;
    private ChannelMessageHandler messageHandler;

    internal ChannelConnectionListener(Channel channel)
    {
      this.channel = channel;
      this.messageHandler = new ChannelMessageHandler(channel);
    }

    public void onConnect() => Logger.Debug("ChannelConnectionListener.onConnect()");

    public void onConnectError(ChannelError error)
    {
      Logger.Debug("ChannelConnectionListener.onConnectError()");
      this.channel.HandleConnectError(error);
    }

    public void onDisconnect()
    {
      Logger.Debug("ChannelConnectionListener.onDisconnect()");
      this.channel.HandleDisconnect();
    }

    public void onDisconnectError(ChannelError error)
    {
      Logger.Debug("ChannelConnectionListener.onDisconnectError()");
      this.channel.HandleDisconnectError(error);
    }

    public void onMessage(JSONRPCMessage message) => this.messageHandler.handleMessage(message);
  }
}
