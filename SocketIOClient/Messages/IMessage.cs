// Decompiled with JetBrains decompiler
// Type: SocketIOClient.Messages.IMessage
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;

namespace SocketIOClient.Messages
{
  public interface IMessage
  {
    SocketIOMessageTypes MessageType { get; }

    string RawMessage { get; }

    string Event { get; }

    int? AckId { get; }

    string Endpoint { get; set; }

    string MessageText { get; }

    JsonEncodedEventMessage Json { get; }

    [Obsolete(".JsonEncodedMessage has been deprecated. Please use .Json instead.")]
    JsonEncodedEventMessage JsonEncodedMessage { get; }

    string Encoded { get; }
  }
}
