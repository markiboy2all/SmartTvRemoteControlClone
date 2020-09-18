// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.DisconnectMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class DisconnectMessage : Message
  {
    public override string Event => "disconnect";

    public DisconnectMessage() => this.MessageType = SocketIOMessageTypes.Disconnect;

    public DisconnectMessage(string endPoint)
      : this()
      => this.Endpoint = endPoint;

    public static DisconnectMessage Deserialize(string rawMessage)
    {
      DisconnectMessage disconnectMessage = new DisconnectMessage();
      disconnectMessage.RawMessage = rawMessage;
      string[] strArray = rawMessage.Split(Message.SPLITCHARS, 3);
      if (strArray.Length == 3 && !string.IsNullOrWhiteSpace(strArray[2]))
        disconnectMessage.Endpoint = strArray[2];
      return disconnectMessage;
    }

    public override string Encoded => string.Format("0::{0}", (object) this.Endpoint);
  }
}
