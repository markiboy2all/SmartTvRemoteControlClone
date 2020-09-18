// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.NoopMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class NoopMessage : Message
  {
    public NoopMessage() => this.MessageType = SocketIOMessageTypes.Noop;

    public static NoopMessage Deserialize(string rawMessage) => new NoopMessage();
  }
}
