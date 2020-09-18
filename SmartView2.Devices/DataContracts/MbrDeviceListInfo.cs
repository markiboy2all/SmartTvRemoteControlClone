// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.MbrDeviceListInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SmartView2.Devices.DataContracts
{
  public class MbrDeviceListInfo
  {
    public MbrDeviceInfo[] MbrList { get; set; }

    public static MbrDeviceListInfo Parse(string xml)
    {
      if (xml == null)
        return (MbrDeviceListInfo) null;
      MbrDeviceListInfo mbrDeviceListInfo = new MbrDeviceListInfo();
      List<MbrDeviceInfo> mbrDeviceInfoList = new List<MbrDeviceInfo>();
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(xml)))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "MBRDevice")
          {
            MbrDeviceInfo mbrDeviceInfo = new MbrDeviceInfo();
            while (xmlReader.Read())
            {
              if (xmlReader.NodeType == XmlNodeType.Element)
              {
                if (xmlReader.Name == "MBRDevice")
                  mbrDeviceInfo = new MbrDeviceInfo();
                if (xmlReader.Name == "ActivityIndex")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.ActivityIndex = int.Parse(xmlReader.Value);
                }
                if (xmlReader.Name == "SourceType")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.SourceType = xmlReader.Value;
                }
                if (xmlReader.Name == "ID")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.Id = int.Parse(xmlReader.Value);
                }
                if (xmlReader.Name == "DeviceType")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.DeviceType = xmlReader.Value;
                }
                if (xmlReader.Name == "BrandName")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.BrandName = xmlReader.Value;
                }
                if (xmlReader.Name == "ModelNumber")
                {
                  xmlReader.Read();
                  mbrDeviceInfo.ModelNumber = xmlReader.Value;
                  mbrDeviceInfoList.Add(mbrDeviceInfo);
                }
              }
            }
          }
        }
      }
      mbrDeviceListInfo.MbrList = mbrDeviceInfoList.ToArray();
      return mbrDeviceListInfo;
    }
  }
}
