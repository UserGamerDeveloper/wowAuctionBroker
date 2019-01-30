using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class Server : IComparable
    {
        const string uri_DB = "http://app-server{0}.tradeskillmaster.com/v2/auctiondb/realm/";
        
        int idTSMServer = 4;
        int timeOutDBPage = 2000;
        public int id;
        public string name;
        public string cookie;
        public long timeUpdate;
        [XmlIgnore]
        public long firstTimeUpdate;
        public List<int> activeItems;
        private int time;
        private int delaySecond;

        public Server() { }

        public Server(int id, string cookie, List<int> activeItems, string name, int time, int delaySecond)
        {
            this.id = id;
            this.cookie = cookie;
            this.activeItems = activeItems;
            this.name = name;
            this.time = time;
            this.delaySecond = delaySecond;
        }

        public void getFirstTimeUpdate()
        {
            string b;

            Uri DB = new Uri(String.Format(uri_DB, idTSMServer) + id);

            string DB_page = getDBPage(DB);

            b = DB_page.Remove(0, 38);
            firstTimeUpdate = Convert.ToInt64(b.Remove(10, b.Length - 10));
            Thread.Sleep(1000);
        }

        public void getTimeUpdate()
        {
            string b;

            Uri DB = new Uri(String.Format(uri_DB, idTSMServer) + id);

            string DB_page = getDBPage(DB);

            b = DB_page.Remove(0, 38);
            timeUpdate = Convert.ToInt64(b.Remove(10, b.Length - 10));
            Thread.Sleep(1000);
        }

        public bool isUpdate()
        {
            long oldTime = timeUpdate;
            getTimeUpdate();
            return timeUpdate > oldTime;
        }

        public void printAndLog()
        {
            string str = String.Format("{0} {1}", name, UnixTimeStampToDateTime(timeUpdate));
            Console.WriteLine(str);
            File.AppendAllText("log.txt", str + "\n");
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private string getDBPage(Uri uri)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                httpWebRequest.Timeout = timeOutDBPage;
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            timeOutDBPage = 2000;
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("Exception_time_page.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                timeOutDBPage += 1000;
                if (timeOutDBPage >= 10000)
                {
                    idTSMServer++;
                    if (idTSMServer == 8)
                    {
                        idTSMServer = 4;
                    }
                }
                return getDBPage(uri);
            }
        }

        public int CompareTo(object obj)
        {
            Server server = obj as Server;
            return firstTimeUpdate.CompareTo(server.firstTimeUpdate);
        }
    }
}
