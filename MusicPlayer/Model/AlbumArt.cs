using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class AlbumArt
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
