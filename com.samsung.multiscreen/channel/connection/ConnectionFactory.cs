// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.connection.ConnectionFactory
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.channel.info;
using System.Collections.Generic;

namespace com.samsung.multiscreen.channel.connection
{
  public class ConnectionFactory
  {
    public virtual ChannelConnection getConnection(
      Channel channel,
      ChannelInfo channelInfo,
      IDictionary<string, string> attributes) => (ChannelConnection) new ChannelWebsocketConnection(channel, channelInfo, attributes);
  }
}
