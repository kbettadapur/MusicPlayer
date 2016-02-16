using MusicPlayer.Model.Net;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer.Model
{
    public class Player
    {
        public bool Playing { get; set; }
        public DispatcherTimer timer { get; set; }
        private static Player Instance;
        public List<Song> SongQueue { get; set; }
        int currentSong;

        private Player()
        {
            Playing = false;
            SongQueue = new List<Song>();
           
            currentSong = -1;
            BackgroundMediaPlayer.Current.MediaEnded += MediaEndedEventHandler;
        }

        public static Player GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Player();
            }
            return Instance;
        }

        public async void Play(object parameter)
        {
            string streamUrl = await GetStreamUrl(parameter);
            BackgroundMediaPlayer.Current.SetUriSource(new Uri(streamUrl));
            BackgroundMediaPlayer.Current.Play();
        }

        public void Resume()
        {
            BackgroundMediaPlayer.Current.Play();
        }

        private async Task<string> GetStreamUrl(object parameter)
        {
            string url = await HttpCall.GetStreamUrlAsync(parameter);
            return url;
        }

        public void Pause(object parameter)
        {
            if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
            {
                MediaPlayer mp = BackgroundMediaPlayer.Current;
                Playing = false;
                mp.Pause();
            }
            else
            {
                MediaPlayer mp = BackgroundMediaPlayer.Current;
                Resume();
                Playing = true;
            }
        }

        public void AddToQueue(Song parameter)
        {
            SongQueue.Add(parameter);
            int x = 0;
        }

        void MediaEndedEventHandler(MediaPlayer mp, object parameter)
        {
            currentSong++;
            if (currentSong == SongQueue.Count)
            {
                BackgroundMediaPlayer.Current.Pause();
            }
            else
            {
                Play(SongQueue[currentSong]);
            }
        }
    }
}
