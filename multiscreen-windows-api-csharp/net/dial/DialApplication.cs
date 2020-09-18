// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.dial.DialApplication
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System.Collections.Generic;
using System.Text;

namespace com.samsung.multiscreen.net.dial
{
  public class DialApplication
  {
    internal IDictionary<string, string> options;

    public DialApplication() => this.options = (IDictionary<string, string>) new Dictionary<string, string>();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[DialApplication]").Append(" name: ").Append(this.Name).Append(", state: ").Append(this.State).Append(", relLink: ").Append(this.RelLink).Append(", hrefLink: ").Append(this.HrefLink).Append(", stopAllowed:").Append(this.StopAllowed);
      return stringBuilder.ToString();
    }

    public virtual string Name { get; set; }

    public virtual string State { get; set; }

    public virtual bool StopAllowed { get; set; }

    public virtual string RelLink { get; set; }

    public virtual string HrefLink { get; set; }

    public virtual void SetOption(string name, string value) => this.options[name] = value;

    public virtual IDictionary<string, string> Options => this.options;
  }
}
