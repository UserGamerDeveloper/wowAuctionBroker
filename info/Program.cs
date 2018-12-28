using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Threading;
using System.Runtime.InteropServices;
using System.Media;
using System.Xml.Serialization;

namespace habrJob
{
    public class Program
    {
        public enum ServerId
        {
            Twisting_Nether = 511,
            Ravencrest = 496,
            Blackmoore = 333,
            Antonidas = 289,
            malganis = 436,
            svezewatel = 502,
            gorduni = 463
        }

        public enum ItemType
        {
            SILK = 0,
            LEATHER = 1,
            SCALE = 2
        }

        //[JsonObject(MemberSerialization.OptIn)]
        [Serializable]
        public class Server : IComparable
        {
            public int id;
            public string name;
            public string cookie;
            public long timeUpdate;
            [XmlIgnore]
            public long firstTimeUpdate;
            public List<ItemType> activeItems;
            public int time;
            public int delaySecond;

            public Server(){}

            public Server(int id, string cookie, List<ItemType> activeItems, string name, int time, int delaySecond)
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

            public int CompareTo(object obj)
            {
                Server server = obj as Server;
                return firstTimeUpdate.CompareTo(server.firstTimeUpdate);
            }
        }

        class Bid : IComparable
        {
            public int idItem;
            public int count;
            public long cost;
            public int profit;
            public int profitPerItem;
            public string autor;

            public Bid(int idItem, int count, long cost, string autor)
            {
                this.idItem = idItem;
                this.count = count;
                this.cost = cost;
                profit = (int)(costsBonus[idItem] - cost) * count;
                profitPerItem = profit / count;
                this.autor = autor;
            }

            public int CompareTo(object obj)
            {
                Bid bid = obj as Bid;
                return profitPerItem.CompareTo(bid.profitPerItem);
            }
        }

        class Item : IComparable
        {
            public int id;
            public string itemName;
            public int profit;
            public List<Bid> bids = new List<Bid>();
            public int count;
            public long costSell;
            public Uri uri;

            public Item(int id, string itemName, long costSell, string uri)
            {
                this.id = id;
                this.itemName = itemName;
                this.costSell = costSell;
                this.uri = new Uri(uri);
            }

            public int CompareTo(object obj)
            {
                Item item = obj as Item;
                return getAverageProfit().CompareTo(item.getAverageProfit());
            }

            public double getAverageProfit()
            {
                return profit / 10000f / count;
            }

            public void print()
            {
                if (bids.Count > 0)
                {
                    int minCount = 0;
                    int maxCount = 0;
                    foreach (var bid in bids)
                    {
                        if ((bid.cost == bids[0].cost))
                        {
                            minCount++;
                        }
                        if ((bid.cost == bids[bids.Count - 1].cost) && bids[bids.Count - 1].autor.Equals(bid.autor))
                        {
                            maxCount++;
                        }
                    }
                    string printStr = itemName+"               " + String.Format("{0:# ## ##} x{1}", bids[0].cost, minCount) + "             " + String.Format("{0:# ## ##} x{1}    {2}", bids[bids.Count - 1].cost, maxCount, bids[bids.Count - 1].autor);
                    Console.WriteLine(printStr);
                    File.AppendAllText("log.txt", printStr + "\n");
                }
            }
        }

