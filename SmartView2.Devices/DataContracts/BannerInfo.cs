// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.BannerInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("BannerInformation")]
  public class BannerInfo
  {
    [XmlElement("CurMode")]
    public string CurrentMode { get; set; }

    [XmlElement("Source")]
    public BaseSourceInfo Source { get; set; }

    [XmlElement("Channel")]
    public BaseChannelInfo Channel { get; set; }

    [XmlElement("DispChNum")]
    public string DisplayChannelNumber { get; set; }

    [XmlElement("DispChName")]
    public string DisplayChannelName { get; set; }

    [XmlElement("ProgTitle")]
    public string ProgramTitle { get; set; }

    [XmlElement("ChInfo")]
    public string ChannelInfo { get; set; }

    [XmlElement("AuxInfo")]
    public string AuxInfo { get; set; }

    [XmlElement("ChListType")]
    public string ChannelListType { get; set; }

    [XmlElement("StartTime")]
    public string StartTime { get; set; }

    [XmlElement("EndTime")]
    public string EndTime { get; set; }

    public static BannerInfo Parse(string value)
    {
      value = value.Replace("<></>", string.Empty);
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(value)))
      {
        BannerInfo bannerInfo = new BannerInfo();
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            if (xmlReader.Name == "CurMode")
            {
              xmlReader.Read();
              bannerInfo.CurrentMode = xmlReader.Value;
            }
            if (xmlReader.Name == "Source" && bannerInfo.Source == null)
              bannerInfo.Source = new BaseSourceInfo();
            if (xmlReader.Name == "ExtSource")
            {
              xmlReader.Read();
              if (bannerInfo.Source == null)
                bannerInfo.Source = new BaseSourceInfo();
              bannerInfo.Source.TypeXml = xmlReader.Value;
            }
            if (xmlReader.Name == "SourceType")
            {
              xmlReader.Read();
              if (bannerInfo.Source == null)
                bannerInfo.Source = new BaseSourceInfo();
              bannerInfo.Source.TypeXml = xmlReader.Value;
            }
            if (xmlReader.Name == "ID")
            {
              xmlReader.Read();
              if (bannerInfo.Source == null)
                bannerInfo.Source = new BaseSourceInfo();
              bannerInfo.Source.Id = Convert.ToInt32(xmlReader.Value);
            }
            if (xmlReader.Name == "Channel" && bannerInfo.Channel == null)
              bannerInfo.Channel = new BaseChannelInfo();
            if (xmlReader.Name == "MajorCh")
            {
              xmlReader.Read();
              if (bannerInfo.Channel == null)
                bannerInfo.Channel = new BaseChannelInfo();
              bannerInfo.Channel.MajorChannel = Convert.ToInt32(xmlReader.Value);
            }
            if (xmlReader.Name == "MinorCh")
            {
              xmlReader.Read();
              if (bannerInfo.Channel == null)
                bannerInfo.Channel = new BaseChannelInfo();
              bannerInfo.Channel.MinorChannel = Convert.ToInt32(xmlReader.Value);
            }
            if (xmlReader.Name == "ChType")
            {
              xmlReader.Read();
              if (bannerInfo.Channel == null)
                bannerInfo.Channel = new BaseChannelInfo();
              bannerInfo.Channel.Type = (ChannelType) Enum.Parse(typeof (ChannelType), xmlReader.Value);
            }
            if (xmlReader.Name == "ProgNum")
            {
              xmlReader.Read();
              if (bannerInfo.Channel == null)
                bannerInfo.Channel = new BaseChannelInfo();
              bannerInfo.Channel.ProgramNumber = Convert.ToInt32(xmlReader.Value);
            }
            if (xmlReader.Name == "PTC")
            {
              xmlReader.Read();
              if (bannerInfo.Channel == null)
                bannerInfo.Channel = new BaseChannelInfo();
              bannerInfo.Channel.PTC = Convert.ToInt32(xmlReader.Value);
            }
            if (xmlReader.Name == "DispChNum")
            {
              xmlReader.Read();
              bannerInfo.DisplayChannelNumber = xmlReader.Value;
            }
            if (xmlReader.Name == "DispChName")
            {
              xmlReader.Read();
              bannerInfo.DisplayChannelName = xmlReader.Value;
            }
            if (xmlReader.Name == "ProgTitle")
            {
              xmlReader.Read();
              bannerInfo.ProgramTitle = xmlReader.Value;
            }
            if (xmlReader.Name == "ChInfo")
            {
              xmlReader.Read();
              bannerInfo.ChannelInfo = xmlReader.Value;
            }
            if (xmlReader.Name == "ChListType")
            {
              xmlReader.Read();
              bannerInfo.ChannelListType = xmlReader.Value;
            }
            if (xmlReader.Name == "StartTime")
            {
              xmlReader.Read();
              bannerInfo.StartTime = xmlReader.Value;
            }
            if (xmlReader.Name == "EndTime")
            {
              xmlReader.Read();
              bannerInfo.EndTime = xmlReader.Value;
            }
            if (xmlReader.Name == "AuxInfo")
            {
              xmlReader.Read();
              bannerInfo.AuxInfo = xmlReader.Value;
            }
          }
        }
        return bannerInfo;
      }
    }
  }
}
