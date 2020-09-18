// Decompiled with JetBrains decompiler
// Type: UPnP.PropertyChangedEventArgs
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;

namespace UPnP
{
  public class PropertyChangedEventArgs : EventArgs
  {
    public PropertyChangedEventArgs()
      : this(string.Empty, string.Empty)
    {
    }

    public PropertyChangedEventArgs(string property, string newValue)
    {
      this.Property = property;
      this.NewValue = newValue;
    }

    public string Property { get; set; }

    public string NewValue { get; set; }
  }
}
