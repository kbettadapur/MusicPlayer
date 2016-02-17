using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModel
{
    public class AlbumViewModel
    {
        public Album CurrentAlbum { get; set; }
        public List<Song> SongList { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string AlbumArtRef { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand PlayOverrideQueueCommand { get; set; }
        public RelayCommand AddToQueueCommand { get; set; }

        public AlbumViewModel()
        {
            CurrentAlbum = (Album) NavigationService.GetInstance().Content;
            SongList = CurrentAlbum.AlbumTracks;
            AlbumTitle = CurrentAlbum.AlbumName;
            AlbumArtist = CurrentAlbum.AlbumArtist;
            AlbumArtRef = CurrentAlbum.AlbumArtRef;
            GoBackCommand = new RelayCommand(GoBack);
            PlayOverrideQueueCommand = new RelayCommand(PlayOverrideQueue);
            AddToQueueCommand = new RelayCommand(AddToQueue);
        }

        public void GoBack(object parameter)
        {
            NavigationService.GetInstance().GoBack();
        }

        public void PlayOverrideQueue(object parameter)
        {
            Player.GetInstance().PlayOverrideQueue(parameter);
        }

        public void AddToQueue(object parameter)
        {
            Player.GetInstance().AddToQueue((Song) parameter);
        }
    }
}
