using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicPlayer.Model
{
    public class Entry
    {
        [JsonProperty("track")]
        public Song song { get; set; }
        [JsonProperty("artist")]
        public Artist artist { get; set; }
    }
}
