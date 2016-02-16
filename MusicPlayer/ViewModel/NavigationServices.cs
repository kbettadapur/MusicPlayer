using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayer.ViewModel
{
    public sealed class NavigationService : INavigationService
    {
        public object Content { get; set; }
        private static NavigationService Instance;

        private NavigationService()
        {

        }

        public static NavigationService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NavigationService();
            }
            return Instance;
        }

        public void Navigate(Type sourcePage)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage);
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            Content = parameter;
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage, parameter);
        }

        public void GoBack()
        {
            var frame = (Frame)Window.Current.Content;
            frame.GoBack();
        }
    }
}
