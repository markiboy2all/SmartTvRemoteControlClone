// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.ChannelInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("Channel")]
  public class ChannelInfo
  {
    [XmlElement("ChannelType")]
    public string TypeXml { get; set; }

    public ChannelType Type => (ChannelType) Convert.ToInt32(this.TypeXml, 16);

    [XmlElement(typeof (int), ElementName = "MajorChannel")]
    public int MajorChannel { get; set; }

    [XmlElement("MinorChannel")]
    public int MinorChannel { get; set; }

    [XmlElement("PTC")]
    public int PTC { get; set; }

    [XmlElement("ProgramNumber")]
    public int ProgramNumber { get; set; }

    [XmlElement("SatelliteId")]
    public int SatelliteId { get; set; }

    [XmlElement("DisplayChannelNumber")]
    public string DisplayChannelNumber { get; set; }

    [XmlElement("ChannelInformation")]
    public string ChannelInformation { get; set; }

    [XmlElement("ChannelNameLengt")]
    public int NameLength { get; set; }

    [XmlElement("DisplayChannelName")]
    public string DisplayChannelName { get; set; }

    [XmlElement("TSID")]
    public int TSID { get; set; }

    [XmlElement("PredefinedChannelNumber")]
    public string PredefinedChannelNumber { get; set; }

    [XmlElement("AstraHD")]
    public string AstraHD { get; set; }

    [XmlElement("AstraHD_BouquetInfo")]
    public string AstraHD_BouquetInfo { get; set; }

    [XmlElement("Favorite1Order")]
    public string Favorite1 { get; set; }

    [XmlElement("Favorite2Order")]
    public string Favorite2 { get; set; }

    [XmlElement("Favorite3Order")]
    public string Favorite3 { get; set; }

    [XmlElement("Favorite4Order")]
    public string Favorite4 { get; set; }

    [XmlElement("Favorite5Order")]
    public string Favorite5 { get; set; }

    internal BaseChannelInfo ToBase() => new BaseChannelInfo()
    {
      MajorChannel = this.MajorChannel,
      MinorChannel = this.MinorChannel,
      ProgramNumber = this.ProgramNumber,
      PTC = this.PTC,
      Type = this.Type
    };
  }
}
