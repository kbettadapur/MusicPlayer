using MusicPlayer.ViewModel;
using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using MusicPlayer.Model.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        MainMenuViewModel mmvm;
        bool _isNewPageInstance = false;

        public MainMenu()
        {
            this.InitializeComponent();
            _isNewPageInstance = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                NavigationHelper.GetInstance().Add(mmvm);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_isNewPageInstance)
            {
                if (mmvm == null)
                {
                    if (NavigationHelper.GetInstance().Count() > 0)
                    {
                        mmvm = (MainMenuViewModel)NavigationHelper.GetInstance().Pop();
                    }
                    else
                    {
                        mmvm = new MainMenuViewModel();
                    }
                }
                DataContext = mmvm;
            }
            _isNewPageInstance = false;
        }

        void NavigateToArtist(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            Artist lvi = (Artist)lv.SelectedItem;
            NavigationService.GetInstance().Navigate(typeof(ArtistPage), lvi);
        }

    }
}
