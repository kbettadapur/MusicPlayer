using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Model;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer.ViewModel
{
    public class PlayerControl : ViewModel
    {
        public static Boolean Playing { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public string CurrentTime { get; set; }
        public DispatcherTimer timer { get; set; }
        
        public PlayerControl()
        {
            timer = new DispatcherTimer();
            timer.Tick += TickEventHandler;
            timer.Start();
            PauseCommand = new RelayCommand(Pause);
            Playing = false;
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

        public void TickEventHandler(object sender, object e)
        {
            CurrentTime = BackgroundMediaPlayer.Current.Position.ToString();
            OnPropertyChanged("CurrentTime");
        }
    }
}
