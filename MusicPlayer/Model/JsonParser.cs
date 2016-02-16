using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicPlayer.Model
{
    public class JsonParser
    {
        public static TotalJsonData Parse(string stringToParse)
        {
            return JsonConvert.DeserializeObject<TotalJsonData>(stringToParse);
        }

        
    }
}
