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
        static int TARGET_PROFIT;
        static SoundPlayer alert = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\alert.wav");
        static SoundPlayer simpleSound = new SoundPlayer(@"F:\d\прожекты\C#\wow\info\bin\Debug\music.wav");
        const string SILK = "https://www.tradeskillmaster.com/items/shal-dorei-silk-124437?sort=buyout";
        const string LEATHER = "https://www.tradeskillmaster.com/items/stonehide-leather-124113?sort=buyout";
        const string SCALE = "https://www.tradeskillmaster.com/items/stormscale-124115?sort=buyout";
        const string Tidespray_Linen = "https://www.tradeskillmaster.com/items/tidespray-linen-152576?sort=buyout";
        const string Shimmerscale = "https://www.tradeskillmaster.com/items/shimmerscale-153050?sort=buyout";
        const string BloodStainedBone = "https://www.tradeskillmaster.com/items/blood-stained-bone-154164?sort=buyout";
        const string CoarseLeather = "https://www.tradeskillmaster.com/items/coarse-leather-152541?sort=buyout";
        static int count_IN_STACK;
        static int TARGET_INCOME_IN_HOUR;
        const double TIME_NEED = 166.66666666666666666666666666666666;
        const double TIME_NEED_BloodStainedBone = 250;
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
            if (args.Length == 6)
            {
                TARGET_PROFIT = Convert.ToInt32(args[0]);
                int COST_BONUS_Shaldorei_Silk = Convert.ToInt32(args[1]);
                int COST_BONUS_LEATHER = Convert.ToInt32(args[2]);
                int COST_BONUS_SCALE = Convert.ToInt32(args[3]);
                count_IN_STACK = Convert.ToInt32(args[4]);
                TARGET_INCOME_IN_HOUR = Convert.ToInt32(args[5]);
                costsItem = new Dictionary<ItemType, long>
                {
                                { ItemType.SILK,COST_Shaldorei_Silk},
                                { ItemType.LEATHER,COST_LEATHER},
                                { ItemType.SCALE,COST_SCALE},
                                { ItemType.Tidespray_Linen,COST_Tidespray_Linen}
                };
                try
                {

                    start();
                }
                catch (Exception e)
                {
                    File.AppendAllText("Exception.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                    Console.WriteLine("Перезагрузка \n");
                    alert.Play();
                    start();
                }
            }
            else
            {
                Console.WriteLine("нет аргумента");
                Console.ReadLine();
            }
        }

        private static void start()
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

                        Item silk = new Item("ткань", costsItem[ItemType.SILK], SILK, TIME_NEED);
                        Item leather = new Item("кожа", costsItem[ItemType.LEATHER], LEATHER, TIME_NEED);
                        Item scale = new Item("чещуя", costsItem[ItemType.SCALE], SCALE, TIME_NEED);
                        Item Linen = new Item("лен", costsItem[ItemType.Tidespray_Linen], Tidespray_Linen, TIME_NEED);
                        Item shimmerscale = new Item("Shimmerscale", 0, Shimmerscale, 0);
                        Item bloodStainedBone = new Item("BloodStainedBone", 0, BloodStainedBone, TIME_NEED_BloodStainedBone);
                        Item coarseLeather = new Item("CoarseLeather", 0, CoarseLeather, 0);

                        Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>
                            {
                                { ItemType.SILK,silk},
                                { ItemType.LEATHER,leather},
                                { ItemType.SCALE,scale},
                                { ItemType.Tidespray_Linen,Linen},
                                { ItemType.Shimmerscale,shimmerscale},
                                { ItemType.BloodStainedBone,bloodStainedBone},
                                { ItemType.CoarseLeather,coarseLeather}
                            };

                        //Recipe coarseLeatherCestus = new Recipe(
                        //    new List<ItemType>
                        //    {
                        //        ItemType.BloodStainedBone,
                        //        ItemType.CoarseLeather
                        //    },
                        //    new Dictionary<ItemType, int>
                        //    {
                        //        { ItemType.BloodStainedBone,8},
                        //        { ItemType.CoarseLeather,10}
                        //    },
                        //    563788
                        //);
                        //Recipe shimmerscaleStriker = new Recipe(
                        //    new List<ItemType>
                        //    {
                        //        ItemType.BloodStainedBone,
                        //        ItemType.Shimmerscale
                        //    },
                        //    new Dictionary<ItemType, int>
                        //    {
                        //        { ItemType.BloodStainedBone,8},
                        //        { ItemType.Shimmerscale,10}
                        //    },
                        //    561697
                        //);

                        //Dictionary<Recipes, Recipe> recipesDictionary = new Dictionary<Recipes, Recipe>
                        //{
                        //    { Recipes.CoarseLeatherCestus,coarseLeatherCestus},
                        //    { Recipes.ShimmerscaleStriker,shimmerscaleStriker}
                        //};

                        int indexBloodStainedBone = 0;
                        int indexCoarseLeather = 0;
                        int indexShimmerscale = 0;
                        AuctionPageHTMLParser parserBloodStainedBone = new AuctionPageHTMLParser(items[ItemType.BloodStainedBone].uri, server.cookie);
                        AuctionPageHTMLParser parserCoarseLeather = new AuctionPageHTMLParser(items[ItemType.CoarseLeather].uri, server.cookie);
                        AuctionPageHTMLParser parserShimmerscale = new AuctionPageHTMLParser(items[ItemType.Shimmerscale].uri, server.cookie);
                        if (parserBloodStainedBone.hasBid())
                        {
                            while (true)
                            {
                                int countShimmerscale = 0;
                                int countCoarseLeather = 0;
                                int countBloodStainedBone = 0;
                                List<Bid> bidsBloodStainedBone = new List<Bid>();
                                List<Bid> bidsCoarseLeather = new List<Bid>();
                                List<Bid> bidsShimmerscale = new List<Bid>();
                                int bidCount = parserBloodStainedBone.getBidCount();
                                for (; indexBloodStainedBone < bidCount; indexBloodStainedBone++)
                                {
                                    int countInBid = parserBloodStainedBone.getCountInBid(indexBloodStainedBone);
                                    long costPerItem = (long)Math.Floor(parserBloodStainedBone.getCostBid(indexBloodStainedBone) / (double)countInBid);
                                    if (costPerItem > 0)
                                    {
                                        countBloodStainedBone += countInBid;
                                        bidsBloodStainedBone.Add(new Bid(ItemType.BloodStainedBone, countInBid, costPerItem, parserBloodStainedBone.getAutor(indexBloodStainedBone), 0));
                                        if (countBloodStainedBone % 8 == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if ((countBloodStainedBone % 8 == 0)&&(countBloodStainedBone>0))
                                {
                                    int countRecipe = countBloodStainedBone / 8;
                                    if (parserCoarseLeather.hasBid())
                                    {
                                        bidCount = parserCoarseLeather.getBidCount();
                                        for (; indexCoarseLeather < bidCount; indexCoarseLeather++)
                                        {
                                            int countInBid = parserCoarseLeather.getCountInBid(indexCoarseLeather);
                                            long costPerItem = (long)Math.Floor(parserCoarseLeather.getCostBid(indexCoarseLeather) / (double)countInBid);
                                            if (costPerItem > 0)
                                            {
                                                countCoarseLeather += countInBid;
                                                bidsCoarseLeather.Add(new Bid(ItemType.CoarseLeather, countInBid, costPerItem, parserCoarseLeather.getAutor(indexCoarseLeather), 0));
                                                if (countCoarseLeather / 10 >= countRecipe)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (parserShimmerscale.hasBid())
                                    {
                                        bidCount = parserShimmerscale.getBidCount();
                                        for (; indexShimmerscale < bidCount; indexShimmerscale++)
                                        {
                                            int countInBid = parserShimmerscale.getCountInBid(indexShimmerscale);
                                            long costPerItem = (long)Math.Floor(parserShimmerscale.getCostBid(indexShimmerscale) / (double)countInBid);
                                            if (costPerItem > 0)
                                            {
                                                countShimmerscale += countInBid;
                                                bidsShimmerscale.Add(new Bid(ItemType.Shimmerscale, countInBid, costPerItem, parserShimmerscale.getAutor(indexShimmerscale), 0));
                                                if (countShimmerscale / 10 >= countRecipe)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    long costBidsBloodStainedBone = 0;
                                    foreach (Bid bid in bidsBloodStainedBone)
                                    {
                                        costBidsBloodStainedBone += bid.costPerItem * bid.count;
                                    }
                                    long costBloodStainedBone = costBidsBloodStainedBone / countBloodStainedBone;
                                    if ((countCoarseLeather / 10f == countRecipe) && (countCoarseLeather > 0))
                                    {
                                        long costBidsCoarseLeather = 0;
                                        foreach (Bid bid in bidsCoarseLeather)
                                        {
                                            costBidsCoarseLeather += bid.costPerItem * bid.count;
                                        }
                                        long costCoarseLeather = costBidsCoarseLeather / countCoarseLeather;
                                        long profit = 563788 - (costBloodStainedBone * 8 + costCoarseLeather * 10);
                                        if ((countShimmerscale / 10f == countRecipe) && (countShimmerscale > 0))
                                        {
                                            long costBidsShimmerscale = 0;
                                            foreach (Bid bid in bidsShimmerscale)
                                            {
                                                costBidsShimmerscale += bid.costPerItem * bid.count;
                                            }
                                            long costShimmerscale = costBidsShimmerscale / countShimmerscale;
                                            long profitt = 561697 - (costBloodStainedBone * 8 + costShimmerscale * 10);
                                            if ((profitt > 0) || (profit > 0))
                                            {
                                                if (profitt > profit)
                                                {
                                                    foreach (Bid bid in bidsBloodStainedBone)
                                                    {
                                                        bid.profitPerItem = profitt / 8;
                                                        bid.profit = bid.profitPerItem * bid.count;
                                                    }
                                                    items[ItemType.Shimmerscale].bids.AddRange(bidsShimmerscale);
                                                }
                                                else
                                                {
                                                    foreach (Bid bid in bidsBloodStainedBone)
                                                    {
                                                        bid.profitPerItem = profit / 8;
                                                        bid.profit = bid.profitPerItem * bid.count;
                                                    }
                                                    items[ItemType.CoarseLeather].bids.AddRange(bidsCoarseLeather);
                                                }
                                                bids.AddRange(bidsBloodStainedBone);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (profit > 0)
                                            {
                                                foreach (Bid bid in bidsBloodStainedBone)
                                                {
                                                    bid.profitPerItem = profit / 8;
                                                    bid.profit = bid.profitPerItem * bid.count;
                                                }
                                                items[ItemType.CoarseLeather].bids.AddRange(bidsCoarseLeather);
                                                bids.AddRange(bidsBloodStainedBone);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if ((countShimmerscale / 10f == countRecipe) && (countShimmerscale > 0))
                                        {
                                            long costBidsShimmerscale = 0;
                                            foreach (Bid bid in bidsShimmerscale)
                                            {
                                                costBidsShimmerscale += bid.costPerItem * bid.count;
                                            }
                                            long costShimmerscale = costBidsShimmerscale / countShimmerscale;
                                            long profitt = 561697 - (costBloodStainedBone * 8 + costShimmerscale * 10);
                                            if (profitt > 0)
                                            {
                                                foreach (Bid bid in bidsBloodStainedBone)
                                                {
                                                    bid.profitPerItem = profitt / 8;
                                                    bid.profit = bid.profitPerItem * bid.count;
                                                }
                                                items[ItemType.Shimmerscale].bids.AddRange(bidsShimmerscale);
                                                bids.AddRange(bidsBloodStainedBone);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        foreach (ItemType item in server.activeItems)
                        {
                            Item itemData = items[item];
                            AuctionPageHTMLParser parser = new AuctionPageHTMLParser(itemData.uri, server.cookie);
                            if (parser.hasBid())
                            {
                                int bidCount = parser.getBidCount();
                                for (int i = 0; i < bidCount; i++)
                                {
                                    int countInBid = parser.getCountInBid(i);
                                    long costPerItem = (long)Math.Floor(parser.getCostBid(i) / (double)countInBid);
                                    if (costPerItem > 0)
                                    {
                                        if (costPerItem <= itemData.costSell)
                                        {
                                            if (countInBid > count_IN_STACK)
                                            {
                                                bids.Add(new Bid(item, countInBid, costPerItem, parser.getAutor(i), (itemData.costSell - costPerItem) * countInBid));
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
                                timeNeed = timeNeed.AddMilliseconds(items[bids[i].itemType].TIME_NEED * bids[i].count);
                                //if (DateTime.Now.Hour >= servers[idServer].time)
                                //{
                                //    timeNeed = timeNeed.AddSeconds(servers[idServer].delaySecond);
                                //}
                                if (getFormatProfit(profit_all) >= TARGET_PROFIT)
                                {
                                    isCollectMoney = true;
                                }
                            }
                            else
                            {
                                DateTime tempTimeNeed = timeNeed.AddMilliseconds(items[bids[i].itemType].TIME_NEED * bids[i].count);
                                //if (DateTime.Now.Hour >= servers[idServer].time)
                                //{
                                //    tempTimeNeed = tempTimeNeed.AddSeconds(servers[idServer].delaySecond);
                                //}
                                double tempIncome = getIncomeInHour(profit_all + bids[i].profit, timeNeed);
                                if (tempIncome >= TARGET_INCOME_IN_HOUR)
                                {
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

                        double incomeInHour = getIncomeInHour(profit_all, timeNeed);
                        string str = "Профит ";
                        string profit_allStr = Math.Floor(getFormatProfit(profit_all)) + " " + Math.Floor(incomeInHour) + " " +
                            getTimeInSeconds(timeNeed) / 60 + " мин";

                        Item[] itemsArray = (new List<Item>(items.Values)).ToArray();
                        Array.Sort(itemsArray);
                        for (int i = itemsArray.Length - 1; i >= 0; i--)
                        {
                            itemsArray[i].printAndLog();
                        }

                        if (getFormatProfit(profit_all) >= TARGET_PROFIT / 2)
                        {
                            if (getFormatProfit(profit_all) >= TARGET_PROFIT && incomeInHour >= TARGET_INCOME_IN_HOUR)
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
                                Console.WriteLine(str + profit_allStr);
                            }
                        }
                        File.AppendAllText("log.txt", str + profit_allStr + "\n\n");

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

        private static double getIncomeInHour(long profit_all, DateTime timeNeed)
        {
            return getFormatProfit(profit_all) / getTimeInSeconds(timeNeed) * 3600;
        }

        private static float getFormatProfit(long profit_all)
        {
            return profit_all / 10000f;
        }

        private static int getTimeInSeconds(DateTime tempTimeNeed)
        {
            return tempTimeNeed.Millisecond / 1000 + tempTimeNeed.Second + tempTimeNeed.Minute * 60 + tempTimeNeed.Hour * 3600;
        }
    }
}