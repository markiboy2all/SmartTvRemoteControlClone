// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.impl.ReachableDeviceHandler
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.util;

namespace com.samsung.multiscreen.device.requests.impl
{
  public class ReachableDeviceHandler : DeviceAsyncResult<Device>
  {
    private Device defaultResult;
    private DeviceAsyncResult<Device> callback;

    public ReachableDeviceHandler(Device defaultResult, DeviceAsyncResult<Device> callback)
    {
      this.defaultResult = defaultResult;
      this.callback = callback;
    }

    public void OnResult(Device localDevice)
    {
      Logger.Trace("ReachableDeviceHandler() onResult() localDevice\n: " + (object) localDevice);
      if (localDevice != null)
        this.callback.OnResult(localDevice);
      else
        this.callback.OnResult(this.defaultResult);
    }

    public void OnError(DeviceError error)
    {
      Logger.Trace("ReachableDeviceHandler() onError() error: " + (object) error);
      this.callback.OnResult(this.defaultResult);
    }
  }
}
