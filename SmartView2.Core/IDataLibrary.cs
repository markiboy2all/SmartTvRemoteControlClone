// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.IDataLibrary
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using MediaLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartView2.Core
{
  public interface IDataLibrary
  {
    Guid ID { get; set; }

    bool IsDataLoaded { get; }

    MultimediaFolder RootFolder { get; }

    MultimediaFolder RootImageFolder { get; }

    MultimediaFolder RootMusicFolder { get; }

    MultimediaFolder RootVideoFolder { get; }

    List<Track> TrackList { get; }

    Task AddContentFromFiles(string[] files, ContentType ContentType);

    Task AddContentFromFolder(string folder, ContentType ContentType);

    void CancelAdding();

    void DeleteFileOrFolder(MultimediaFolder rootfolder, ItemBase itemtodelete);

    void DeleteItems(MultimediaFolder rootfolder, List<Content> itemstodelete);

    List<ItemBase> GetAlbums();

    List<Content> GetAlbumTracks(Guid id);

    List<ItemBase> GetArtists();

    List<Content> GetArtistsTracks(Guid id);

    void GetFolderById(
      ref MultimediaFolder searchresult,
      Guid id,
      MultimediaFolder folderwheretosearch);

    List<ItemBase> GetGenres();

    List<Content> GetGenreTracks(string genre);

    ItemBase GetItemById(Guid id, MultimediaFolder root);

    Task LoadLibrary();

    Task SaveLibrary();

    event EventHandler DataLoaded;

    event EventHandler ContentAlreadyExist;

    event EventHandler DataUpdated;
  }
}
