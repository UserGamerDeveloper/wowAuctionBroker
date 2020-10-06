using info;
using Microsoft.AspNetCore.SignalR;
using NAudio.Wave;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Xml.Serialization;

namespace wowCalc
{
    public class LogHub : Hub
    {

    }

    public class ParseService
    {
        private class Response
        {
            public Response(string message, object alertId)
            {
                Message = message;
                AlertId = alertId;
            }

            public string Message { get; set; }
            public object AlertId { get; set; }
        }

        internal Dictionary<string, Server> GetModel()
        {
            return serversByName;
        }

        static IHubContext<LogHub> hubContext;
        static Dictionary<string, Server> serversByName;
        public static readonly object consoleLocker = new object();
        public static Settings settings = Loader.DeserializeSettings();
        public const int AmountCopperInGold = 10000;
        private const int AmountCopperInSilver = 100;
        public const string URL_ITEM_PAGE_FORMAT = "https://eu.api.blizzard.com/data/wow/connected-realm/{0}/auctions?namespace=dynamic-eu&locale=en_US";
        public static readonly object exeptionLocker = new object();
        public static string accessToken;

        public ParseService(IHubContext<LogHub> hubContext)
        {
            ParseService.hubContext = hubContext;
        }

        public void Start(string accessToken)
        {
            ParseService.accessToken = accessToken;
            Saver.SerializeServers();
            serversByName = Loader.DeserializeServers();

            foreach (var server in serversByName.Values)
            {
                new Thread(new ThreadStart(server.Parse))
                {
                    IsBackground = true
                }
                .Start();
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

        public async static void SendAndLog(string message, object alertId = null)
        {
            await hubContext.Clients.All.SendAsync("Notify", new Response(message, alertId));

            File.AppendAllText("log.txt", message.Replace("<br>", "\n").Replace("&#9;", "\t"));
        }

        public static void WriteLineAndLogWhithTime(string str)
        {
            Console.WriteLine(DateTime.Now + "\n" + str);
            File.AppendAllText("log.txt", DateTime.Now + "\n" + str + "\n");
        }

        public static void ExceptionLogAndAlert(Exception e)
        {
            //throw e;
            Log.Logger.Error(e, "Exception");
            lock (exeptionLocker)
            {
                using (var audioFile = new AudioFileReader("wwwroot/media/music.aac"))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Console.Read();
                }
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

        static async public Task<string> GetResponseStringAsync(string uri)
        {
            while (true)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(10d);
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), uri))
                        {
                            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {accessToken}");
                            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request);
                            //task.Wait();
                            //var a = task.Result.Content.ReadAsStringAsync();
                            //a.Wait();
                            //return a.Result;
                            return await httpResponseMessage.Content.ReadAsStringAsync();
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