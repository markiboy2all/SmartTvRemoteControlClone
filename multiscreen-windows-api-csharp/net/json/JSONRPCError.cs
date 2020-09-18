// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.json.JSONRPCError
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.net.json
{
  public class JSONRPCError
  {
    private long code = -1;
    private string message = "error";

    public JSONRPCError()
    {
    }

    public JSONRPCError(long code, string message)
    {
      this.code = code;
      this.message = message;
    }

    public override string ToString() => "[JSONRPCError]" + " code: " + (object) this.code + ", message: " + this.message;

    public virtual long Code => this.code;

    public virtual string Message => this.message;

    public static JSONRPCError CreateWithJSONData(string data) => JSONRPCError.CreateWithMap(JSONUtil.Parse(data));

    public static JSONRPCError CreateWithMap(IDictionary<string, object> data)
    {
      JSONRPCError jsonrpcError = new JSONRPCError();
      try
      {
        jsonrpcError.code = (long) data["code"];
        jsonrpcError.message = (string) data["message"];
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      return jsonrpcError;
    }
  }
}
