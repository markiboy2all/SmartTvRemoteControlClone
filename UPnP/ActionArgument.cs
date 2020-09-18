// Decompiled with JetBrains decompiler
// Type: UPnP.ActionArgument
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

namespace UPnP
{
  public class ActionArgument
  {
    public ActionArgument()
      : this(string.Empty, (object) null)
    {
    }

    public ActionArgument(string name, object value)
    {
      this.Name = name;
      this.Value = value;
    }

    public string Name { get; set; }

    public object Value { get; set; }
  }
}
