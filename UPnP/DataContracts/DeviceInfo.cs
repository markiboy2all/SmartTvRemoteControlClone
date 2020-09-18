// Decompiled with JetBrains decompiler
// Type: UPnP.DataContracts.DeviceInfo
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace UPnP.DataContracts
{
  public class DeviceInfo : INotifyPropertyChanged
  {
    private string friendlyName = string.Empty;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(object sender, [CallerMemberName] string propertyName = "")
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(sender, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }

    [XmlElement("deviceType")]
    public string DeviceType { get; set; }

    [XmlElement("friendlyName")]
    public string FriendlyName
    {
      get => this.friendlyName;
      set
      {
        this.friendlyName = value;
        this.OnPropertyChanged((object) this, nameof (FriendlyName));
      }
    }

    [XmlElement("manufacturer")]
    public string Manufacturer { get; set; }

    [XmlElement("manufacturerURL")]
    public string ManufacturerUrl { get; set; }

    [XmlElement("modelDescription")]
    public string ModelDescription { get; set; }

    [XmlElement("modelName")]
    public string ModelName { get; set; }

    [XmlElement("modelNumber")]
    public string ModelNumber { get; set; }

    [XmlElement("serialNumber")]
    public string SerialNumber { get; set; }

    [XmlArray("serviceList")]
    [XmlArrayItem("service")]
    public ServiceInfo[] Services { get; set; }

    [XmlArrayItem("device")]
    [XmlArray("deviceList")]
    public DeviceInfo[] Devices { get; set; }

    [XmlElement("UDN")]
    public string UniqueDeviceName { get; set; }

    [XmlIgnore]
    public string UniqueServiceName { get; set; }

    [XmlIgnore]
    public Uri InfoAddress { get; set; }

    [XmlIgnore]
    public Uri DeviceAddress { get; set; }

    [XmlIgnore]
    public Uri LocalAddress { get; set; }

    [XmlIgnore]
    public DateTime LastActive { get; set; }

    [XmlIgnore]
    public string SourceXml { get; set; }

    [XmlIgnore]
    public bool IsValid { get; set; }
  }
}
