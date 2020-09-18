// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.ProgramInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.IO;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("DetailProgramInformation")]
  public class ProgramInfo
  {
    [XmlElement("ProgTitle")]
    public string ProgramTitle { get; set; }

    [XmlElement("StartTime")]
    public string StartTime { get; set; }

    [XmlElement("EndTime")]
    public string EndTime { get; set; }

    [XmlElement("DetailInfo")]
    public string DetailInfo { get; set; }

    [XmlElement("Genre")]
    public string Genre { get; set; }

    [XmlElement("SeriesID")]
    public string SeriesId { get; set; }

    [XmlElement("ChInfo")]
    public string ChannelInfo { get; set; }

    [XmlElement("FreeCAMode")]
    public string FreeCAMode { get; set; }

    internal static ProgramInfo Parse(string value)
    {
      using (StringReader stringReader = new StringReader(value))
        return (ProgramInfo) new XmlSerializer(typeof (ProgramInfo)).Deserialize((TextReader) stringReader);
    }
  }
}
