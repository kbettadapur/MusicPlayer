using MusicPlayer.Model.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace MusicPlayer.Model
{
    public class Player
    {
        public bool Playing { get; set; }
        private static Player Instance;
        public List<Song> SongQueue { get; set; }
        public int CurrentSong { get; set; }

        private Player()
        {
            Playing = false;
            SongQueue = new List<Song>();
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
                Playing = false;
                mp.Pause();
            }
            else
            {
                MediaPlayer mp = BackgroundMediaPlayer.Current;
                mp.Play();
            }
        }

        public void AddToQueue(Song parameter)
        {
            SongQueue.Add(parameter);
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
