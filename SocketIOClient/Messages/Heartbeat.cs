// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.Heartbeat
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class Heartbeat : Message
  {
    public static string HEARTBEAT = "2::";

    public Heartbeat() => this.MessageType = SocketIOMessageTypes.Heartbeat;

    public override string Encoded => Heartbeat.HEARTBEAT;
  }
}
