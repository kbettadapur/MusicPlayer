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
        private static PlayerControl Instance { get; set; }
        public static Boolean Playing { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public string CurrentTime { get; set; }
        public DispatcherTimer timer { get; set; }
        public RelayCommand GoBackwardsCommand { get; set; }
        public RelayCommand GoForwardCommand { get; set; }
        public Song NowPlaying { get; set; }
        public int SongProgress { get; set; }

        public static PlayerControl GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PlayerControl();
            }
            return Instance;
        }
        
        private PlayerControl()
        {
            timer = new DispatcherTimer();
            timer.Tick += TickEventHandler;
            timer.Start();
            PauseCommand = new RelayCommand(Pause);
            GoForwardCommand = new RelayCommand(GoForward);
            GoBackwardsCommand = new RelayCommand(GoBackwards);
            Playing = false;
            NowPlaying = null;
            SongProgress = 0;
        }

        public void Pause(object parameter)
        {
            Player.GetInstance().Pause(parameter);
            UpdatePlaying();
        }

        public void TickEventHandler(object sender, object e)
        {
            MediaPlayer mp = BackgroundMediaPlayer.Current;
            CurrentTime = BackgroundMediaPlayer.Current.Position.ToString();
            NowPlaying = Player.GetInstance().NowPlaying;
            SongProgress = getSongProgress();
            OnPropertyChanged("NowPlaying");
            OnPropertyChanged("CurrentTime");
            OnPropertyChanged("SongProgress");
            int x = 0;
            UpdatePlaying();
        }

        public int getSongProgress()
        {
            double minutes = BackgroundMediaPlayer.Current.Position.Minutes;
            double seconds = BackgroundMediaPlayer.Current.Position.Seconds;
            if (Player.GetInstance().CurrentSong == -1) { return 0; }
            double totalSeconds = 0;
            try {
                totalSeconds = Int32.Parse((Player.GetInstance().SongQueue[Player.GetInstance().CurrentSong].durationMillis / 1000).ToString());
            } catch (Exception e) {
                return (int)totalSeconds;
            }
            if (totalSeconds == 0) { return 0; }
            double currentSeconds = (minutes * 60) + seconds;
            return (int)((currentSeconds / totalSeconds) * 100);
        }

        public void UpdatePlaying()
        {
            if (!BackgroundMediaPlayer.Current.CurrentState.Equals(MediaPlayerState.Playing)
                && !BackgroundMediaPlayer.Current.CurrentState.Equals(MediaPlayerState.Opening))
            {
                Playing = false;
                OnPropertyChanged("Playing");
            }
            else
            {
                Playing = true;
                OnPropertyChanged("Playing");
            }
        }

        public void GoForward(object parameter)
        {
            Player.GetInstance().GoForward();
        }

        public void GoBackwards(object parameter)
        {
            Player.GetInstance().GoBackwards();
        }
    }
}
