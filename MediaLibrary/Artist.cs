// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.Artist
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  public class Artist
  {
    public string Name { get; set; }

    public Guid ID { get; set; }

    [XmlElement("albumscount")]
    public int AlbumsCount { get; set; }

    [XmlElement("songscount")]
    public int SongsCount { get; set; }

    [XmlIgnore]
    public Uri Preview { get; set; }

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

    public Artist()
    {
    }

    public Artist(string name, Uri preview)
    {
      this.Name = name;
      this.Preview = preview;
      this.ID = Guid.NewGuid();
    }
  }
}
