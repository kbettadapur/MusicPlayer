using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModel
{
    public class ArtistViewModel
    {
        public Artist CurrentArtist { get; set; }
        public string ArtistName { get; set; }
        public string ArtistArtRef { get; set; }
        public List<Song> ArtistSongs { get; set; }
        public string AlbumArtRef { get; set; }
        public List<Song> ArtistTopFive { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand PlayOverrideQueueCommand { get; set; }
        public RelayCommand AddToQueueCommand { get; set; }

        public ArtistViewModel()
        {
            CurrentArtist = (Artist)NavigationService.GetInstance().Content;
            ArtistName = CurrentArtist.ArtistName;
            ArtistArtRef = CurrentArtist.ArtistArtRef;
            ArtistSongs = CurrentArtist.TopTracks;
            ArtistTopFive = new List<Song>();
            for (int i = 0; i < 5; i++) { ArtistTopFive.Add(ArtistSongs[i]); }
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
            Player.GetInstance().AddToQueue((Song)parameter);
        }
    }
}
