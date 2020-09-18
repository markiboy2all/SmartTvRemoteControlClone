// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.Content
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  [XmlInclude(typeof (MultimediaFile))]
  [XmlInclude(typeof (Track))]
  public abstract class Content : ItemBase
  {
    public ContentType ContentType { get; set; }

    public string Path { get; set; }

    [XmlIgnore]
    public Uri Preview { get; set; }

    public DateTime LastUpdate { get; set; }

    public string PreviewString
    {
      get => this.Preview != (Uri) null ? this.Preview.OriginalString : (string) null;
      set
      {
        Uri result;
        if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out result))
          this.Preview = result;
        else
          this.Preview = (Uri) null;
      }
    }

    public string QueueId { get; set; }

    [XmlIgnore]
    public Uri Thumbnail { get; set; }

    public bool IsPreviewLoaded { get; set; }
  }
}
