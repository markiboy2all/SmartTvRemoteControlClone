// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelClient
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace com.samsung.multiscreen.channel
{
  public class ChannelClient
  {
    internal static string KEY_ID = "id";
    internal static string KEY_HOST = "isHost";
    internal static string KEY_CONNECT_TIME = "connectTime";
    internal static string KEY_ATTRIBUTES = "attributes";
    private Channel channel;
    private IDictionary<string, object> @params;

    protected internal ChannelClient(Channel channel, IDictionary<string, object> @params)
    {
      this.channel = channel;
      this.@params = @params;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[ChannelClient]").Append(" id: ").Append(this.GetId()).Append(", isHost: ").Append(this.IsHost()).Append(", connectTime: ").Append(this.GetConnectTime()).Append(", attributes: ").Append(this.GetAttributes().ToString());
      return stringBuilder.ToString();
    }

    public virtual bool IsMe() => this.Equals((object) this.channel.Clients.Me());

    public virtual string GetId() => this.@params.ContainsKey(ChannelClient.KEY_ID) ? (string) this.@params[ChannelClient.KEY_ID] : (string) null;

    public virtual bool IsHost() => (bool) this.@params[ChannelClient.KEY_HOST];

    public virtual long GetConnectTime() => (long) this.@params[ChannelClient.KEY_CONNECT_TIME];

    public virtual IDictionary<string, string> GetAttributes()
    {
      object obj = this.@params[ChannelClient.KEY_ATTRIBUTES];
      if (obj == null)
        return (IDictionary<string, string>) null;
      return obj is JObject ? (IDictionary<string, string>) JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.ToString()) : (IDictionary<string, string>) obj;
    }

    public virtual void Send(string message) => this.Send(message, false);

    public virtual void Send(string message, bool encryptMessage) => this.channel.SendToClient(this, message, encryptMessage);
  }
}
