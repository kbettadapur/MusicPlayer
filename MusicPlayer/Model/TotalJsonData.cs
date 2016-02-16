using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicPlayer.Model
{
    public class TotalJsonData
    {
        [JsonProperty("entries")]
        public List<Entry> entries;
    }
}
