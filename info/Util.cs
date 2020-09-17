using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace info
{
    static class Util
    {
        private class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        private static TimeSpan TimeOut = TimeSpan.FromSeconds(20d);
        private const int AmountCopperInGold = 10000;
        private const int AmountCopperInSilver = 100;
        public const string URL_ITEM_PAGE_FORMAT = "https://eu.api.blizzard.com/data/wow/connected-realm/{0}/auctions?namespace=dynamic-eu&locale=en_US";
        public static readonly object exeptionLocker = new object();
        private static string accessToken;

        public static void SetAccessToken(string clientId, string clientSecret)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://eu.battle.net/oauth/token"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    request.Content = new StringContent("grant_type=client_credentials");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                    httpClient.Timeout = TimeOut;
                    Task<HttpResponseMessage> task = httpClient.SendAsync(request);
                    task.Wait();
                    var a = task.Result.Content.ReadAsStringAsync();
                    a.Wait();
                    string b = a.Result;
                    accessToken = JsonConvert.DeserializeObject<Token>(b).AccessToken;
                }
            }
        }

        public static double GetIncomeGoldInHour(double profit, TimeSpan timeSpan)
        {
            if (timeSpan.Ticks != 0)
            {
                return ConvertCopperToGold(profit / timeSpan.Ticks * TimeSpan.TicksPerHour);
            }
            else
            {
                return double.NaN;
            }
        }

        public static double ConvertCopperToGold(double copper)
        {
            return copper / AmountCopperInGold;
        }

        public static double ConvertCopperToSilver(double copper)
        {
            return copper / AmountCopperInSilver;
        }

        public static void WriteAndLog(string str)
        {
            Console.Write(str);
            File.AppendAllText("log.txt", str);
        }

        public static void WriteLineAndLog(string str)
        {
            Console.WriteLine(str);
            File.AppendAllText("log.txt", str + "\n");
        }
        public static void WriteLineAndLogWhithTime(string str)
        {
            Console.WriteLine(DateTime.Now + "\n" + str);
            File.AppendAllText("log.txt", DateTime.Now + "\n"+str + "\n");
        }

        public static void ExceptionLogAndAlert(Exception e)
        {
            lock (exeptionLocker)
            {
                Exception ex = e;
                string log = DateTime.Now.ToString() + "\n";
                while (ex != null)
                {
                    log += string.Format("{0} {1} \n", ex.Source, ex.Message);
                    ex = ex.InnerException;
                }
                File.AppendAllText("Exception.txt", log + e.StackTrace + "\n\n");
                Console.WriteLine("Надо перезагрузить\n");
                SoundPlayer alert = new SoundPlayer("music.wav");
                alert.PlayLooping();
            }
        }

        static public string GetAuctionDataStr(int idRealm)
        {
            while (true)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeOut;
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format(URL_ITEM_PAGE_FORMAT, idRealm)))
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
                            Task<HttpResponseMessage> task = httpClient.SendAsync(request);
                            task.Wait();
                            var a = task.Result.Content.ReadAsStringAsync();
                            a.Wait();
                            return a.Result;
                        }
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        static public string GetTimeUpdateStr(int idRealm)
        {
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(
                        string.Format(URL_ITEM_PAGE_FORMAT, idRealm) + $"&access_token={accessToken}");
                    httpWebRequest.AllowAutoRedirect = false;
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = 2000;
                    using (var webResponse = httpWebRequest.GetResponse())
                    {
                        return webResponse.Headers["Last-Modified"];
                    }
                }
                catch (WebException e)
                {
                    if (e.Response != null)
                    {
                        HttpWebResponse httpWebResponse = e.Response as HttpWebResponse;
                        if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw e;
                        }
                    }
                }
            }
        }
    }
}
