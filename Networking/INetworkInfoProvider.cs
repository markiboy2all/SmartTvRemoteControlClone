// Decompiled with JetBrains decompiler
// Type: Networking.INetworkInfoProvider
// Assembly: Networking, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: E30E8B5A-D94B-466A-9DB8-FA63095FAAFB
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.dll

using System.Collections.Generic;

namespace Networking
{
  public interface INetworkInfoProvider
  {
    IEnumerable<string> GetIPv4NetworkInterfaces();

    IEnumerable<string> GetIPv6LinkLocalNetworkInterfaces();

    IEnumerable<string> GetIPv6SiteLocalNetworkInterfaces();
  }
}
