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
using MusicPlayer.ViewModel;
using MusicPlayer.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllArtistsPage : Page
    {
        AllArtistsViewModel aavm;
        bool beenHere = false;

        public AllArtistsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                NavigationHelper.GetInstance().Add(aavm);
            }
                
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                aavm = (AllArtistsViewModel)NavigationHelper.GetInstance().Peek();
                aavm = (AllArtistsViewModel)NavigationHelper.GetInstance().Pop();
            } catch (Exception ex)
            {
                aavm = new AllArtistsViewModel();
            }
            DataContext = aavm;
        }
    }
}
