﻿// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelAsyncResult`1
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

namespace com.samsung.multiscreen.channel
{
  public interface ChannelAsyncResult<T>
  {
    void OnResult(T result);

    void OnError(ChannelError error);
  }
}
