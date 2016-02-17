using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class Album
    {
        [JsonProperty("albumArtRef")]
        public string AlbumArtRef { get; set; }
        [JsonProperty("name")]
        public string AlbumName { get; set; }
        [JsonProperty("artistName")]
        public string ArtistName { get; set; }
        [JsonProperty("albumId")]
        public string Id { get; set; }
        [JsonProperty("tracks")]
        public List<Song> AlbumTracks { get; set; }
        [JsonProperty("albumArtist")]
        public string AlbumArtist { get; set; }
    }
}
