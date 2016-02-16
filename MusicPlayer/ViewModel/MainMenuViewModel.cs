﻿using MusicPlayer.Model;
using MusicPlayer.Model.Net;
using MusicPlayer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System.Net;
using System.IO;
using NAudio.Wave;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer.ViewModel
{
    public class MainMenuViewModel : ViewModel
    {
        
        public RelayCommand SearchCommand { get; set; }
        public HttpClient client;
        public List<Song> SongHits { get; set; }
        public List<Artist> ArtistHits { get; set; }
        public string SearchQuery { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public Song SongSelected { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand AllSongsCommand { get; set; }
        public string SongLabelVisibility { get; set; }
        public string ArtistLabelVisibility { get; set; }
        public DispatcherTimer timer { get; set; }
        public string TimeElapsed { get; set; }
        TotalJsonData AllHits;
        

        public MainMenuViewModel()
        {
            SearchCommand = new RelayCommand(Search);
            client = new HttpClient();
            PlayCommand = new RelayCommand(Play);
            PauseCommand = new RelayCommand(Pause);
            AllSongsCommand = new RelayCommand(NavigateToAllSongs);
            SongLabelVisibility = "Collapsed";
            ArtistLabelVisibility = "Collapsed";
            BackgroundMediaPlayer.Current.MediaEnded += MediaEndedEventHandler;
        }

        public async void Search(object parameter)
        {
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
            SongHits = Hits.GetSongHits(AllHits.entries, 5);
            ArtistHits = Hits.GetArtistHits(AllHits.entries, 5);
            SongLabelVisibility = "Visible";
            ArtistLabelVisibility = "Visible";
            OnPropertyChanged("SongHits");
            OnPropertyChanged("ArtistHits");
            OnPropertyChanged("SongLabelVisibility");
            OnPropertyChanged("ArtistLabelVisibility");
        }

        private async Task<string> GetStreamUrl(object parameter)
        {
            string url = await HttpCall.GetStreamUrlAsync(parameter);
            return url;
        }

        public async void Play(object parameter)
        {
            Player.GetInstance().Play(parameter);
        }

        public void Pause(object parameter)
        {
            Player.GetInstance().Pause(parameter);
        }

        public void NavigateToAllSongs(object parameter)
        {
            NavigationService.GetInstance().Navigate(typeof(AllSongsPage), SongHits);
        }

        void MediaEndedEventHandler(MediaPlayer mp, object parameter)
        {
            int x = 0;
        }

        void TickEventHandler(object sender, object e)
        {
            TimeElapsed = BackgroundMediaPlayer.Current.Position.ToString();
            //string[] arr = TimeElapsed.Split(':','.');
            //TimeElapsed = "" + arr[1] + ":" + arr[2];
            OnPropertyChanged("TimeElapsed");
        }

        
    }
}