using MusicPlayer.Model;
using MusicPlayer.Model.Net;
using MusicPlayer.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayer.ViewModel
{
    public class MainMenuViewModel : ViewModel
    {
        
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand PlayOverrideQueueCommand { get; set; }
        public RelayCommand AllSongsCommand { get; set; }
        public RelayCommand NavigateToAlbumCommand { get; set; }
        public RelayCommand NavigateToArtistCommand { get; set; }
        public HttpClient client;
        public List<Song> SongHits { get; set; }
        public List<Artist> ArtistHits { get; set; }
        public List<Album> AlbumHits { get; set; }
        public string SearchQuery { get; set; }
        public string SongLabelVisibility { get; set; }
        public string ArtistLabelVisibility { get; set; }
        public string AlbumLabelVisibility { get; set; }
        TotalJsonData AllHits;
        public RelayCommand AddToQueueCommand { get; set; }
        
        public MainMenuViewModel()
        {
            SearchCommand = new RelayCommand(Search);
            client = new HttpClient();
            PlayOverrideQueueCommand = new RelayCommand(PlayOverrideQueue);
            AllSongsCommand = new RelayCommand(NavigateToAllSongs);
            SongLabelVisibility = "Collapsed";
            ArtistLabelVisibility = "Collapsed";
            AlbumLabelVisibility = "Collapsed";
            AddToQueueCommand = new RelayCommand(AddToQueue);
            NavigateToAlbumCommand = new RelayCommand(NavigateToAlbum);
            NavigateToArtistCommand = new RelayCommand(NavigateToArtist);
        }

        public async void Search(object parameter)
        {
            SongLabelVisibility = "Collapsed";
            ArtistLabelVisibility = "Collapsed";
            AlbumLabelVisibility = "Collapsed";
            if (SearchQuery == "" || SearchQuery == null)
            {
                return;
            }
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://mclients.googleapis.com/sj/v1.11/query?q=" + SearchQuery + "&max-results=50", UriKind.Absolute)
            };
            HttpResponseMessage returnString = await HttpCall.MakeGetCallAsync(message);
            var songString = await returnString.Content.ReadAsStringAsync();
            AllHits = JsonParser.Parse(songString);
            if (AllHits.entries != null)
            {
                SongHits = Hits.GetSongHits(AllHits.entries, 5);
                ArtistHits = Hits.GetArtistHits(AllHits.entries, 5);
                AlbumHits = Hits.GetAlbumHits(AllHits.entries, 5);
            }
            if (SongHits != null && SongHits.Count != 0) { SongLabelVisibility = "Visible"; }
            if (ArtistHits != null &&ArtistHits.Count != 0) { ArtistLabelVisibility = "Visible"; }
            if (AlbumHits != null &&AlbumHits.Count != 0) { AlbumLabelVisibility = "Visible"; }
            OnPropertyChanged("SongHits");
            OnPropertyChanged("ArtistHits");
            OnPropertyChanged("AlbumHits");
            OnPropertyChanged("SongLabelVisibility");
            OnPropertyChanged("ArtistLabelVisibility");
            OnPropertyChanged("AlbumLabelVisibility");
        }

        private async Task<string> GetStreamUrl(object parameter)
        {
            string url = await HttpCall.GetStreamUrlAsync(parameter);
            return url;
        }

        public void PlayOverrideQueue(object parameter)
        {
            Player.GetInstance().PlayOverrideQueue(parameter);
        }

        public void NavigateToAllSongs(object parameter)
        {
            NavigationService.GetInstance().Navigate(typeof(AllSongsPage), Hits.GetSongHits(AllHits.entries, 50));
        }

        public async void NavigateToArtist(object parameter)
        {
            Artist currentArtist = (Artist)parameter;
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://mclients.googleapis.com/sj/v1.11/fetchartist?alt=json&nid="
                + currentArtist.ArtistId + "&include-albums=true&num-top-tracks=50&num-related-artists=5", UriKind.Absolute )
            };
            HttpResponseMessage returnString = await HttpCall.MakeGetCallAsync(message);
            var str = await returnString.Content.ReadAsStringAsync();
            currentArtist = JsonParser.ParseArtist(str);
            NavigationService.GetInstance().Navigate(typeof(ArtistPage), currentArtist);   
        }

        public async void NavigateToAlbum(object parameter)
        {
            Album currentAlbum = (Album)parameter;
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://mclients.googleapis.com/sj/v1.11/fetchalbum?alt=json&nid=" + currentAlbum.Id + "&include-tracks=true", UriKind.Absolute)
            };
            HttpResponseMessage returnString = await HttpCall.MakeGetCallAsync(message);
            var str = await returnString.Content.ReadAsStringAsync();
            currentAlbum = JsonParser.ParseAlbum(str);
            NavigationService.GetInstance().Navigate(typeof(AlbumPage), currentAlbum);
        }

        public void AddToQueue(object parameter)
        {
            Player.GetInstance().AddToQueue((Song)parameter);
        }
        
    }
}
