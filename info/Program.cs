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

namespace info
{
    public class Program
    {
        const int COST_Shaldorei_Silk = 23936;
        const int COST_LEATHER = 23648;
        const int COST_SCALE = 23292;
        const int COST_Tidespray_Linen = 19949;
        static int requestMoney;
        static SoundPlayer alert = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\alert.wav");
        static SoundPlayer simpleSound = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\music.wav");
        const string SILK = "https://www.tradeskillmaster.com/items/shal-dorei-silk-124437?sort=buyout";
        const string LEATHER = "https://www.tradeskillmaster.com/items/stonehide-leather-124113?sort=buyout";
        const string SCALE = "https://www.tradeskillmaster.com/items/stormscale-124115?sort=buyout";
        const string Tidespray_Linen = "https://www.tradeskillmaster.com/items/tidespray-linen-152576?sort=buyout";
        public const string regionidValue = "1de8a1f36d12d6d93b180176e811127d35fb174f75e311c5025f1fb101c6194aa%3A2%3A%7Bi%3A0%3Bs%3A8%3A%22regionId%22%3Bi%3A1%3Bs%3A2%3A%22EU%22%3B%7D";
        static int count_IN_STACK;
        static int N_INCOME;
        const double TIME_NEED = 166.66666666666666666666666666666666;
        static Server[] servers;
        //const string antonidas = "4189f471c98b39465bf62b9def9057c8aa4e28cce9ddc285580e5d5448caa3f7a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A289%3B%7D";
        //const string blackmoore = "833adf3275b85e5ce95b750245f3013212fe3bff64cc4bd93df24598a982a7d5a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A333%3B%7D";
        //const string malganis = "8108e41136d7d492af435be3e2ba019ca5cd4fe25b17955be6d128f1d4d7f14da%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A437%3B%7D";
        //const string Ravencrest = "69d53acf7ee790d7dbe3637413ab9b83a1ec12eef78e5e62257afb8d11c688eaa%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A496%3B%7D";
        //const string Twisting_Nether  = "90fa4178b714cf2b6db247b90969bd497c299c5bd1406170107c262a01cc8657a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A511%3B%7D";
        //const string svezewatel = "47f9045c0840cbff05b27392572249648406aede97b1cf58a2f262dc08cd31d8a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A502%3B%7D";
        //const string gorduni = "1f3552dbc09c9097de622c7ffb69fda01b8f41789c712aee433334a85dcd5470a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A463%3B%7D";
        //static Server[] servers = new Server[] {
        //    new Server((int)ServerId.Twisting_Nether, Twisting_Nether, new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Twisting Nether",18,0),
        //    new Server((int)ServerId.Blackmoore, blackmoore, new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Blackmoore",18,0),
        //    new Server((int)ServerId.Antonidas, antonidas,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Antonidas",20,0),
        //    new Server((int)ServerId.Ravencrest, Ravencrest,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"Ravencrest",18,0),
        //    new Server((int)ServerId.malganis, malganis,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"malganis",18,0),
        //    new Server((int)ServerId.svezewatel, svezewatel,new List<ItemType>{ ItemType.SILK, ItemType.LEATHER, ItemType.SCALE},"свежеватель",18,0),
        //    new Server((int)ServerId.gorduni, gorduni,new List<ItemType>{ ItemType.LEATHER, ItemType.SCALE},"гордунни",18,0)
        //};
        public static Dictionary<ItemType, long> costsItem;
        static XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
       // static XmlSerializer itemXmlSerializer = new XmlSerializer(typeof(Item[]));

        static void Main(string[] args)
        {
            if (args.Length==6)
            {
                requestMoney = Convert.ToInt32(args[0]);
                int COST_BONUS_Shaldorei_Silk = Convert.ToInt32(args[1]);
                int COST_BONUS_LEATHER = Convert.ToInt32(args[2]);
                int COST_BONUS_SCALE = Convert.ToInt32(args[3]);
                count_IN_STACK = Convert.ToInt32(args[4]);
                N_INCOME = Convert.ToInt32(args[5]);
                costsItem = new Dictionary<ItemType, long>
                {
                                { ItemType.SILK,COST_Shaldorei_Silk},
                                { ItemType.LEATHER,COST_LEATHER},
                                { ItemType.SCALE,COST_SCALE},
                                { ItemType.Tidespray_Linen,COST_Tidespray_Linen}
                };
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
                    servers = (Server[])serverXmlSerializer.Deserialize(fs);
                }
                //using (FileStream fs = new FileStream("data.xml", FileMode.Create))
                //{
                //    formatter.Serialize(fs, servers);
                //}
                //using (FileStream fs = new FileStream("item.xml", FileMode.Create))
                //{
                //    serverXmlSerializer.Serialize(fs, servers);
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
                        Server server = servers[idServer];
                        if (server.isUpdate())
                        {
                            server.printAndLog();

                            List<Bid> bids = new List<Bid>();

                            Item silk = new Item("   ткань", costsItem[ItemType.SILK], SILK);
                            Item leather = new Item("   кожа", costsItem[ItemType.LEATHER], LEATHER);
                            Item scale = new Item("   чещуя", costsItem[ItemType.SCALE], SCALE);
                            Item Linen = new Item("   лен", costsItem[ItemType.Tidespray_Linen], Tidespray_Linen);

                            Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>
                            {
                                { ItemType.SILK,silk},
                                { ItemType.LEATHER,leather},
                                { ItemType.SCALE,scale},
                                { ItemType.Tidespray_Linen,Linen}
                            };

                            foreach (ItemType item in server.activeItems)
                            {
                                AuctionPageHTMLParser parser = new AuctionPageHTMLParser(items[item].uri, server.cookie);
                                if (parser.hasBid())
                                {
                                    int bidCount = parser.getBidCount();
                                    for (int i = 0; i < bidCount; i++)
                                    {
                                        int countInBid = parser.getCountInBid(i);
                                        long costItem = (long)Math.Floor(parser.getCostBid(i) / (double)countInBid);
                                        if (costItem > 0)
                                        {
                                            if (costItem <= costsItem[item])
                                            {
                                                if (countInBid > count_IN_STACK)
                                                {
                                                    bids.Add(new Bid(item, countInBid, costItem, parser.getAutor(i)));
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
                                    items[bids[i].itemType].profit += bids[i].profit;
                                    items[bids[i].itemType].count += bids[i].count;
                                    items[bids[i].itemType].bids.Add(bids[i]);

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

                                        items[bids[i].itemType].profit += bids[i].profit;
                                        items[bids[i].itemType].count += bids[i].count;
                                        items[bids[i].itemType].bids.Add(bids[i]);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            string str = "Профит ";
                            string profit_allStr = Math.Floor(profit_all / 10000f) + " " + Math.Floor(income)+" " 
                                +(timeNeed.Millisecond / 1000f + timeNeed.Second + timeNeed.Minute * 60 + timeNeed.Hour * 3600)/60+ " мин";
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
                                serverXmlSerializer.Serialize(fs, servers);
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
    }
}