// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.SourceInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("Source")]
  public class SourceInfo : BaseSourceInfo
  {
    [XmlElement("Editable")]
    public string XmlEditable { get; set; }

    [XmlIgnore]
    public bool Editable => this.XmlEditable.ToLower() == "yes";

    [XmlElement("EditNameType")]
    public string EditNameType { get; set; }

    [XmlElement("DeviceName")]
    public string DeviceName { get; set; }

    [XmlElement("Connected")]
    public string XmlConnected { get; set; }

    public bool Connected => this.XmlConnected.ToLower() == "yes";

    [XmlElement("SupportView")]
    public string XmlSupportView { get; set; }

    public bool SupportView => this.XmlSupportView.ToLower() == "yes";
  }
}
