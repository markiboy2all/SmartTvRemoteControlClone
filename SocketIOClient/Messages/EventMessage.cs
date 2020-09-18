// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.EventMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;
using System.Diagnostics;

namespace SocketIOClient.Messages
{
  public class EventMessage : Message
  {
    private static object ackLock = new object();
    private static int _akid = 0;
    public Action<object> Callback;

    private static int NextAckID
    {
      get
      {
        lock (EventMessage.ackLock)
        {
          ++EventMessage._akid;
          if (EventMessage._akid < 0)
            EventMessage._akid = 0;
          return EventMessage._akid;
        }
      }
    }

    public EventMessage() => this.MessageType = SocketIOMessageTypes.Event;

    public EventMessage(
      string eventName,
      object jsonObject,
      string endpoint = "",
      Action<object> callBack = null)
      : this()
    {
      this.Callback = callBack;
      this.Endpoint = endpoint;
      if (callBack != null)
        this.AckId = new int?(EventMessage.NextAckID);
      this.JsonEncodedMessage = new JsonEncodedEventMessage(eventName, jsonObject);
      this.MessageText = this.Json.ToJsonString();
    }

    public static EventMessage Deserialize(string rawMessage)
    {
      EventMessage eventMessage = new EventMessage();
      eventMessage.RawMessage = rawMessage;
      try
      {
        string[] strArray = rawMessage.Split(Message.SPLITCHARS, 4);
        if (strArray.Length == 4)
        {
          int result;
          if (int.TryParse(strArray[1], out result))
            eventMessage.AckId = new int?(result);
          eventMessage.Endpoint = strArray[2];
          eventMessage.MessageText = strArray[3];
          if (!string.IsNullOrEmpty(eventMessage.MessageText) && eventMessage.MessageText.Contains("name") && eventMessage.MessageText.Contains("args"))
          {
            eventMessage.Json = JsonEncodedEventMessage.Deserialize(eventMessage.MessageText);
            eventMessage.Event = eventMessage.Json.Name;
          }
          else
            eventMessage.Json = new JsonEncodedEventMessage();
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine((object) ex);
      }
      return eventMessage;
    }

    public override string Encoded
    {
      get
      {
        int messageType = (int) this.MessageType;
        if (!this.AckId.HasValue)
          return string.Format("{0}::{1}:{2}", (object) messageType, (object) this.Endpoint, (object) this.MessageText);
        return this.Callback == null ? string.Format("{0}:{1}:{2}:{3}", (object) messageType, (object) (this.AckId ?? -1), (object) this.Endpoint, (object) this.MessageText) : string.Format("{0}:{1}+:{2}:{3}", (object) messageType, (object) (this.AckId ?? -1), (object) this.Endpoint, (object) this.MessageText);
      }
    }
  }
}
