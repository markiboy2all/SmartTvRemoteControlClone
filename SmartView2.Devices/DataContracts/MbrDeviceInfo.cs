// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.MbrDeviceInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("MBRDevice")]
  public class MbrDeviceInfo
  {
    [XmlElement("ActivityIndex")]
    public int ActivityIndex { get; set; }

    [XmlElement("SourceType")]
    public string SourceType { get; set; }

    [XmlElement("ID")]
    public int Id { get; set; }

    [XmlElement("DeviceType")]
    public string DeviceType { get; set; }

    [XmlElement("BrandName")]
    public string BrandName { get; set; }

    [XmlElement("ModelNumber")]
    public string ModelNumber { get; set; }
  }
}
