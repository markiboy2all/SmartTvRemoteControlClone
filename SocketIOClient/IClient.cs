// Decompiled with JetBrains decompiler
// Type: SocketIOClient.IClient
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using SocketIOClient.Messages;
using System;
using WebSocket4Net;

namespace SocketIOClient
{
  internal interface IClient
  {
    event EventHandler Opened;

    event EventHandler<MessageEventArgs> Message;

    event EventHandler SocketConnectionClosed;

    event EventHandler<ErrorEventArgs> Error;

    SocketIOHandshake HandShake { get; }

    bool IsConnected { get; }

    WebSocketState ReadyState { get; }

    void Connect();

    IEndPointClient Connect(string endPoint);

    void Close();

    void Dispose();

    void On(string eventName, Action<IMessage> action);

    void On(string eventName, string endPoint, Action<IMessage> action);

    void Emit(string eventName, object payload);

    void Emit(string eventName, object payload, string endPoint = "", Action<object> callBack = null);

    void Send(IMessage msg);
  }
}
