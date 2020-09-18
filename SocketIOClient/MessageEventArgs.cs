// Decompiled with JetBrains decompiler
// Type: SocketIOClient.MessageEventArgs
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using SocketIOClient.Messages;
using System;

namespace SocketIOClient
{
  public class MessageEventArgs : EventArgs
  {
    public IMessage Message { get; private set; }

    public MessageEventArgs(IMessage msg) => this.Message = msg;
  }
}
