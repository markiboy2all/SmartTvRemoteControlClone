// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.BaseChannelInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System.IO;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("Channel")]
  public class BaseChannelInfo
  {
    [XmlElement(ElementName = "MajorCh")]
    public int MajorChannel { get; set; }

    [XmlElement(ElementName = "MinorCh")]
    public int MinorChannel { get; set; }

    [XmlElement("ChType")]
    public ChannelType Type { get; set; }

    [XmlElement("ProgNum")]
    public int ProgramNumber { get; set; }

    [XmlElement("PTC")]
    public int PTC { get; set; }

    public static BaseChannelInfo Parse(string value)
    {
      using (StringReader stringReader = new StringReader(value))
        return (BaseChannelInfo) new XmlSerializer(typeof (BaseChannelInfo)).Deserialize((TextReader) stringReader);
    }
  }
}