        static string row = "//tr[@data-key='{0}']"; 
        const int COST_Shaldorei_Silk = 23936;
        const int COST_LEATHER = 23648;
        const int COST_SCALE = 23292;
        static int COST_BONUS_Shaldorei_Silk;
        static int COST_BONUS_LEATHER;
        static int COST_BONUS_SCALE;
        static int requestMoney;
        static SoundPlayer alert = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\alert.wav");
        static SoundPlayer simpleSound = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\music.wav");
        const string SILK = "https://www.tradeskillmaster.com/items/shal-dorei-silk-124437?sort=buyout";
        const string LEATHER = "https://www.tradeskillmaster.com/items/stonehide-leather-124113?sort=buyout";
        const string SCALE = "https://www.tradeskillmaster.com/items/stormscale-124115?sort=buyout";
        const string uri_DB = "http://app-server{0}.tradeskillmaster.com/v2/auctiondb/realm/";
        static int idTSMServer = 4;
        static int timeOutDBPage = 2000;
        static int timeOutAHPage = 5000;
        const string regionidValue = "1de8a1f36d12d6d93b180176e811127d35fb174f75e311c5025f1fb101c6194aa%3A2%3A%7Bi%3A0%3Bs%3A8%3A%22regionId%22%3Bi%3A1%3Bs%3A2%3A%22EU%22%3B%7D";
        const string antonidas = "4189f471c98b39465bf62b9def9057c8aa4e28cce9ddc285580e5d5448caa3f7a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A289%3B%7D";
        const string blackmoore = "833adf3275b85e5ce95b750245f3013212fe3bff64cc4bd93df24598a982a7d5a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A333%3B%7D";
        const string malganis = "8108e41136d7d492af435be3e2ba019ca5cd4fe25b17955be6d128f1d4d7f14da%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A437%3B%7D";
        const string Ravencrest = "69d53acf7ee790d7dbe3637413ab9b83a1ec12eef78e5e62257afb8d11c688eaa%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A496%3B%7D";
        const string Twisting_Nether  = "90fa4178b714cf2b6db247b90969bd497c299c5bd1406170107c262a01cc8657a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A511%3B%7D";
        const string svezewatel = "47f9045c0840cbff05b27392572249648406aede97b1cf58a2f262dc08cd31d8a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A502%3B%7D";
        const string gorduni = "1f3552dbc09c9097de622c7ffb69fda01b8f41789c712aee433334a85dcd5470a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A463%3B%7D";
        static HtmlDocument htmlDocument = new HtmlDocument();
        static int count_IN_STACK;
        static int N_INCOME;
        const double TIME_NEED = 166.66666666666666666666666666666666;
        static Server[] servers;
        //static Server[] servers = new Server[] {
        //    new Server((int)ServerId.Twisting_Nether, Twisting_Nether, new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Twisting Nether",18,0),
        //    new Server((int)ServerId.Blackmoore, blackmoore, new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Blackmoore",18,0),
        //    new Server((int)ServerId.Antonidas, antonidas,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Antonidas",20,0),
        //    new Server((int)ServerId.Ravencrest, Ravencrest,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Ravencrest",18,0),
        //    new Server((int)ServerId.malganis, malganis,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"malganis",18,0),
        //    new Server((int)ServerId.svezewatel, svezewatel,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"свежеватель",18,0),
        //    new Server((int)ServerId.gorduni, gorduni,new List<ItemType>{ ItemType.LEATHER, ItemType.SCALE},"гордунни",18,0)
        //};
        static long[] costsBonus;
        static XmlSerializer formatter = new XmlSerializer(typeof(Server[]));

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        static void Main(string[] args)
        {
            if (args.Length==6)
            {
                requestMoney = Convert.ToInt32(args[0]);
                COST_BONUS_Shaldorei_Silk = Convert.ToInt32(args[1]);
                COST_BONUS_LEATHER = Convert.ToInt32(args[2]);
                COST_BONUS_SCALE = Convert.ToInt32(args[3]);
                count_IN_STACK = Convert.ToInt32(args[4]);
                N_INCOME = Convert.ToInt32(args[5]);
                costsBonus = new long[] { COST_Shaldorei_Silk, COST_LEATHER, COST_SCALE };

                start();
            }
            else
            {
                Console.WriteLine("нет аргумента");
                Console.ReadLine();
            }
        }

