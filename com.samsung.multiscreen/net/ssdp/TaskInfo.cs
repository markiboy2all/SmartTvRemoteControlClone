// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.ssdp.TaskInfo
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace com.samsung.multiscreen.net.ssdp
{
  internal class TaskInfo
  {
    internal RegisteredWaitHandle Handle;
    internal IPEndPoint iep;
    internal string data;
    internal Socket socket;
  }
}
