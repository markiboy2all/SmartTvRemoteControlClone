// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.util.Logger
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;
using System.Diagnostics;

namespace com.samsung.multiscreen.util
{
  internal class Logger
  {
    private static bool DEBUG;

    public static void Trace(Exception ex) => Debugger.Log(0, (string) null, ex.ToString() + Environment.NewLine);

    public static void Trace(string log) => Debugger.Log(0, (string) null, log + Environment.NewLine);

    public static void Debug(string msg)
    {
      if (!Logger.DEBUG)
        return;
      Console.WriteLine(msg);
    }
  }
}
