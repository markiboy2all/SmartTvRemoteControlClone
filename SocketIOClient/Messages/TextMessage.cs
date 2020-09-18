// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.TextMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class TextMessage : Message
  {
    private string eventName = "message";

    public override string Event => this.eventName;

    public TextMessage() => this.MessageType = SocketIOMessageTypes.Message;

    public TextMessage(string textMessage)
      : this()
      => this.MessageText = textMessage;

    public static TextMessage Deserialize(string rawMessage)
    {
      TextMessage textMessage = new TextMessage();
      textMessage.RawMessage = rawMessage;
      string[] strArray = rawMessage.Split(Message.SPLITCHARS, 4);
      if (strArray.Length == 4)
      {
        int result;
        if (int.TryParse(strArray[1], out result))
          textMessage.AckId = new int?(result);
        textMessage.Endpoint = strArray[2];
        textMessage.MessageText = strArray[3];
      }
      else
        textMessage.MessageText = rawMessage;
      return textMessage;
    }
  }
}
