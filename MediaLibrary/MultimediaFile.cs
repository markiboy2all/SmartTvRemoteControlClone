// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.MultimediaFile
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  public class MultimediaFile : Content
  {
    [XmlElement("firstletter")]
    public string FirstLetter { get; set; }

    [XmlIgnore]
    public TimeSpan Duration { get; set; }

    [XmlElement("duration")]
    public int DurationSeconds
    {
      get => (int) this.Duration.TotalSeconds;
      set => this.Duration = TimeSpan.FromSeconds((double) value);
    }

    public MultimediaFile()
    {
    }

    public MultimediaFile(
      string name,
      string path,
      Guid guid,
      Guid parent,
      ContentType type,
      DateTime date,
      string queueId = null)
    {
      this.Name = name;
      this.Path = path;
      this.Parent = parent;
      this.ContentType = type;
      this.Type = ItemType.Content;
      this.Preview = (Uri) null;
      this.ID = guid;
      this.Date = date;
      this.QueueId = queueId;
      this.FirstLetter = name.Trim().Substring(0, 1).ToUpper();
    }

    public MultimediaFile(
      string name,
      string path,
      Guid parent,
      ContentType type,
      DateTime date,
      string queueId = null)
      : this(name, path, Guid.NewGuid(), parent, type, date, queueId)
    {
    }
  }
}
