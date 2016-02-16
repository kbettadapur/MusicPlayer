using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class Artist
    {
        [JsonProperty("name")]
        public string ArtistName { get; set; }
        [JsonProperty("artistArtRef")]
        public string ArtistArtRef { get; set; }
    }
}
