// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.StreamInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("SupportStream")]
  public class StreamInfo
  {
    [XmlElement("Container")]
    public string Container { get; set; }

    [XmlElement("VideoFormat")]
    public string VideoFormat { get; set; }

    [XmlElement("AudioFormat")]
    public string AudioFormat { get; set; }

    [XmlElement("XResolution")]
    public int XResolution { get; set; }

    [XmlElement("YResolution")]
    public int YResolution { get; set; }

    [XmlElement("AudioSamplingRate")]
    public int AudioSamplingRate { get; set; }

    [XmlElement("AudioChannel")]
    public int AudioChannels { get; set; }
  }
}
