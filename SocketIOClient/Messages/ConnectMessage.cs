// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.ConnectMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class ConnectMessage : Message
  {
    public string Query { get; private set; }

    public override string Event => "connect";

    public ConnectMessage() => this.MessageType = SocketIOMessageTypes.Connect;

    public ConnectMessage(string endPoint)
      : this()
      => this.Endpoint = endPoint;

    public static ConnectMessage Deserialize(string rawMessage)
    {
      ConnectMessage connectMessage = new ConnectMessage();
      connectMessage.RawMessage = rawMessage;
      string[] strArray1 = rawMessage.Split(Message.SPLITCHARS, 3);
      if (strArray1.Length == 3)
      {
        string[] strArray2 = strArray1[2].Split('?');
        if (strArray2.Length > 0)
          connectMessage.Endpoint = strArray2[0];
        if (strArray2.Length > 1)
          connectMessage.Query = strArray2[1];
      }
      return connectMessage;
    }

    public override string Encoded => string.Format("1::{0}{1}", (object) this.Endpoint, string.IsNullOrEmpty(this.Query) ? (object) string.Empty : (object) string.Format("?{0}", (object) this.Query));
  }
}
