// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.GetChannelListResponse
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("GetChannelListResponse")]
  public class GetChannelListResponse : SecondTvResponse
  {
    [XmlArray(ElementName = "ChannelList", IsNullable = true)]
    [XmlArrayItem("Channel")]
    public ChannelInfo[] ChannelList { get; set; }

    [XmlElement("ChannelListType")]
    public string ChannelListType { get; set; }

    [XmlElement("SatelliteId")]
    public int SatelliteId { get; set; }

    public static GetChannelListResponse Parse(string value)
    {
      value = value.Replace("AstraHD+BouquetInfo", "AstraHD_BouquetInfo");
      Regex regex = new Regex("<ChannelCount>(?<Count>\\d+)</ChannelCount>");
      string str = regex.Match(value).Groups["Count"].Value;
      value = regex.Replace(value, string.Empty);
      return SecondTvResponse.Parse<GetChannelListResponse>(value);
    }
  }
}
