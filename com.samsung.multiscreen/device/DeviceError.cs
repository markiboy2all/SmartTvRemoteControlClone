// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.DeviceError
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.device
{
  public class DeviceError
  {
    private long code = -1;
    private string message = "error";

    public DeviceError()
    {
    }

    public DeviceError(string message)
      : this(-1L, message)
    {
    }

    protected internal DeviceError(long code, string message)
    {
      this.code = code;
      this.message = message;
    }

    public override string ToString() => "[DeviceError]" + " code: " + (object) this.code + ", message: " + this.message;

    public virtual long Code => this.code;

    public virtual string Message => this.message;

    public static DeviceError CreateWithJSONData(string data)
    {
      IDictionary<string, object> dictionary = JSONUtil.Parse(data);
      return new DeviceError()
      {
        code = (long) dictionary["code"],
        message = (string) dictionary["message"]
      };
    }

    public static DeviceError CreateWithJSONRPCError(JSONRPCError rpcError) => new DeviceError(rpcError.Code, rpcError.Message);

    public static DeviceError CreateWithException(Exception e) => new DeviceError(e.ToString());
  }
}
