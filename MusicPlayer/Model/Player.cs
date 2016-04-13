using MusicPlayer.Model.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Playback;
using MusicPlayer.ViewModel;
using System.Collections.ObjectModel;

namespace MusicPlayer.Model
{
    public class Player
    {
        public bool Playing { get; set; }
        private static Player Instance;
        public ObservableCollection<Song> SongQueue { get; set; }
        public Song NowPlaying
        {
            get
            {
                if (CurrentSong < 0 || CurrentSong > SongQueue.Count - 1)
                    return null;
                return SongQueue[CurrentSong];
            }
        }
        public int CurrentSong { get; set; }

        private Player()
        {
            Playing = true;
            SongQueue = new ObservableCollection<Song>();
            CurrentSong = -1;
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

        public async void PlayOverrideQueue(object parameter)
        {
            SongQueue.Add((Song)parameter);
            CurrentSong = SongQueue.Count - 1;
            string streamUrl = await GetStreamUrl(SongQueue[CurrentSong]);
            BackgroundMediaPlayer.Current.SetUriSource(new Uri(streamUrl));
            BackgroundMediaPlayer.Current.Volume = BackgroundMediaPlayer.Current.Volume;
            BackgroundMediaPlayer.Current.Play();
            QueueControl.GetInstance().UpdateQueue();
        }

        private async Task<string> GetStreamUrl(object parameter)
        {
            string url = await HttpCall.GetStreamUrlAsync(parameter);
            return url;
        }

        public void Pause(object parameter)
        {
            if (CurrentSong == -1)
            {
                if (SongQueue.Count == 0)
                {
                    return;
                }
                else
                {
                    CurrentSong++;
                    Play(SongQueue[CurrentSong]);
                }
            } else if (CurrentSong == SongQueue.Count)
            {
                CurrentSong = 0;
                Play(SongQueue[CurrentSong]);
            }
            else if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
            {
                MediaPlayer mp = BackgroundMediaPlayer.Current;
                mp.Pause();
            }
            else
            {
                MediaPlayer mp = BackgroundMediaPlayer.Current;
                mp.Play();
            }
            int x = 0;
        }

        public void AddToQueue(Song parameter)
        {
            SongQueue.Add(parameter);
            QueueControl.GetInstance().UpdateQueue();
        }

        public void GoForward()
        {
            if (CurrentSong + 1 < SongQueue.Count)
            {
                Play(SongQueue[++CurrentSong]);
            }
        }

        public void GoBackwards()
        {
            if (CurrentSong > 0)
            {
                Play(SongQueue[--CurrentSong]);
            }
        }

        void MediaEndedEventHandler(MediaPlayer mp, object parameter)
        {
            CurrentSong++;
            if (CurrentSong == SongQueue.Count)
            {
                BackgroundMediaPlayer.Current.Pause();
            }
            else
            {
                Play(SongQueue[CurrentSong]);
            }
        }
    }
}
