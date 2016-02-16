using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class Hits
    {
        public static List<Song> GetSongHits(List<Entry> songList, int numResults)
        {
            int songCounter = 0;
            List<Song> SongHits = new List<Song>();
            foreach (Entry e in songList)
            {
                if (e.song != null)
                {
                    SongHits.Add(e.song);
                    songCounter++;
                }
                if (songCounter == numResults)
                {
                    break;
                }

            }
            return SongHits;
        }
        public static List<Artist> GetArtistHits(List<Entry> entries, int numResults)
        {
            int artistCounter = 0;
            List<Artist> ArtistHits = new List<Artist>();
            foreach (Entry e in entries)
            {
                if (e.artist != null)
                {
                    ArtistHits.Add(e.artist);
                    artistCounter++;
                }
                if (artistCounter == numResults)
                {
                    break;
                }
            }
            return ArtistHits;
        }
    }
}
