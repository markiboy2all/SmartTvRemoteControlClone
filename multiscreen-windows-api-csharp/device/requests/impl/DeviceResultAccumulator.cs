// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.impl.DeviceResultAccumulator
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.device.requests.impl
{
  public class DeviceResultAccumulator : DeviceAsyncResult<Device>
  {
    private IList<Device> deviceResults;
    private IList<DeviceError> deviceErrors;
    private DeviceAsyncResult<IList<Device>> callback;
    internal int resultSize;

    public DeviceResultAccumulator(
      IList<Device> resultList,
      IList<DeviceError> errorList,
      int totalSize,
      DeviceAsyncResult<IList<Device>> cb)
    {
      this.deviceResults = resultList;
      this.deviceErrors = errorList;
      this.resultSize = totalSize;
      this.callback = cb;
    }

    public virtual void OnResult(Device device)
    {
      this.deviceResults.Add(device);
      if (this.deviceResults.Count + this.deviceErrors.Count < this.resultSize)
        return;
      this.callback.OnResult(this.deviceResults);
    }

    public virtual void OnError(DeviceError error)
    {
      Logger.Trace("findLocal() onError() error: " + (object) error);
      Console.WriteLine("findLocal() onError() error: " + (object) error);
      this.deviceErrors.Add(error);
      if (this.deviceResults.Count + this.deviceErrors.Count < this.resultSize)
        return;
      this.callback.OnResult(this.deviceResults);
    }
  }
}
