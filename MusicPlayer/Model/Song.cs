using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicPlayer.Model
{
    public class Song
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("album")]
        public string Album { get; set; }
        [JsonProperty("albumArtRef")]
        public List<AlbumArt> AlbumArtList { get; set; }
        [JsonProperty("nid")]
        public string Id { get; set; }
        [JsonProperty("durationMillis")]
        public double durationMillis { get; set; }

        public string Duration
        {
            get
            {
                string minutes = ((int)(durationMillis / 1000)/60).ToString();
                string seconds = ((int)((durationMillis / 1000) % 60)).ToString();
                string duration = minutes + ":";
                if (seconds.Length == 1)
                {
                    seconds = "0" + seconds;
                }
                duration = duration + seconds;
                return duration;
            }
            set { }
        }

        public string SongInfo
        {
            get { return Title + "  -  " + Artist; }
        }
    }
}
