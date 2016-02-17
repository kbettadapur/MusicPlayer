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
        public ArtistViewModel()
        {
            CurrentArtist = (Artist)NavigationService.GetInstance().Content;
        }
    }
}
