using System.Collections.Generic;
using MusicPlayer.Model;

namespace MusicPlayer.ViewModel
{
    public class AllSongsViewModel
    {
        public RelayCommand GoBackCommand { get; set; }
        public List<Song> SongHits { get; set; }
        public RelayCommand AddToQueueCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }

        public AllSongsViewModel()
        {
            GoBackCommand = new RelayCommand(GoBack);
            AddToQueueCommand = new RelayCommand(AddToQueue);
            PlayCommand = new RelayCommand(Play);
            SongHits = (List<Song>)NavigationService.GetInstance().Content;   
        }

        public void GoBack(object parameter)
        {
            NavigationService.GetInstance().GoBack();
        }

        public void AddToQueue(object parameter)
        {
            Player.GetInstance().AddToQueue((Song)parameter);
        }

        public void Play(object parameter)
        {
            Player.GetInstance().Play(parameter);
        }
    }
}
