// Decompiled with JetBrains decompiler
// Type: UPnP.UPnPMessageHelper
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;

namespace UPnP
{
  internal static class UPnPMessageHelper
  {
    public static string Location(this HttpMessage message) => message["location"];

    public static bool HasLocation(this HttpMessage message) => !string.IsNullOrEmpty(message.Location());

    public static string UniqueServiceName(this HttpMessage message) => message["usn"];

    public static bool HasUniqueServiceName(this HttpMessage message) => !string.IsNullOrEmpty(message.UniqueServiceName());

    public static string NotificationType(this HttpMessage message) => message["nt"];

    public static bool HasNotificationType(this HttpMessage message) => !string.IsNullOrEmpty(message["nt"]);

    public static string NotificationSubType(this HttpMessage message) => message["nts"];
  }
}
