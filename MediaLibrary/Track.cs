// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.Track
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  public class Track : Content
  {
    [XmlElement("album")]
    public Album Album { get; set; }

    [XmlElement("artist")]
    public Artist Artist { get; set; }

    [XmlElement("genre")]
    public Genre Genre { get; set; }

    [XmlElement("artistfirstletter")]
    public string ArtistFirstLetter { get; set; }

    [XmlElement("genrefirstletter")]
    public string GenreFirstLetter { get; set; }

    [XmlElement("trackfirstletter")]
    public string TrackFirstLetter { get; set; }

    public int Priority { get; set; }

    [XmlElement("trackNumber")]
    public int TrackNumber { get; set; }

    [XmlIgnore]
    public TimeSpan Duration { get; set; }

    [XmlElement("duration")]
    public int DurationSeconds
    {
      get => (int) this.Duration.TotalSeconds;
      set => this.Duration = TimeSpan.FromSeconds((double) value);
    }

    public Track()
    {
    }

    public Track(
      string name,
      string filepath,
      Guid guid,
      Guid parent,
      Artist artist,
      Album album,
      Genre genre,
      Uri preview,
      string queueId = null)
    {
      this.Name = StringPreprocessor.RemoveInvisibleChars(name);
      this.Path = filepath;
      this.Album = album;
      this.Artist = artist;
      this.Genre = genre;
      this.Parent = parent;
      this.Preview = preview;
      this.ID = guid;
      this.ContentType = ContentType.Track;
      this.Type = ItemType.Content;
      this.QueueId = queueId;
      this.ArtistFirstLetter = StringPreprocessor.GetFirstLetter(this.Artist.Name);
      this.GenreFirstLetter = StringPreprocessor.GetFirstLetter(this.Genre.Name);
      this.TrackFirstLetter = StringPreprocessor.GetFirstLetter(this.Name);
    }

    public Track(
      string name,
      string filepath,
      Guid parent,
      Artist artist,
      Album album,
      Genre genre,
      Uri preview,
      string queueId = null)
      : this(name, filepath, Guid.NewGuid(), parent, artist, album, genre, preview, queueId)
    {
    }
  }
}
