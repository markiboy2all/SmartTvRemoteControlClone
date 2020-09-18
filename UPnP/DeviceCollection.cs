// Decompiled with JetBrains decompiler
// Type: UPnP.DeviceCollection
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System.Collections;
using System.Collections.Generic;

namespace UPnP
{
  public class DeviceCollection : ICollection<IUPnPDevice>, IEnumerable<IUPnPDevice>, IEnumerable
  {
    private readonly IDictionary<string, IUPnPDevice> devices = (IDictionary<string, IUPnPDevice>) new Dictionary<string, IUPnPDevice>();

    public void Add(IUPnPDevice item) => this.devices.Add(item.ID, item);

    public void Clear() => this.devices.Clear();

    public bool Contains(IUPnPDevice item) => this.devices.ContainsKey(item.ID);

    public bool Contains(string id) => this.devices.ContainsKey(id);

    public void CopyTo(IUPnPDevice[] array, int arrayIndex) => this.devices.Values.CopyTo(array, arrayIndex);

    public bool Remove(IUPnPDevice item) => this.devices.Remove(item.ID);

    public bool Remove(string id) => this.devices.Remove(id);

    public IEnumerator<IUPnPDevice> GetEnumerator() => this.devices.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.devices.Values.GetEnumerator();

    public IUPnPDevice this[string id] => this.devices[id];

    public int Count => this.devices.Count;

    public bool IsReadOnly => this.devices.IsReadOnly;
  }
}
