using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security;
using System.Collections.Specialized;
using System.IO;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Digests;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Net;

namespace MusicPlayer
{
    public class GPSOAuthClient
    {
        static string b64Key = "AAAAgMom/1a/v0lblO2Ubrt60J2gcuXSljGFQXgcyZWveWLEwo6prwgi3" +
            "iJIZdodyhKZQrNWp5nKJ3srRXcUW+F1BD3baEVGcmEgqaLZUNBjm057pK" +
            "RI16kB0YppeGx5qIQ5QjKzsR8ETQbKLNWgRY0QRNVz34kMJR3P/LgHax/" +
            "6rmf5AAAAAwEAAQ==";
        private RsaKeyParameters androidKey;

        static string version = "0.0.5";
        static string authUrl = "https://android.clients.google.com/auth";
        static string userAgent = "GPSOAuthSharp/" + version;

        private string email;
        private string password;
        private HttpClient client;

        public GPSOAuthClient(string email, string password)
        {
            this.email = email;
            this.password = password;
            client = new HttpClient();
            androidKey = GoogleKeyUtils.KeyFromB64(b64Key);
        }

        // _perform_auth_request
        private async Task<Dictionary<string, string>> PerformAuthRequest(Dictionary<string, string> data)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://android.clients.google.com/auth", UriKind.Absolute)
            };
            var keyValues = new List<KeyValuePair<string, string>>();
            foreach(var kvp in data)
            {
                keyValues.Add(new KeyValuePair<string, string>(kvp.Key, kvp.Value));
            }
            
            request.Headers.Add("User-Agent", userAgent);
            string result = "";
            try
            {
                request.Content = new HttpFormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await client.SendRequestAsync(request);
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception();
                }
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GoogleKeyUtils.ParseAuthResponse(result);
            
        }

        // perform_master_login
        public async Task<Dictionary<string, string>> PerformMasterLogin(string android_id, string service = "ac2dm",
            string deviceCountry = "us", string operatorCountry = "us", string lang = "en", int sdkVersion = 21)
        {
            string signature = GoogleKeyUtils.CreateSignature(email, password, androidKey);
            //string signature = @"AFcb4KS_5Yp0W66fYbt1IjFCaYVTNW6cRAKk8VzOh9RGFef8Jsv3aUvW7bNxap3aNVbVbCQ3p4k4Hp17bxxjzd3ekoiD34POjpJG0RGHx8a8aaRo5xYT2k3Aw5oReZvvLck_VnEjuDTPzSS4iolk3iASYedgc20uXB2dZTts_fM0hgWZHg==";
            var dict = new Dictionary<string, string> {
                { "accountType", "HOSTED_OR_GOOGLE" },
                { "Email", email },
                { "has_permission", 1.ToString() },
                { "add_account", 1.ToString() },
                { "EncryptedPasswd",  signature},
                { "service", service },
                { "source", "android" },
                { "device_country", deviceCountry },
                { "operatorCountry", operatorCountry },
                { "lang", lang },
               // { "androidId", android_id },
                { "sdk_version", sdkVersion.ToString() }
            };
            return await PerformAuthRequest(dict);
        }

        // perform_oauth
        public async Task<Dictionary<string, string>> PerformOAuth(string android_id, string masterToken, string service, string app, string clientSig,
            string deviceCountry = "us", string operatorCountry = "us", string lang = "en", int sdkVersion = 21)
        {
            var dict = new Dictionary<string, string> {
                { "accountType", "HOSTED_OR_GOOGLE" },
                { "Email", email },
                { "has_permission", 1.ToString() },
                { "EncryptedPasswd",  masterToken},
                { "service", service },
                { "source", "android" },
                { "app", app },
                { "client_sig", clientSig },
                { "device_country", deviceCountry },
                { "operatorCountry", operatorCountry },
                { "lang", lang },
                { "androidId", android_id },
                { "sdk_version", sdkVersion.ToString() }
            };
            return await PerformAuthRequest(dict);
        }

    }

    // gpsoauth:google.py
    // URL: https://github.com/simon-weber/gpsoauth/blob/master/gpsoauth/google.py
    class GoogleKeyUtils
    {
        // key_from_b64
        // BitConverter has different endianness, hence the Reverse()
        public static RsaKeyParameters KeyFromB64(string b64Key)
        {
            byte[] decoded = Convert.FromBase64String(b64Key);
            int modLength = BitConverter.ToInt32(decoded.Take(4).Reverse().ToArray(), 0);
            byte[] mod = decoded.Skip(4).Take(modLength).ToArray();
            int expLength = BitConverter.ToInt32(decoded.Skip(modLength + 4).Take(4).Reverse().ToArray(), 0);
            byte[] exponent = decoded.Skip(modLength + 8).Take(expLength).ToArray();
            BigInteger modulusBI = new BigInteger(1, mod);
            BigInteger exponentBI = new BigInteger(1, exponent);
            return new RsaKeyParameters(false, modulusBI, exponentBI);
        }

        // key_to_struct
        // Python version returns a string, but we use byte[] to get the same results
        public static byte[] KeyToStruct(RsaKeyParameters key)
        {
            byte[] modLength = { 0x00, 0x00, 0x00, 0x80 };
            byte[] mod = key.Modulus.ToByteArray().Skip(1).ToArray();
            byte[] expLength = { 0x00, 0x00, 0x00, 0x03 };
            byte[] exponent = key.Exponent.ToByteArray();
            return DataTypeUtils.CombineBytes(modLength, mod, expLength, exponent);
        }

        // parse_auth_response
        public static Dictionary<string, string> ParseAuthResponse(string text)
        {
            Dictionary<string, string> responseData = new Dictionary<string, string>();
            foreach (string line in text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split('=');
                responseData.Add(parts[0], parts[1]);
            }
            return responseData;
        }

        // signature
        public static string CreateSignature(string email, string password, RsaKeyParameters key)
        {
            
            byte[] prefix = { 0x00 };
            var keyStruct = KeyToStruct(key);
            var toEncrypt = Encoding.UTF8.GetBytes(email + "\x00" + password);
            var cipher = new OaepEncoding(new RsaEngine(), new Sha1Digest(), null);
            cipher.Init(true, key);
            var encrypted = cipher.ProcessBlock(toEncrypt, 0, toEncrypt.Length);

            var digest = new Sha1Digest();
            var hash = new byte[digest.GetByteLength()];
            digest.BlockUpdate(keyStruct, 0, keyStruct.Length);
            digest.DoFinal(hash, 0);
            var hashExcerpt = hash.Take(4).ToArray();

            return DataTypeUtils.UrlSafeBase64(DataTypeUtils.CombineBytes(prefix, hashExcerpt, encrypted));
        }

    }

    class DataTypeUtils
    {
        public static string UrlSafeBase64(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray).Replace('+', '-').Replace('/', '_');
        }

        public static byte[] CombineBytes(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
