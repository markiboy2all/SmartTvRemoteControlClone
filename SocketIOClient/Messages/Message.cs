// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.Message
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace SocketIOClient.Messages
{
  public abstract class Message : IMessage
  {
    private static Regex re = new Regex("\\d:\\d?:\\w?:");
    public static char[] SPLITCHARS = new char[1]{ ':' };
    private JsonEncodedEventMessage _json;
    public static Regex reMessageType = new Regex("^[0-8]{1}:", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public string RawMessage { get; protected set; }

    public SocketIOMessageTypes MessageType { get; protected set; }

    public int? AckId { get; set; }

    public string Endpoint { get; set; }

    public string MessageText { get; set; }

    [Obsolete(".JsonEncodedMessage has been deprecated. Please use .Json instead.")]
    public JsonEncodedEventMessage JsonEncodedMessage
    {
      get => this.Json;
      set => this._json = value;
    }

    public JsonEncodedEventMessage Json
    {
      get
      {
        if (this._json == null)
          this._json = string.IsNullOrEmpty(this.MessageText) || !this.MessageText.Contains("name") || !this.MessageText.Contains("args") ? new JsonEncodedEventMessage() : JsonEncodedEventMessage.Deserialize(this.MessageText);
        return this._json;
      }
      set => this._json = value;
    }

    public virtual string Event { get; set; }

    public virtual string Encoded
    {
      get
      {
        int messageType = (int) this.MessageType;
        if (!this.AckId.HasValue)
          return string.Format("{0}::{1}:{2}", (object) messageType, (object) this.Endpoint, (object) this.MessageText);
        return string.Format("{0}:{1}:{2}:{3}", (object) messageType, (object) (this.AckId ?? -1), (object) this.Endpoint, (object) this.MessageText);
      }
    }

    public Message() => this.MessageType = SocketIOMessageTypes.Message;

    public Message(string rawMessage)
      : this()
    {
      this.RawMessage = rawMessage;
      string[] strArray = rawMessage.Split(Message.SPLITCHARS, 4);
      if (strArray.Length != 4)
        return;
      int result;
      if (int.TryParse(strArray[1], out result))
        this.AckId = new int?(result);
      this.Endpoint = strArray[2];
      this.MessageText = strArray[3];
    }

    public static IMessage Factory(string rawMessage)
    {
      if (Message.reMessageType.IsMatch(rawMessage))
      {
        switch (rawMessage.First<char>())
        {
          case '0':
            return (IMessage) DisconnectMessage.Deserialize(rawMessage);
          case '1':
            return (IMessage) ConnectMessage.Deserialize(rawMessage);
          case '2':
            return (IMessage) new Heartbeat();
          case '3':
            return (IMessage) TextMessage.Deserialize(rawMessage);
          case '4':
            return (IMessage) JSONMessage.Deserialize(rawMessage);
          case '5':
            return (IMessage) EventMessage.Deserialize(rawMessage);
          case '6':
            return (IMessage) AckMessage.Deserialize(rawMessage);
          case '7':
            return (IMessage) ErrorMessage.Deserialize(rawMessage);
          case '8':
            return (IMessage) new NoopMessage();
          default:
            Trace.WriteLine(string.Format("Message.Factory undetermined message: {0}", (object) rawMessage));
            return (IMessage) new TextMessage();
        }
      }
      else
      {
        Trace.WriteLine(string.Format("Message.Factory did not find matching message type: {0}", (object) rawMessage));
        return (IMessage) new NoopMessage();
      }
    }
  }
}
