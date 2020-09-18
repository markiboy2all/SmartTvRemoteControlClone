// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.BaseSourceInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  public class BaseSourceInfo
  {
    public int MbrActivityIndex { get; set; }

    [XmlElement("SourceType")]
    public string TypeXml { get; set; }

    public SourceType Type
    {
      get
      {
        SourceType result = SourceType.UNKNOWN_SOURCE;
        return Enum.TryParse<SourceType>(this.TypeXml.Replace('/', '_'), out result) ? result : SourceType.UNKNOWN_SOURCE;
      }
      set => this.TypeXml = value.ToString().Replace('_', '/');
    }

    [XmlElement("ID")]
    public int Id { get; set; }
  }
}