        private static void start()
        {
            try
            {
                using (FileStream fs = new FileStream("data.xml", FileMode.Open))
                {
                    servers = (Server[])formatter.Deserialize(fs);
                }
                //using (FileStream fs = new FileStream("data.xml", FileMode.Create))
                //{
                //    formatter.Serialize(fs, servers);
                //}
                foreach (var server in servers)
                {
                    server.getFirstTimeUpdate();
                }
                Array.Sort(servers);
                while (true)
                {
                    for (int idServer = servers.Length - 1; idServer >= 0; idServer--)
                    {
                        if (servers[idServer].isUpdate())
                        {
                            servers[idServer].printAndLog();

                            List<Bid> bids = new List<Bid>();

                            Item silk = new Item((int)ItemType.SILK, "   ткань", costsBonus[(int)ItemType.SILK], SILK);
                            Item leather = new Item((int)ItemType.LEATHER, "   кожа", costsBonus[(int)ItemType.LEATHER], LEATHER);
                            Item scale = new Item((int)ItemType.SCALE, "   чещуя", costsBonus[(int)ItemType.SCALE], SCALE);

                            Dictionary<int, Item> items = new Dictionary<int, Item>
                            {
                                { silk.id,silk},
                                { leather.id,leather},
                                { scale.id,scale}
                            };

                            foreach (ItemType item in servers[idServer].activeItems)
                            {
                                string HTML = getAHPage(items[(int)item].uri, servers, idServer);
                                htmlDocument.LoadHtml(HTML);
                                HtmlNodeCollection pads = htmlDocument.DocumentNode.SelectNodes("//div[@class='summary']");
                                if (pads!=null)
                                {
                                    string[] countTemp = pads[0].InnerText.Split(' ')[1].Split(',');
                                    string countTempa = "";
                                    foreach (var a in countTemp)
                                    {
                                        countTempa += a;
                                    }
                                    int count = Convert.ToInt32(countTempa);
                                    for (int i = 0; i < count; i++)
                                    {
                                        pads = htmlDocument.DocumentNode.SelectNodes(String.Format(row, i));
                                        HtmlDocument rowHTML = new HtmlDocument();
                                        rowHTML.LoadHtml(pads[0].InnerHtml);
                                        pads = rowHTML.DocumentNode.SelectNodes("//td[@data-col-seq='3']");
                                        string[] a = pads[0].InnerText.Split(new char[] { 'g', 's', 'c', ' ', ',' });
                                        string costBidStr = "";
                                        foreach (var c in a)
                                        {
                                            costBidStr += c;
                                        }
                                        long costBid = Convert.ToInt64(costBidStr);
                                        pads = rowHTML.DocumentNode.SelectNodes("//td[@data-col-seq='0']");
                                        int countInBid = Convert.ToInt32(pads[0].InnerText);
                                        long cost = (long)Math.Floor(costBid / (double)countInBid);
                                        if (cost > 0)
                                        {
                                            if (cost <= costsBonus[(int)item])
                                            {
                                                if (countInBid > count_IN_STACK)
                                                {
                                                    pads = rowHTML.DocumentNode.SelectNodes("//td[@data-col-seq='4']");
                                                    string autor = pads[0].InnerText;
                                                    bids.Add(new Bid((int)item, countInBid, cost, autor));
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            bids.Sort();

                            long profit_all = 0;
                            double income = 0;
                            DateTime timeNeed = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            bool isCollectMoney = false;
                            for (int i = bids.Count - 1; i >= 0; i--)
                            {
                                if (!isCollectMoney)
                                {
                                    items[bids[i].idItem].profit += bids[i].profit;
                                    items[bids[i].idItem].count += bids[i].count;
                                    items[bids[i].idItem].bids.Add(bids[i]);

                                    profit_all += bids[i].profit;
                                    timeNeed = timeNeed.AddMilliseconds(TIME_NEED * bids[i].count);
                                    if (DateTime.Now.Hour >= servers[idServer].time)
                                    {
                                        timeNeed = timeNeed.AddSeconds(servers[idServer].delaySecond);
                                    }
                                    income = profit_all / 10000f / (timeNeed.Millisecond / 1000f + timeNeed.Second + timeNeed.Minute * 60 + timeNeed.Hour * 3600) * 3600;
                                    if ((profit_all / 10000f) >= requestMoney)
                                    {
                                        isCollectMoney = true;
                                    }
                                }
                                else
                                {
                                    DateTime tempTimeNeed = timeNeed.AddMilliseconds(TIME_NEED * bids[i].count);
                                    if (DateTime.Now.Hour >= servers[idServer].time)
                                    {
                                        tempTimeNeed = tempTimeNeed.AddSeconds(servers[idServer].delaySecond);
                                    }
                                    double tempIncome = (profit_all + bids[i].profit) / 10000f / (tempTimeNeed.Millisecond / 1000f + tempTimeNeed.Second + tempTimeNeed.Minute * 60 + tempTimeNeed.Hour * 3600) * 3600;
                                    if (tempIncome >= N_INCOME)
                                    {
                                        income = tempIncome;
                                        profit_all += bids[i].profit;
                                        timeNeed = tempTimeNeed;

                                        items[bids[i].idItem].profit += bids[i].profit;
                                        items[bids[i].idItem].count += bids[i].count;
                                        items[bids[i].idItem].bids.Add(bids[i]);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            string str = "Профит ";
                            string profit_allStr = Math.Floor(profit_all / 10000f) + " " + Math.Floor(income);
                            File.AppendAllText("log.txt", str + profit_allStr + "\n\n");

                            if ((profit_all / 10000f) >= requestMoney / 2)
                            {
                                Item[] itemsArray = (new List<Item>(items.Values)).ToArray();
                                Array.Sort(itemsArray);
                                for (int i = itemsArray.Length - 1; i >= 0; i--)
                                {
                                    itemsArray[i].print();
                                }

                                if ((profit_all / 10000f) >= requestMoney && income >= N_INCOME)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(str);
                                    Console.ResetColor();
                                    Console.WriteLine(profit_allStr);

                                    simpleSound.PlayLooping();
                                    Console.ReadLine();
                                    simpleSound.Stop();
                                }
                                else
                                {
                                    Console.Write(str);
                                    Console.WriteLine(profit_allStr);
                                }
                            }

                            using (FileStream fs = new FileStream("data.xml", FileMode.Create))
                            {
                                formatter.Serialize(fs, servers);
                            }
                            Console.WriteLine();
                        }
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("Exception.txt", DateTime.Now.ToString() + "\n"+e.ToString() + "\n");
                Console.WriteLine("Перезагрузка \n");
                alert.Play();
                start();
            }
        }

        private static string getDBPage(Uri DB)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(DB);
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
                if (timeOutDBPage == 10000)
                {
                    idTSMServer++;
                    if (idTSMServer == 8)
                    {
                        idTSMServer = 4;
                    }
                }
                return getDBPage(DB);
            }
        }

        private static string getAHPage(Uri target, Server[] servers, int idServer)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(target);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                httpWebRequest.Timeout = timeOutAHPage;
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.CookieContainer.Add(new Cookie("realmId", servers[idServer].cookie) { Domain = target.Host });
                httpWebRequest.CookieContainer.Add(new Cookie("regionId", regionidValue) { Domain = target.Host });
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            timeOutAHPage = 5000;
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("Exception_AH_page.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                timeOutAHPage += 1000;
                return getAHPage(target, servers, idServer);
            }
        }
    }
}