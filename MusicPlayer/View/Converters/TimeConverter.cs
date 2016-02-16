using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MusicPlayer.View.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            String modTime = "";
            if (value != null)
            {
                String val = value.ToString();
                string[] arr = val.Split(':', '.');
                modTime = arr[1] + ":" +  arr[2];
            }
            
            return modTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
