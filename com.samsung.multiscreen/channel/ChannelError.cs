// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelError
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;
using System.Collections.Generic;

namespace com.samsung.multiscreen.channel
{
  public class ChannelError
  {
    protected internal ChannelError()
    {
    }

    protected internal ChannelError(int code, string message)
    {
      this.Code = code;
      this.Message = message;
    }

    public ChannelError(string message) => this.Message = message;

    public override string ToString() => "[ChannelError]" + " code: " + (object) this.Code + ", message: " + this.Message;

    public virtual int Code { get; set; }

    public virtual string Message { get; set; }

    public static ChannelError CreateWithJSONData(string data)
    {
      IDictionary<string, object> dictionary = JSONUtil.Parse(data);
      return new ChannelError()
      {
        Code = (int) dictionary["code"],
        Message = (string) dictionary["message"]
      };
    }
  }
}
