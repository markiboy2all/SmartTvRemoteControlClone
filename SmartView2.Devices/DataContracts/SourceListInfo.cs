// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.SourceListInfo
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SmartView2.Devices.DataContracts
{
  public class SourceListInfo
  {
    public SourceInfo[] SourceList { get; set; }

    public string CurrentSourceType { get; set; }

    public int Id { get; set; }

    public SourceListInfo() => this.SourceList = new SourceInfo[0];

    public static SourceListInfo Parse(string xml)
    {
      if (xml == null)
        return (SourceListInfo) null;
      SourceListInfo sourceListInfo = new SourceListInfo();
      IList<SourceInfo> source = (IList<SourceInfo>) new List<SourceInfo>();
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(xml)))
      {
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            if (xmlReader.Name == "CurrentSourceType")
            {
              xmlReader.Read();
              sourceListInfo.CurrentSourceType = xmlReader.Value;
            }
            if (xmlReader.Name == "ID")
            {
              xmlReader.Read();
              sourceListInfo.Id = int.Parse(xmlReader.Value);
            }
            if (xmlReader.Name == "Source")
            {
              SourceInfo sourceInfo = new SourceInfo();
              while (xmlReader.Read())
              {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                  if (xmlReader.Name == "Source")
                    sourceInfo = new SourceInfo();
                  if (xmlReader.Name == "SourceType")
                  {
                    xmlReader.Read();
                    sourceInfo.TypeXml = xmlReader.Value;
                  }
                  if (xmlReader.Name == "ID")
                  {
                    xmlReader.Read();
                    sourceInfo.Id = int.Parse(xmlReader.Value);
                  }
                  if (xmlReader.Name == "Editable")
                  {
                    xmlReader.Read();
                    sourceInfo.XmlEditable = xmlReader.Value;
                  }
                  if (xmlReader.Name == "EditNameType")
                  {
                    xmlReader.Read();
                    sourceInfo.EditNameType = xmlReader.Value;
                  }
                  if (xmlReader.Name == "DeviceName")
                  {
                    xmlReader.Read();
                    sourceInfo.DeviceName = xmlReader.Value;
                  }
                  if (xmlReader.Name == "Connected")
                  {
                    xmlReader.Read();
                    sourceInfo.XmlConnected = xmlReader.Value;
                  }
                  if (xmlReader.Name == "SupportView")
                  {
                    xmlReader.Read();
                    sourceInfo.XmlSupportView = xmlReader.Value;
                    source.Add(sourceInfo);
                  }
                }
              }
            }
          }
        }
      }
      sourceListInfo.SourceList = source.ToArray<SourceInfo>();
      return sourceListInfo;
    }
  }
}
