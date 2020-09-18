// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.AckMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;
using System.Text.RegularExpressions;

namespace SocketIOClient.Messages
{
  public sealed class AckMessage : Message
  {
    private static Regex reAckId = new Regex("^(\\d{1,})");
    private static Regex reAckPayload = new Regex("(?:[\\d\\+]*)(?<data>.*)$");
    private static Regex reAckComplex = new Regex("^\\[(?<payload>.*)\\]$");
    private static object ackLock = new object();
    private static int _akid = 0;
    public Action<object> Callback;

    public static int NextAckID
    {
      get
      {
        lock (AckMessage.ackLock)
        {
          ++AckMessage._akid;
          if (AckMessage._akid < 0)
            AckMessage._akid = 0;
          return AckMessage._akid;
        }
      }
    }

    public AckMessage() => this.MessageType = SocketIOMessageTypes.ACK;

    public static AckMessage Deserialize(string rawMessage)
    {
      AckMessage ackMessage = new AckMessage();
      ackMessage.RawMessage = rawMessage;
      string[] strArray1 = rawMessage.Split(Message.SPLITCHARS, 4);
      if (strArray1.Length == 4)
      {
        ackMessage.Endpoint = strArray1[2];
        string[] strArray2 = strArray1[3].Split('+');
        int result;
        if (strArray2.Length > 1 && int.TryParse(strArray2[0], out result))
        {
          ackMessage.AckId = new int?(result);
          ackMessage.MessageText = strArray2[1];
          Match match = AckMessage.reAckComplex.Match(ackMessage.MessageText);
          if (match.Success)
          {
            ackMessage.Json = new JsonEncodedEventMessage();
            ackMessage.Json.Args = (object[]) new string[1]
            {
              match.Groups["payload"].Value
            };
          }
        }
      }
      return ackMessage;
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
