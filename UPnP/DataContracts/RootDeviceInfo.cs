// Decompiled with JetBrains decompiler
// Type: UPnP.DataContracts.RootDeviceInfo
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System.IO;
using System.Xml.Serialization;

namespace UPnP.DataContracts
{
  [XmlRoot("root", Namespace = "urn:schemas-upnp-org:device-1-0")]
  public class RootDeviceInfo
  {
    public static RootDeviceInfo Parse(string value) => (RootDeviceInfo) new XmlSerializer(typeof (RootDeviceInfo)).Deserialize((TextReader) new StringReader(value));

    [XmlElement("device")]
    public DeviceInfo Device { get; set; }
  }
}
