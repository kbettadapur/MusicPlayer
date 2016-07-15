using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using Windows.Web.Http;
using MusicPlayer.Model.Net;
using MusicPlayer.View;

namespace MusicPlayer.ViewModel
{
    public class AllArtistsViewModel : ViewModel
    {
        public List<Artist> ArtistHits { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand NavigateToArtistCommand { get; set; }

        public AllArtistsViewModel()
        {
            ArtistHits = (List<Artist>) NavigationService.GetInstance().Content;
            GoBackCommand = new RelayCommand(GoBack);
            NavigateToArtistCommand = new RelayCommand(NavigateToArtist);
        }

        public void GoBack(object parameter)
        {
            NavigationService.GetInstance().GoBack();
        }

        public async void NavigateToArtist(object parameter)
        {
            Artist currentArtist = (Artist)parameter;
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://mclients.googleapis.com/sj/v1.11/fetchartist?alt=json&nid="
                + currentArtist.ArtistId + "&include-albums=true&num-top-tracks=50&num-related-artists=5", UriKind.Absolute)
            };
            HttpResponseMessage returnString = await HttpCall.MakeGetCallAsync(message);
            var str = await returnString.Content.ReadAsStringAsync();
            currentArtist = JsonParser.ParseArtist(str);
            NavigationService.GetInstance().Navigate(typeof(ArtistPage), currentArtist);
        }
    }
}
