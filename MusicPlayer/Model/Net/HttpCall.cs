using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace MusicPlayer.Model.Net
{
    public class HttpCall
    {
        public async static Task<HttpResponseMessage> MakeGetCallAsync(HttpRequestMessage message)
        {
            using (HttpClient client = new HttpClient(new HttpBaseProtocolFilter { AllowAutoRedirect = false }))
            {
                message.Headers.Add("Authorization", "GoogleLogin auth=" + Auth.getInstance().authToken);
                message.Headers.Add("X-Device-ID", "5453EDB5AAC9");
                HttpResponseMessage response = await client.SendRequestAsync(message);
                return response;
            }
        }
        public async static Task<string> GetStreamUrlAsync(object parameter)
        {
            var SongSelected = (Song)parameter;
            byte[] s1 = Convert.FromBase64String("VzeC4H4h+T2f0VI180nVX8x+Mb5HiTtGnKgH52Otj8ZCGDz9jRW" +
                                 "yHb6QXK0JskSiOgzQfwTY5xgLLSdUSreaLVMsVVWfxfa8Rw==");
            byte[] s2 = Convert.FromBase64String("ZAPnhUkYwQ6y5DdQxWThbvhJHN8msQ1rqJw0ggKdufQjelrKuiG" +
                                 "GJI30aswkgCWTDyHkTGK9ynlqTkJ5L4CiGGUabGeo8M6JTQ==");
            StringBuilder byteString = new StringBuilder();

            for (int i = 0; i < s1.Length; i++)
            {
                byteString.Append((char)(s1[i] ^ s2[i]));
            }
            byte[] key = Encoding.ASCII.GetBytes(byteString.ToString());

            HMac mac = new HMac(new Sha1Digest());

            KeyParameter param = new KeyParameter(key);
            byte[] output = new byte[mac.GetMacSize()];
            mac.Init(param);
            parameter = SongSelected.Id;
            string CurrentTimestamp = ((long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 1000)).ToString();
            Encoding enc = new UTF8Encoding();
            var salt = enc.GetBytes(CurrentTimestamp);
            var songId = enc.GetBytes((string)parameter);

            mac.BlockUpdate(songId, 0, songId.Length);
            mac.BlockUpdate(enc.GetBytes(CurrentTimestamp), 0, salt.Length);
            mac.DoFinal(output, 0);

            var sig = System.Convert.ToBase64String(output).TrimEnd(new char[] { '=' }).Replace('+', '-').Replace('/', '_');

            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://android.clients.google.com/music/mplay?opt=hi&net=mob&pt=e&slt=" + CurrentTimestamp + "&sig=" + sig + "&mjck=" + SongSelected.Id, UriKind.Absolute)
            };

            var songString = await HttpCall.MakeGetCallAsync(message);
            string url = songString.Headers.Location.ToString();
            return url;
        }
    }
}
