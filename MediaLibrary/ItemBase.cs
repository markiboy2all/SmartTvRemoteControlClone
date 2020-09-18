// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.ItemBase
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  [XmlInclude(typeof (MultimediaFile))]
  [XmlInclude(typeof (Content))]
  [XmlInclude(typeof (Track))]
  [XmlInclude(typeof (MultimediaFolder))]
  public abstract class ItemBase
  {
    [XmlElement("id")]
    public Guid ID { get; set; }

    [XmlElement("type")]
    public ItemType Type { get; set; }

    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("parent")]
    public Guid Parent { get; set; }

    public DateTime Date { get; set; }
  }
}
