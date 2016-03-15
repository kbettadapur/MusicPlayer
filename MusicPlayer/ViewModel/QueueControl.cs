using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace MusicPlayer.ViewModel
{
    public class QueueControl : ViewModel
    {
        private static QueueControl Instance { get; set; }
        public ObservableCollection<Song> Queue { get; set; }
        public ScrollBarVisibility IsVerticalRailVisible { get; set; }

        private QueueControl()
        {
            IsVerticalRailVisible = ScrollBarVisibility.Hidden;
            Queue = Player.GetInstance().SongQueue;
        }

        public static QueueControl GetInstance()
        {
            if (Instance == null) { Instance = new QueueControl(); }
            return Instance;
        }

        public void UpdateQueue()
        {
            Queue = Player.GetInstance().SongQueue;
            OnPropertyChanged("Queue");
            if (Queue.Count > 6) { IsVerticalRailVisible = ScrollBarVisibility.Visible; }
            else { IsVerticalRailVisible = ScrollBarVisibility.Hidden; }
            OnPropertyChanged("IsVerticalRailVisible");
        }
    }
}
