// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.EditNumMode
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("EditNumMode")]
  public class EditNumMode
  {
    [XmlElement("NumMode")]
    public string NumMode { get; set; }

    [XmlElement("MinValue")]
    public int MinValue { get; set; }

    [XmlElement("MaxValue")]
    public int MaxValue { get; set; }
  }
}
