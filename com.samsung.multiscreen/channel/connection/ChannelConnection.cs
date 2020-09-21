// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.connection.ChannelConnection
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;

namespace com.samsung.multiscreen.channel.connection
{
  public abstract class ChannelConnection
  {
    private Channel channel;

    protected internal ChannelConnection(Channel channel) => this.channel = channel;

    public override string ToString() => "[ChannelConnection]" + " connected: " + (object) this.IsConnected();

    protected internal virtual Channel Channel => this.channel;

    public virtual IChannelConnectionListener Listener { get; set; }

    public abstract bool IsConnected();

    public abstract void connect();

    public abstract void disconnect();

    public abstract void send(JSONRPCMessage message, bool encrypt);
  }
}
