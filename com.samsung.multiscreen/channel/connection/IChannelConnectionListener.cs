// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.connection.IChannelConnectionListener
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;

namespace com.samsung.multiscreen.channel.connection
{
  public interface IChannelConnectionListener
  {
    void onConnect();

    void onConnectError(ChannelError error);

    void onDisconnect();

    void onDisconnectError(ChannelError error);

    void onMessage(JSONRPCMessage message);
  }
}
