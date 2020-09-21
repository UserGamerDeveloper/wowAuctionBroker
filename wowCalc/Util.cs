using NAudio.Wave;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using wowCalc;

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

        private const int AmountCopperInGold = 10000;
        private const int AmountCopperInSilver = 100;
        public const string URL_ITEM_PAGE_FORMAT = "https://eu.api.blizzard.com/data/wow/connected-realm/{0}/auctions?namespace=dynamic-eu&locale=en_US";
        public static readonly object exeptionLocker = new object();
        private static string accessToken;
        private static readonly Logger logger = 
            new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .WriteTo.File("Exception.txt"/*, fileSizeLimitBytes: 1, rollOnFileSizeLimit: true*/)
            /*(new JsonFormatter(renderMessage: true), @"logs\log-{Date}.txt")*/.CreateLogger();

        public static void SetAccessToken(string clientId, string clientSecret)
        {
            while (true)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(2d);
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://eu.battle.net/oauth/token"))
                        {
                            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Basic {base64authorization}");
                            request.Content = new StringContent("grant_type=client_credentials");
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                            Task<HttpResponseMessage> task = httpClient.SendAsync(request);
                            task.Wait();
                            var a = task.Result.Content.ReadAsStringAsync();
                            a.Wait();
                            string b = a.Result;
                            accessToken = JsonConvert.DeserializeObject<Token>(b).AccessToken;
                            break;
                        }
                    }
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        if (!(e is TaskCanceledException))
                        {
                            throw;
                        }
                    }
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
            Program.textBox.Invoke(new Action(() => {
                Program.textBox.Text += str;
            }));

            File.AppendAllText("log.txt", str);
        }

        public static void WriteLineAndLogWhithTime(string str)
        {
            Console.WriteLine(DateTime.Now + "\n" + str);
            File.AppendAllText("log.txt", DateTime.Now + "\n"+str + "\n");
        }

        public static void ExceptionLogAndAlert(Exception e)
        {
            logger.Error(e, "Exception");
            using (var audioFile = new AudioFileReader("music.aac"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                DialogResult result = MessageBox.Show(
                    "Exception",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
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
                        httpClient.Timeout = TimeSpan.FromSeconds(60d);
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format(URL_ITEM_PAGE_FORMAT, idRealm)))
                        {
                            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}");
                            Task<HttpResponseMessage> task = httpClient.SendAsync(request);
                            task.Wait();
                            var a = task.Result.Content.ReadAsStringAsync();
                            a.Wait();
                            return a.Result;
                        }
                    }
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        if (!(e is TaskCanceledException))
                        {
                            throw;
                        }
                    }
                }
            }
        }

        static public string GetTimeUpdateStr(int idRealm)
        {
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(URL_ITEM_PAGE_FORMAT, idRealm));
                    httpWebRequest.AllowAutoRedirect = false;
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = 2000;
                    httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {accessToken}");
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
