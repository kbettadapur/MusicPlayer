using MusicPlayer.Model;
using MusicPlayer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModel
{
    public class LoginViewModel : ViewModel
    {
        public RelayCommand LoginCommand { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ShowErrorMessage { get; set; }
        string authToken;
        private INavigationService navigationService;

        public LoginViewModel() 
        {
            LoginCommand = new RelayCommand(PerformLogin);
            ShowErrorMessage = "Collapsed";
            
        }

        public async void PerformLogin(object parameter)
        {
            Auth auth = Auth.getInstance();
            bool loginSuccessful = await auth.Login(Username, Password);
            if (loginSuccessful)
            {
                NavigationService.GetInstance().Navigate(typeof(MainMenu), auth.authToken);
            }
            else
            {
                ShowErrorMessage = "Visible";
                OnPropertyChanged("ShowErrorMessage");
            }
        }
    }
}
