using MediaLibrary.DataModels;
using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace SmartTVRemoteControl.Native.MediaLibrary
{
	public class DataLibrary : IDataLibrary
	{
		private bool isDataLoaded;

		private CancellationTokenSource cancelToken;

		private IBaseDispatcher dispatcher;

		public Guid ID
		{
			get;
			set;
		}

        public bool IsDataLoaded
        {
            get
            {
                return true;
            }
            set
            {
                JustDecompileGenerated_set_IsDataLoaded(true);
            }
        }

        public bool JustDecompileGenerated_get_IsDataLoaded()
        {
            return this.isDataLoaded;
        }
        private void JustDecompileGenerated_set_IsDataLoaded(bool value)
        {
            this.isDataLoaded = value;
            this.OnDataLoaded();
        }

        public MultimediaFolder RootFolder => throw new NotImplementedException();

        public MultimediaFolder RootImageFolder => throw new NotImplementedException();

        public MultimediaFolder RootMusicFolder => throw new NotImplementedException();

        public MultimediaFolder RootVideoFolder => throw new NotImplementedException();

        public List<Track> TrackList => throw new NotImplementedException();

        static DataLibrary()
		{
			
		}
        
        public DataLibrary(IBaseDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        private void OnDataLoaded()
		{
			EventHandler eventHandler = this.DataLoaded;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		private void OnDataUpdated()
		{
			EventHandler eventHandler = this.DataUpdated;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
        
		public async Task SaveLibrary()
		{
            throw new NotImplementedException();
            //Settings.Default.ID = this.ID;
            //Settings.Default.Save();
        }

        public Task AddContentFromFiles(string[] files, ContentType ContentType)
        {
            throw new NotImplementedException();
        }

        public Task AddContentFromFolder(string folder, ContentType ContentType)
        {
            throw new NotImplementedException();
        }

        public void CancelAdding()
        {
            throw new NotImplementedException();
        }

        public void DeleteFileOrFolder(MultimediaFolder rootfolder, ItemBase itemtodelete)
        {
            throw new NotImplementedException();
        }

        public void DeleteItems(MultimediaFolder rootfolder, List<Content> itemstodelete)
        {
            throw new NotImplementedException();
        }

        public List<ItemBase> GetAlbums()
        {
            throw new NotImplementedException();
        }

        public List<Content> GetAlbumTracks(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<ItemBase> GetArtists()
        {
            throw new NotImplementedException();
        }

        public List<Content> GetArtistsTracks(Guid id)
        {
            throw new NotImplementedException();
        }

        public void GetFolderById(ref MultimediaFolder searchresult, Guid id, MultimediaFolder folderwheretosearch)
        {
            throw new NotImplementedException();
        }

        public List<ItemBase> GetGenres()
        {
            throw new NotImplementedException();
        }

        public List<Content> GetGenreTracks(string genre)
        {
            throw new NotImplementedException();
        }

        public ItemBase GetItemById(Guid id, MultimediaFolder root)
        {
            throw new NotImplementedException();
        }

        public async Task LoadLibrary()
        {
            this.IsDataLoaded = true;
        }

        public event EventHandler DataLoaded;

		public event EventHandler DataUpdated;
        public event EventHandler ContentAlreadyExist;
    }
}