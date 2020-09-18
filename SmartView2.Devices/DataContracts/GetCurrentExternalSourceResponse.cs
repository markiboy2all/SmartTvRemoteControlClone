// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.GetCurrentExternalSourceResponse
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("GetCurrentExternalSourceResponse")]
  public class GetCurrentExternalSourceResponse : SecondTvResponse
  {
    [XmlElement("CurrentExternalSource")]
    public string CurrentExternalSource { get; set; }

    [XmlElement("ID")]
    public int ID { get; set; }

    [XmlElement("CurrentMBRActivityIndex")]
    public int CurrentMBRActivityIndex { get; set; }
  }
}
