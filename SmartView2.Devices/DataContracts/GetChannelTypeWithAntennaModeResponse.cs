// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.GetChannelTypeWithAntennaModeResponse
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SmartView2.Devices.DataContracts
{
  [XmlRoot("GetChannelTypeWithAntennaMode")]
  public class GetChannelTypeWithAntennaModeResponse : SecondTvResponse
  {
    public ChannelType[] ChannelTypes { get; set; }

    public static GetChannelTypeWithAntennaModeResponse Parse(
      string value)
    {
      string empty = string.Empty;
      List<ChannelType> channelTypeList = new List<ChannelType>();
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(value)))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            if (xmlReader.Name == "Result")
            {
              xmlReader.Read();
              empty = xmlReader.Value;
            }
            else if (xmlReader.Name == "ChannelType")
            {
              xmlReader.Read();
              ChannelType int32 = (ChannelType) Convert.ToInt32(xmlReader.Value, 16);
              channelTypeList.Add(int32);
            }
          }
        }
      }
      GetChannelTypeWithAntennaModeResponse antennaModeResponse = new GetChannelTypeWithAntennaModeResponse();
      antennaModeResponse.Result = empty;
      antennaModeResponse.ChannelTypes = channelTypeList.ToArray();
      return antennaModeResponse;
    }
  }
}
