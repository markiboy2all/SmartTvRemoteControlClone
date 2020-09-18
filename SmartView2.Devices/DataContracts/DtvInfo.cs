// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.DtvInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("DTVInformation")]
  public class DtvInfo
  {
    [XmlElement("SupportTVVersion")]
    public int TvVersion { get; set; }

    [XmlElement("SupportGetAvailableActions")]
    public string XmlAvailableActions { get; set; }

    [XmlIgnore]
    public bool AvailableActions => this.XmlAvailableActions.ToLower() == "yes";

    [XmlElement("SupportBluetooth")]
    public string XmlBluetooth { get; set; }

    [XmlIgnore]
    public bool Bluetooth => this.XmlBluetooth.ToLower() == "yes";

    [XmlElement("TargetLocation")]
    public string TargetLocation { get; set; }

    [XmlElement("SupportAntMode")]
    public string XmlAntennaMode { get; set; }

    [XmlIgnore]
    public IEnumerable<SmartView2.Devices.DataContracts.AntennaMode> AntennaMode
    {
      get
      {
        string[] strArray = this.XmlAntennaMode.Split(',');
        List<SmartView2.Devices.DataContracts.AntennaMode> antennaModeList = new List<SmartView2.Devices.DataContracts.AntennaMode>();
        try
        {
          foreach (string str in strArray)
            antennaModeList.Add((SmartView2.Devices.DataContracts.AntennaMode) Convert.ToInt32(str, 16));
        }
        catch (FormatException ex)
        {
          Logger.Instance.LogMessageFormat("Invalid convert to AntennaMode {0}", (object) this.XmlAntennaMode);
          throw ex;
        }
        return (IEnumerable<SmartView2.Devices.DataContracts.AntennaMode>) antennaModeList;
      }
    }

    [XmlElement("CIOPOneName")]
    public string CIOPOneName { get; set; }

    [XmlElement("CIOPTwoName")]
    public string CIOPTwoName { get; set; }

    [XmlElement("SupportChSort")]
    public string XmlChannelSort { get; set; }

    [XmlIgnore]
    public bool ChannelSort => this.XmlChannelSort.ToLower() == "yes";

    [XmlElement("SupportChannelLock")]
    public string XmlChannelLock { get; set; }

    [XmlIgnore]
    public bool ChannelLock => this.XmlChannelLock.ToLower() == "yes";

    [XmlElement("SupportChannelInfo")]
    public string XmlChannelInfo { get; set; }

    [XmlIgnore]
    public bool ChannelInfo => this.XmlChannelInfo.ToLower() == "yes";

    [XmlElement("SupportChannelDelete")]
    public string XmlChannelDelete { get; set; }

    [XmlIgnore]
    public bool ChannelDelete => this.XmlChannelDelete.ToLower() == "yes";

    [XmlArray("SupportEditNumMode")]
    [XmlArrayItem("EditNumMode")]
    private EditNumMode[] EditNumModes { get; set; }

    [XmlElement("SupportRegionalVariant")]
    public string XmlRegionalVariant { get; set; }

    [XmlIgnore]
    public bool RegionalVariant => this.XmlRegionalVariant.ToLower() == "yes";

    [XmlElement("SupportPVR")]
    public string XmlPvr { get; set; }

    [XmlIgnore]
    public bool Pvr => this.XmlPvr.ToLower() == "yes";

    [XmlElement("NumberOfRecord")]
    public int NumberOfRecord { get; set; }

    [XmlElement("SupportDTV")]
    public string XmlDtv { get; set; }

    [XmlIgnore]
    public bool Dtv => this.XmlDtv.ToLower() == "yes";

    [XmlElement("SupportStream")]
    public StreamInfo Stream { get; set; }

    [XmlElement("SupportDRMType")]
    public string XmlDrmType { get; set; }

    [XmlIgnore]
    public IEnumerable<SmartView2.Devices.DataContracts.DrmType> DrmType
    {
      get
      {
        string[] strArray = this.XmlDrmType.Split(',');
        List<SmartView2.Devices.DataContracts.DrmType> drmTypeList = new List<SmartView2.Devices.DataContracts.DrmType>();
        try
        {
          foreach (string str in strArray)
          {
            SmartView2.Devices.DataContracts.DrmType result = SmartView2.Devices.DataContracts.DrmType.HDCP;
            if (Enum.TryParse<SmartView2.Devices.DataContracts.DrmType>(str, out result))
              drmTypeList.Add(result);
          }
        }
        catch (FormatException ex)
        {
          Logger.Instance.LogMessageFormat("Invalid convert to DrmType {0}", (object) this.XmlDrmType);
          throw ex;
        }
        return (IEnumerable<SmartView2.Devices.DataContracts.DrmType>) drmTypeList;
      }
    }

    [XmlElement("SupportGUICloneView")]
    public string XmlGuiCloneView { get; set; }

    [XmlIgnore]
    public bool GuiCloneView => this.XmlGuiCloneView.ToLower() == "yes";

    public static DtvInfo Parse(string value)
    {
      using (StringReader stringReader = new StringReader(value))
      {
        try
        {
          return (DtvInfo) new XmlSerializer(typeof (DtvInfo)).Deserialize((TextReader) stringReader);
        }
        catch (Exception ex)
        {
          return (DtvInfo) null;
        }
      }
    }
  }
}
