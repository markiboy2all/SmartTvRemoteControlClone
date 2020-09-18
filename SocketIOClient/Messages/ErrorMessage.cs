// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.ErrorMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

namespace SocketIOClient.Messages
{
  public class ErrorMessage : Message
  {
    public string Reason { get; set; }

    public string Advice { get; set; }

    public override string Event => "error";

    public ErrorMessage() => this.MessageType = SocketIOMessageTypes.Error;

    public static ErrorMessage Deserialize(string rawMessage)
    {
      ErrorMessage errorMessage = new ErrorMessage();
      string[] strArray1 = rawMessage.Split(':');
      if (strArray1.Length == 4)
      {
        errorMessage.Endpoint = strArray1[2];
        errorMessage.MessageText = strArray1[3];
        string[] strArray2 = strArray1[3].Split('+');
        if (strArray2.Length > 1)
        {
          errorMessage.Advice = strArray2[1];
          errorMessage.Reason = strArray2[0];
        }
      }
      return errorMessage;
    }
  }
}
