// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.ssdp.SSDPSearchListener
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System.Collections.Generic;

namespace com.samsung.multiscreen.net.ssdp
{
  public interface SSDPSearchListener
  {
    void OnResult(SSDPSearchResult result);

    void OnResults(IList<SSDPSearchResult> results);
  }
}
