using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using MusicPlayer.View;
namespace MusicPlayer.ViewModel
{
    public class AllSongsViewModel
    {
        public RelayCommand GoBackCommand { get; set; }
        public List<Song> SongList { get; set; }

        public AllSongsViewModel()
        {
            GoBackCommand = new RelayCommand(GoBack);
            SongList = (List<Song>)NavigationService.GetInstance().Content;   
        }

        public void GoBack(object parameter)
        {
            
        }
    }
}
