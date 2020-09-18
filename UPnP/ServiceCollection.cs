// Decompiled with JetBrains decompiler
// Type: UPnP.ServiceCollection
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System.Collections;
using System.Collections.Generic;

namespace UPnP
{
  public class ServiceCollection : ICollection<IUPnPService>, IEnumerable<IUPnPService>, IEnumerable
  {
    private readonly IDictionary<string, IUPnPService> services = (IDictionary<string, IUPnPService>) new Dictionary<string, IUPnPService>();

    public void Add(IUPnPService item) => this.services.Add(item.ID, item);

    public void Clear() => this.services.Clear();

    public bool Contains(IUPnPService item) => this.services.Values.Contains(item);

    public bool Contains(string id) => this.services.ContainsKey(id);

    public void CopyTo(IUPnPService[] array, int arrayIndex) => this.services.Values.CopyTo(array, arrayIndex);

    public bool Remove(IUPnPService item) => this.services.Remove(item.ID);

    public bool Remove(string id) => this.services.Remove(id);

    public IEnumerator<IUPnPService> GetEnumerator() => this.services.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.services.GetEnumerator();

    public IUPnPService this[string id] => this.services[id];

    public int Count => this.services.Count;

    public bool IsReadOnly => this.services.IsReadOnly;
  }
}
