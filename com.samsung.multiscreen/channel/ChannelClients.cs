// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.ChannelClients
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace com.samsung.multiscreen.channel
{
  public class ChannelClients
  {
    private IDictionary<string, ChannelClient> clientsMap;
    private string myClientId;

    protected internal ChannelClients() => this.clientsMap = (IDictionary<string, ChannelClient>) new Dictionary<string, ChannelClient>();

    protected internal virtual void Clear()
    {
      this.myClientId = (string) null;
      this.clientsMap.Clear();
    }

    protected internal virtual void Reset(string myClientId, IList<ChannelClient> clientList)
    {
      this.myClientId = myClientId;
      this.clientsMap.Clear();
      foreach (ChannelClient client in (IEnumerable<ChannelClient>) clientList)
        this.clientsMap[client.GetId()] = client;
    }

    protected internal virtual void add(ChannelClient client) => this.clientsMap[client.GetId()] = client;

    protected internal virtual void Remove(ChannelClient client) => this.clientsMap.Remove(client.GetId());

    public virtual ChannelClient Me() => this.Get(this.myClientId);

    public virtual ChannelClient Get(string id) => this.clientsMap[id];

    public virtual ChannelClient Host()
    {
      IEnumerator<ChannelClient> enumerator = this.clientsMap.Values.GetEnumerator();
      while (enumerator.MoveNext())
      {
        ChannelClient current = enumerator.Current;
        if (current.IsHost())
          return current;
      }
      return (ChannelClient) null;
    }

    public virtual IList<ChannelClient> List() => (IList<ChannelClient>) new ReadOnlyCollection<ChannelClient>((IList<ChannelClient>) new System.Collections.Generic.List<ChannelClient>((IEnumerable<ChannelClient>) this.clientsMap.Values));

    public virtual int Size() => this.clientsMap.Count;
  }
}
