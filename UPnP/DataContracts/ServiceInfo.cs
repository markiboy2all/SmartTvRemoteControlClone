// Decompiled with JetBrains decompiler
// Type: UPnP.DataContracts.ServiceInfo
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System.Xml.Serialization;

namespace UPnP.DataContracts
{
  public class ServiceInfo
  {
    [XmlElement("serviceType")]
    public string ServiceType { get; set; }

    [XmlElement("serviceId")]
    public string ServiceId { get; set; }

    [XmlElement("SCPDURL")]
    public string DescriptionUrl { get; set; }

    [XmlElement("controlURL")]
    public string ControlUrl { get; set; }

    [XmlElement("eventSubURL")]
    public string EventUrl { get; set; }
  }
}
