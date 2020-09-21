// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.info.ChannelInfo
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.json;
using System.Collections.Generic;
using System.Text;

namespace com.samsung.multiscreen.channel.info
{
  public class ChannelInfo
  {
    private static string KEY_ID = "id";
    private static string KEY_ENDPOINT = "endpoint";
    private static string KEY_HOSTCONNECTED = "hostConnected";
    private IDictionary<string, object> @params;

    protected internal ChannelInfo(IDictionary<string, object> @params) => this.@params = @params;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[ChannelInfo]").Append(" ").Append(ChannelInfo.KEY_ID).Append(": ").Append(this.Id).Append(", ").Append(ChannelInfo.KEY_ENDPOINT).Append(": ").Append(this.EndPoint);
      return stringBuilder.ToString();
    }

    public virtual string Id
    {
      get
      {
        string str = (string) null;
        if (this.@params.ContainsKey(ChannelInfo.KEY_ID))
          str = (string) this.@params[ChannelInfo.KEY_ID];
        return str;
      }
    }

    public virtual string EndPoint
    {
      get
      {
        string str = (string) null;
        if (this.@params.ContainsKey(ChannelInfo.KEY_ENDPOINT))
          str = (string) this.@params[ChannelInfo.KEY_ENDPOINT];
        return str;
      }
    }

    public virtual bool HostConnected
    {
      get
      {
        bool flag = false;
        if (this.@params.ContainsKey(ChannelInfo.KEY_HOSTCONNECTED))
          flag = (bool) this.@params[ChannelInfo.KEY_HOSTCONNECTED];
        return flag;
      }
    }

    protected internal static ChannelInfo createWithJSONData(string jsonData) => ChannelInfo.createWithMap(JSONUtil.Parse(jsonData));

    public static ChannelInfo createWithMap(IDictionary<string, object> map) => new ChannelInfo(map);
  }
}
