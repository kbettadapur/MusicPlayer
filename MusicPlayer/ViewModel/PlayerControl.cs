using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using Windows.Media.Playback;

namespace MusicPlayer.ViewModel
{
    public class PlayerControl : ViewModel
    {
        public static Boolean Playing { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public PlayerControl()
        {
            //Player.GetInstance().timer.Tick += TickEventHandler;
            PauseCommand = new RelayCommand(Pause);
            Playing = true;
        }

        void TickEventHandler(object sender, object e)
        {

        }

        public void Pause(object parameter)
        {
            if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Paused)
            {
                Playing = true;
                OnPropertyChanged("Playing");
            }
            else
            {
                Playing = false;
                OnPropertyChanged("Playing");
            }
            Player.GetInstance().Pause(parameter);
        }

    }
}
