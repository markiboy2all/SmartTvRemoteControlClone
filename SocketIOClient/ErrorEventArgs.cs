// Decompiled with JetBrains decompiler
// Type: SocketIOClient.ErrorEventArgs
// Assembly: SocketIOClient, Version=0.6.26.0, Culture=neutral, PublicKeyToken=null
// MVID: 376B7D7E-E5E4-4C83-8279-001ABBB3A959
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SocketIOClient.dll

using System;

namespace SocketIOClient
{
  public class ErrorEventArgs : EventArgs
  {
    public string Message { get; set; }

    public Exception Exception { get; set; }

    public ErrorEventArgs(string message) => this.Message = message;

    public ErrorEventArgs(string message, Exception exception)
    {
      this.Message = message;
      this.Exception = exception;
    }
  }
}
