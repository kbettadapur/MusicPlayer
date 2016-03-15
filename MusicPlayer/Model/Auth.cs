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
            while (res.Count != 9)
            {
                try
                {
                    res = await client.PerformMasterLogin("5453EDB5AAC9");
                }
                catch (Exception e)
                {
                    loginSuccessful = false;
                    return loginSuccessful;
                }
            }
            //token oauth2rt_1/4y6xyPZ14pJmjUGGk2HrRecdM9Ing36lFxZQXN1Yrqk
            //auth DQAAABQBAAAtxzjhseQwMlLFVCrn7EEtSISt9C4UDzksepnCxmDrvUhm1QrU6_HLhFzixL_v2zw3fFiDA3sHzhWBhezbBOYYiKicSZecHXCI31CG12QMTSNQ10HTW9d6ghVjDAsxSVDuVgc7D5zh5FqZCDuLS-xBiin93WJ0Genq5EKnJIYYZaRL10yru4lJRik7JurPGBxnaLxsiWNnjnEJ75ikptA8thVxneGyGzkd4vh-V6YVlu3VFmJMrS2MhUaXEn8vG5oejfUpSvmYlQLI_ZqX02JmI5i353vwtABO_6AaqAUZ72I05oU5nzysE5xPFegPbfC8CF6EBzy5bkYQji5jZ-6qIbdkM1U8I1alUpeYIB1Uksl5L3psqFAOJupoXc86qiw
            //var token = "oauth2rt_1/4y6xyPZ14pJmjUGGk2HrRecdM9Ing36lFxZQXN1Yrqk";
            //var authToken1 = "DQAAABQBAAAtxzjhseQwMlLFVCrn7EEtSISt9C4UDzksepnCxmDrvUhm1QrU6_HLhFzixL_v2zw3fFiDA3sHzhWBhezbBOYYiKicSZecHXCI31CG12QMTSNQ10HTW9d6ghVjDAsxSVDuVgc7D5zh5FqZCDuLS-xBiin93WJ0Genq5EKnJIYYZaRL10yru4lJRik7JurPGBxnaLxsiWNnjnEJ75ikptA8thVxneGyGzkd4vh-V6YVlu3VFmJMrS2MhUaXEn8vG5oejfUpSvmYlQLI_ZqX02JmI5i353vwtABO_6AaqAUZ72I05oU5nzysE5xPFegPbfC8CF6EBzy5bkYQji5jZ-6qIbdkM1U8I1alUpeYIB1Uksl5L3psqFAOJupoXc86qiw";
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
