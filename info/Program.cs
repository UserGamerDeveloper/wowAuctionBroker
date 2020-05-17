using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Runtime.InteropServices;
using System.Media;
using System.Xml.Serialization;
using System.Linq;

namespace info
{
    public class Program
    {
        static Server[] servers;
        static Settings settings;
        static Dictionary<int, ItemData> ITEMS_DATA;
        public static bool DEBUG = false;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length>0)
                {
                    DEBUG = true;
                    Console.WriteLine("DEBUG");
                }
                start();
            }
            catch (Exception e)
            {
                File.AppendAllText("Exception.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n\n");
                Console.WriteLine("Перезагрузка\n");
                SoundPlayer alert = new SoundPlayer("music.wav");
                alert.Play();
                start();
            }
        }

        private static void start()
        {
            DeserializeServers();
            //SerializeServers();

            //SerializeRecipes();
            Dictionary<int, RecipeData> recipeIdAndRecipeData = DeserializeRecipes();

            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)xmlSerializer.Deserialize(fs);
            }

            DeserializeItemsData();
            //SerializeItemsData();

            foreach (var server in servers)
            {
                if (!DEBUG)
                {
                    server.SetFirstTimeUpdate();
                }
                server.SetRecipes(recipeIdAndRecipeData);

                List<RecipeData> serverRecipesList = new List<RecipeData>(server.recipes);
                while (serverRecipesList.Count > 0)
                {
                    HashSet<RecipeData> recipeDataTree = new HashSet<RecipeData>();
                    foreach (var idItem in serverRecipesList[0].ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        foreach (RecipeData recipe in serverRecipesList)
                        {
                            if (recipe.ID_ITEM_AND_NEED_AMOUNT.ContainsKey(idItem))
                            {
                                recipeDataTree.Add(recipe);
                            }
                        }
                    }
                    List<RecipeData> recipeDataList = new List<RecipeData>(recipeDataTree);
                    recipeDataList.Remove(server.recipes[0]);
                    while (recipeDataList.Count > 0)
                    {
                        RecipeData recipeData = recipeDataList[0];
                        recipeDataList.Remove(recipeData);

                        foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                        {
                            List<RecipeData> recipeDataListNotInTree = new List<RecipeData>(server.recipes);
                            foreach (var item in recipeDataTree)
                            {
                                recipeDataListNotInTree.Remove(item);
                            }
                            foreach (RecipeData recipe in recipeDataListNotInTree)
                            {
                                if (recipe.ID_ITEM_AND_NEED_AMOUNT.ContainsKey(idItem))
                                {
                                    recipeDataTree.Add(recipe);
                                    recipeDataList.Add(recipe);
                                }
                            }
                        }
                    }
                    foreach (var item in recipeDataTree)
                    {
                        serverRecipesList.Remove(item);
                    }
                    server.recipeDataTrees.Add(recipeDataTree);
                }
            }
            Array.Sort(servers);
            for (int idServer = servers.Length - 1; idServer >= 0; idServer--)
            {
                Server server = servers[idServer];
                if (server.hasUpdate(true))
                {
                    parseServer(server);
                }
            }
            while (true)
            {
                foreach (var server in servers)
                {
                    if (server.hasUpdate(false))
                    {
                        parseServer(server);
                    }
                }
            }
        }

        private static Dictionary<int, RecipeData> DeserializeRecipes()
        {
            Dictionary<int, RecipeData> recipeData = new Dictionary<int, RecipeData>();
            using (FileStream fs = new FileStream("recipes.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeData[]));
                RecipeData[] recipeDataArray = (RecipeData[])xmlSerializer.Deserialize(fs);
                foreach (RecipeData recipe in recipeDataArray)
                {
                    recipeData.Add(recipe.ID, recipe);
                }
            }
            return recipeData;
        }

        private static void SerializeRecipes()
        {
            RecipeData[] recipeData = new RecipeData[]{
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    561697,
                    2000),
                new RecipeData(
                    RecipeInfo.Silkweave_Slippers,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shaldorei_Silk , 1}
                    },
                    24353,
                    166.6666666666666666666666666666),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 1}
                    },
                    23477,
                    117.64705885352),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    563788,
                    2000),
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 1}
                    },
                    23292,
                    166.6666666666666666666666666666),
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 1}
                    },
                    23648,
                    166.6666666666666666666666666666),
                new RecipeData(
                    RecipeInfo.Crafted_Dreadful_Gladiators_Cloak_of_Prowess,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Windwool_Cloth , 1}
                    },
                    6872,
                    300)
            };
            using (FileStream fs = new FileStream("recipes.xml", FileMode.Create))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(RecipeData[]));
                serverXmlSerializer.Serialize(fs, recipeData);
            }
        }

        private static void DeserializeServers()
        {
            using (FileStream fs = new FileStream("servers.xml", FileMode.Open))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
                servers = (Server[])serverXmlSerializer.Deserialize(fs);
            }
        }

        private static void DeserializeItemsData()
        {
            ITEMS_DATA = new Dictionary<int, ItemData>();
            using (FileStream fs = new FileStream("items.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemData[]));
                ItemData[] items = (ItemData[])xmlSerializer.Deserialize(fs);
                foreach (ItemData item in items)
                {
                    ITEMS_DATA.Add(item.id, item);
                }
            }
        }

        private static void SerializeItemsData()
        {
            ItemData[] items = new ItemData[]
            {
                new ItemData(ItemInfo.Shaldorei_Silk, "Shal'dorei Silk | Шал'дорайский шелк", "https://www.tradeskillmaster.com/items/shal-dorei-silk-124437?sort=buyout"),
                new ItemData(ItemInfo.Stonehide_Leather, "Stonehide Leather | Твердокаменная кожа", "https://www.tradeskillmaster.com/items/stonehide-leather-124113?sort=buyout"),
                new ItemData(ItemInfo.Stormscale, "Stormscale | Штормовая чешуя", "https://www.tradeskillmaster.com/items/stormscale-124115?sort=buyout"),
                new ItemData(ItemInfo.Tidespray_Linen, "Tidespray Linen | Морской лен", "https://www.tradeskillmaster.com/items/tidespray-linen-152576?sort=buyout"),
                new ItemData(ItemInfo.Shimmerscale, "Shimmerscale | Поблескивающая чешуя", "https://www.tradeskillmaster.com/items/shimmerscale-153050?sort=buyout"),
                new ItemData(ItemInfo.BloodStainedBone, "Blood-Stained Bone | Окровавленная кость", "https://www.tradeskillmaster.com/items/blood-stained-bone-154164?sort=buyout"),
                new ItemData(ItemInfo.CoarseLeather, "Coarse Leather | Шершавая кожа", "https://www.tradeskillmaster.com/items/coarse-leather-152541?sort=buyout")
            };
            using (FileStream fs = new FileStream("items.xml", FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemData[]));
                xmlSerializer.Serialize(fs, items);
            }
        }

        private static void SerializeServers()
        {
            servers = new Server[] {
                new Server(
                    ServerInfo.Twisting_Nether,
                    "90fa4178b714cf2b6db247b90969bd497c299c5bd1406170107c262a01cc8657a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A511%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Blackmoore,
                    "833adf3275b85e5ce95b750245f3013212fe3bff64cc4bd93df24598a982a7d5a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A333%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Antonidas,
                    "4189f471c98b39465bf62b9def9057c8aa4e28cce9ddc285580e5d5448caa3f7a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A289%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Ravencrest,
                    "69d53acf7ee790d7dbe3637413ab9b83a1ec12eef78e5e62257afb8d11c688eaa%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A496%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.svezewatel,
                    "47f9045c0840cbff05b27392572249648406aede97b1cf58a2f262dc08cd31d8a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A502%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.gorduni,
                    "1f3552dbc09c9097de622c7ffb69fda01b8f41789c712aee433334a85dcd5470a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A463%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Azjol_Nerub,
                    "9edded7a6c3280d4d88b8d7131a5edbbed9110c9ea177d7a51a1b1d3b50429d8a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A323%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Kazzak,
                    "5cea7fd01755cdcdbf49d3a0616c12e7766e729cb7bf439abfe10d7cc54e36d1a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A470%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.Silvermoon,
                    "ad0d4e64b8e6ccf1b277208434dbee62f1fa7390efc68ab47eb3439b1e216d47a%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A501%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    }),
                new Server(
                    ServerInfo.malganis,
                    "8108e41136d7d492af435be3e2ba019ca5cd4fe25b17955be6d128f1d4d7f14da%3A2%3A%7Bi%3A0%3Bs%3A7%3A%22realmId%22%3Bi%3A1%3Bi%3A437%3B%7D",
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 417},
                        {(int)RecipeInfo.Tidespray_Linen_Pants, 3530},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    })
            };
            using (FileStream fs = new FileStream("servers.xml", FileMode.Create))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
                serverXmlSerializer.Serialize(fs, servers);
            }
        }

        private static void parseServer(Server server)
        {
            server.printAndLog();

            List<Recipe> recipes = new List<Recipe>();

            foreach (var recipeDataTree in server.recipeDataTrees)
            {
                HashSet<ItemData> itemsDataTree = new HashSet<ItemData>();
                foreach (var recipeData in recipeDataTree)
                {
                    foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        itemsDataTree.Add(ITEMS_DATA[idItem]);
                    }
                }
                Dictionary<int, AuctionPageHTMLParser> parsersForTree = new Dictionary<int, AuctionPageHTMLParser>();
                foreach (var itemData in itemsDataTree)
                {
                    AuctionPageHTMLParser parser = new AuctionPageHTMLParser(itemData.getUri(), server.cookie);
                    parsersForTree.Add(itemData.id, parser);
                }
                while (true)
                {
                    List<Recipe> recipesTree = new List<Recipe>();
                    foreach (var recipeData in recipeDataTree)
                    {
                        bool enoughItemsForRecipe = true;
                        foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                        {
                            enoughItemsForRecipe = enoughItemsForRecipe && parsersForTree[idItem].HasRequiredAmount(recipeData.ID_ITEM_AND_NEED_AMOUNT[idItem]);
                        }
                        if (enoughItemsForRecipe)
                        {
                            Recipe recipe = new Recipe(recipeData);
                            long costCraft = 0;
                            foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                            {
                                recipe.items.Add(idItem, new List<Item>());
                                for (int i = 0; i < recipeData.ID_ITEM_AND_NEED_AMOUNT[idItem]; i++)
                                {
                                    Item item = parsersForTree[idItem].GetItem(i);
                                    costCraft += item.cost;
                                    recipe.items[idItem].Add(item);
                                }
                            }
                            recipe.SetProfit(costCraft);
                            if (recipe.profit > 0)
                            {
                                recipesTree.Add(recipe);
                            }
                        }
                    }
                    if (recipesTree.Count > 0)
                    {
                        recipesTree.Sort();
                        recipesTree[recipesTree.Count - 1].SetIncome();
                        recipes.Add(recipesTree[recipesTree.Count - 1]);
                        foreach (var idItem in recipesTree[recipesTree.Count - 1].items.Keys)
                        {
                            foreach (var item in recipesTree[recipesTree.Count - 1].items[idItem])
                            {
                                parsersForTree[idItem].Remove(item);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            recipes.Sort();

            long globalProfit = 0;
            DateTime timeNeed = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            Dictionary<int, List<Recipe>> targetProfitRecipes = new Dictionary<int, List<Recipe>>();
            Dictionary<int, double> summaryIncomeRecipesByRecipeId = new Dictionary<int, double>();
            Dictionary<int, long> summaryProfitRecipesByRecipeId = new Dictionary<int, long>();
            bool isCollectMoney = false;
            for (int i = recipes.Count - 1; i >= 0; i--)
            {
                if (!targetProfitRecipes.ContainsKey(recipes[i].recipeData.ID))
                {
                    targetProfitRecipes.Add(recipes[i].recipeData.ID, new List<Recipe>());
                }
                if (!summaryIncomeRecipesByRecipeId.ContainsKey(recipes[i].recipeData.ID))
                {
                    summaryIncomeRecipesByRecipeId.Add(recipes[i].recipeData.ID, 0f);
                }
                if (!summaryProfitRecipesByRecipeId.ContainsKey(recipes[i].recipeData.ID))
                {
                    summaryProfitRecipesByRecipeId.Add(recipes[i].recipeData.ID, 0);
                }
                if (!isCollectMoney)
                {
                    globalProfit += recipes[i].profit;
                    summaryProfitRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].profit;
                    targetProfitRecipes[recipes[i].recipeData.ID].Add(recipes[i]);
                    summaryIncomeRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].income;
                    timeNeed = timeNeed.AddMilliseconds(recipes[i].recipeData.TIME_NEED);
                    if (Util.convertCopperToGold(globalProfit) >= settings.TARGET_PROFIT)
                    {
                        isCollectMoney = true;
                    }
                }
                else
                {
                    DateTime tempTimeNeed = timeNeed.AddMilliseconds(recipes[i].recipeData.TIME_NEED);
                    double tempIncome = Util.getIncomeGoldInHour(globalProfit + recipes[i].profit, tempTimeNeed);
                    if (tempIncome >= settings.TARGET_INCOME_IN_HOUR)
                    {
                        globalProfit += recipes[i].profit;
                        summaryProfitRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].profit;
                        targetProfitRecipes[recipes[i].recipeData.ID].Add(recipes[i]);
                        summaryIncomeRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].income;
                        timeNeed = tempTimeNeed;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            double incomeInHour = Util.getIncomeGoldInHour(globalProfit, timeNeed);
            const string ITEM_STRING = "\t\t{0}\n\t\t\tКол-во: {1}\tЦена: {2:# ##}";
            const string STRING = "Профит ";
            string globalProfitString = Math.Floor(Util.convertCopperToGold(globalProfit)) + " " + Math.Floor(incomeInHour) + " " +
                Math.Floor(Util.getTimeInMinuts(timeNeed)) + " мин";

            //if (getProfitInGold(globalProfit) >= settings.TARGET_PROFIT)
            if (true)
            {
                foreach (var summaryIncomeRecipesPair in summaryIncomeRecipesByRecipeId.OrderByDescending(pair => pair.Value))
                {
                    Console.WriteLine();
                    string recipeString = String.Format(
                        "\t{0}\t\t\t\tПрофит: {1} {2}",
                        targetProfitRecipes[summaryIncomeRecipesPair.Key][0].recipeData.name,
                        Math.Floor(Util.convertCopperToGold(summaryProfitRecipesByRecipeId[summaryIncomeRecipesPair.Key])),
                        Math.Floor(summaryIncomeRecipesPair.Value / targetProfitRecipes[summaryIncomeRecipesPair.Key].Count));
                    Console.WriteLine(recipeString);
                    File.AppendAllText("log.txt", recipeString + "\n");
                    foreach (var itemId in targetProfitRecipes[summaryIncomeRecipesPair.Key][0].recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        int countItems = 0;
                        long cost = 0;
                        foreach (var recipe in targetProfitRecipes[summaryIncomeRecipesPair.Key])
                        {
                            foreach (var item in recipe.items[itemId])
                            {
                                countItems++;
                                cost += item.cost;
                            }
                        }
                        string printStr = String.Format(ITEM_STRING, ITEMS_DATA[itemId].itemName, countItems, Util.convertCopperToSilver(cost));
                        Console.WriteLine(printStr);
                        File.AppendAllText("log.txt", printStr + "\n");
                    }
                }
                if (Util.convertCopperToGold(globalProfit) >= settings.TARGET_PROFIT && incomeInHour >= settings.TARGET_INCOME_IN_HOUR)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(STRING);
                    Console.ResetColor();
                    Console.WriteLine(globalProfitString);

                    SoundPlayer simpleSound = new SoundPlayer("music.wav");
                    simpleSound.PlayLooping();
                    Console.ReadLine();
                    simpleSound.Stop();
                }
                else
                {
                    Console.WriteLine(STRING + globalProfitString);
                    if (globalProfit > 0)
                    {
                        SoundPlayer alert = new SoundPlayer("alert.wav");
                        alert.Play();
                    }
                }
                File.AppendAllText("log.txt", STRING + globalProfitString + "\n\n");
            }
            //foreach (var idItem in items.Keys)
            //{
            //    long cost = 0;
            //    foreach (var item in items[idItem])
            //    {
            //        cost += item.cost;
            //    }
            //    string printStr = String.Format(ITEM_STRING, ITEMS_DATA[idItem].itemName, items[idItem].Count, convertToSilver(cost));
            //    File.AppendAllText("log.txt", printStr + "\n");
            //}
            //File.AppendAllText("log.txt", STRING + globalProfitString + "\n\n");

            using (FileStream fs = new FileStream("servers.xml", FileMode.Create))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
                serverXmlSerializer.Serialize(fs, servers);
            }
            Console.WriteLine();
            Thread.Sleep(3000);
        }
    }
}