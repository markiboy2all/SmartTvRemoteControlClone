// Decompiled with JetBrains decompiler
// Type: DateTimeHelperClass
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;

internal static class DateTimeHelperClass
{
  private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  internal static long CurrentUnixTimeMillis() => (long) (DateTime.UtcNow - DateTimeHelperClass.Jan1st1970).TotalMilliseconds;
}
