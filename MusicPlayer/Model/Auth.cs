using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class Auth
    {
        private static Auth instance;
        public string authToken;
        public static Auth getInstance()
        {
            if (instance == null)
            {
                instance = new Auth();
            }
            return instance;
        }

        private Auth()
        {
            //private constructor
        }

        public async Task<bool> Login(string Username, string Password)
        {
            bool loginSuccessful = true;
            var client = new GPSOAuthClient(Username, Password);
            Dictionary<String, String> res = null;
            try
            {
                res = await client.PerformMasterLogin("5453EDB5AAC9");
            }
            catch (Exception e)
            {
                loginSuccessful = false;
                return loginSuccessful;
            }
            int x = 0;
            var token = res["Token"];
            res = await client.PerformOAuth("5453EDB5AAC9", token, "sj", "com.google.android.music", "38918a453d07199354f8b19af05ec6562ced5788");
            var authToken1 = res["Auth"];
            authToken = authToken1;
            loginSuccessful = true;
            return loginSuccessful;
            
        }
        public string Token { get; set; }
        public bool LoggedIn { get { return Token != "" && Token != null; } }
    }
}
