// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.JSONMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using Newtonsoft.Json;
using System;

namespace SocketIOClient.Messages
{
  public class JSONMessage : SocketIOClient.Messages.Message
  {
    public void SetMessage(object value) => this.MessageText = JsonConvert.SerializeObject(value, Formatting.None);

    public virtual T Message<T>()
    {
      try
      {
        return JsonConvert.DeserializeObject<T>(this.MessageText);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public JSONMessage() => this.MessageType = SocketIOMessageTypes.JSONMessage;

    public JSONMessage(object jsonObject, int? ackId = null, string endpoint = null)
      : this()
    {
      this.AckId = ackId;
      this.Endpoint = endpoint;
      this.MessageText = JsonConvert.SerializeObject(jsonObject, Formatting.None);
    }

    public static JSONMessage Deserialize(string rawMessage)
    {
      JSONMessage jsonMessage = new JSONMessage();
      jsonMessage.RawMessage = rawMessage;
      string[] strArray = rawMessage.Split(SocketIOClient.Messages.Message.SPLITCHARS, 4);
      if (strArray.Length == 4)
      {
        int result;
        if (int.TryParse(strArray[1], out result))
          jsonMessage.AckId = new int?(result);
        jsonMessage.Endpoint = strArray[2];
        jsonMessage.MessageText = strArray[3];
      }
      return jsonMessage;
    }
  }
}
