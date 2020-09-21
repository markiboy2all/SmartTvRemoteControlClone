// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.impl.DeviceURIResultAccumulator
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.device.requests.impl
{
  public class DeviceURIResultAccumulator : AsyncResult<DeviceURIResult>
  {
    private IList<DeviceURIResult> results;
    private int count;
    private AsyncResult<IList<DeviceURIResult>> callback;

    public DeviceURIResultAccumulator(int totalSize, AsyncResult<IList<DeviceURIResult>> callback)
    {
      this.count = 0;
      this.results = (IList<DeviceURIResult>) new List<DeviceURIResult>();
      this.callback = callback;
    }

    public void onResult(DeviceURIResult result)
    {
      this.results.Add(result);
      --this.count;
      if (this.count > 0)
        return;
      this.callback.onResult(this.results);
    }

    public void onException(Exception e)
    {
      --this.count;
      if (this.count > 0)
        return;
      this.callback.onResult(this.results);
    }
  }
}
