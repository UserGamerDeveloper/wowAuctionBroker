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
                //if (args.Length>0)
                //{
                //    DEBUG = true;
                //    Console.WriteLine("DEBUG");
                //}
                start();
            }
            catch (Exception e)
            {
                File.AppendAllText("Exception.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n\n");
                Console.WriteLine("Перезагрузка\n");
                SoundPlayer alert = new SoundPlayer("music.wav");
                alert.Play();
                Console.ReadLine();
            }
        }

        private static void start()
        {
            DeserializeServers();
            //SerializeServers();

            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)xmlSerializer.Deserialize(fs);
            }

            DeserializeItemsData();
            //SerializeItemsData();

            //SerializeRecipes();
            Dictionary<int, RecipeData> recipeDataById = DeserializeRecipes();

            foreach (var server in servers)
            {
                server.SetRecipes(recipeDataById);

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
                if (server.hasUpdate())
                {
                    parseServer(server);
                }
                else
                {
                    break;
                }
            }
            WriteAndLogDelimetr();
            Array.Sort(servers);
            while (true)
            {
                if (Server
                    .UnixTimeStampToDateTime(servers[0].timeUpdate)
                    .AddHours(1d)
                    .AddMinutes(Server.AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA)
                    .CompareTo(DateTime.Now) == -1)
                {
                    List<Server> needUpdateServers = new List<Server>();
                    needUpdateServers.AddRange(servers);
                    while (needUpdateServers.Count > 0)
                    {
                        List<Server> updateServers = new List<Server>();
                        foreach (var server in needUpdateServers)
                        {
                            if (server.hasUpdate())
                            {
                                parseServer(server);
                                updateServers.Add(server);
                            }
                            else
                            {
                                Thread.Sleep(1000);
                            }
                        }
                        foreach (var server in updateServers)
                        {
                            needUpdateServers.Remove(server);
                        }
                    }
                    WriteAndLogDelimetr();
                    Array.Sort(servers);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void WriteAndLogDelimetr()
        {
            const string DELIMETR = "-------------------------------------------------------------------------";
            Console.WriteLine(DELIMETR);
            File.AppendAllText("log.txt", DELIMETR + "\n");
        }

        private static Dictionary<int, RecipeData> DeserializeRecipes()
        {
            Dictionary<int, RecipeData> recipeDataById = new Dictionary<int, RecipeData>();
            using (FileStream fs = new FileStream("recipes.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeData[]));
                RecipeData[] recipeDataArray = (RecipeData[])xmlSerializer.Deserialize(fs);
                foreach (RecipeData recipe in recipeDataArray)
                {
                    recipeDataById.Add(recipe.ID, recipe);
                }
            }
            return recipeDataById;
        }

        private static void SerializeRecipes()
        {
            const double BFA_TIME_NEED = 15000f/70f;
            RecipeData[] recipeData = new RecipeData[]{
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    561697,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    574983,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Silkweave_Slippers,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shaldorei_Silk , 12}
                    },
                    292241,
                    2000),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    399124,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    390773,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    563788,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    577018,
                    BFA_TIME_NEED),
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 12}
                    },
                    279504,
                    2000),
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 12}
                    },
                    283778,
                    2000),
                new RecipeData(
                    RecipeInfo.Crafted_Dreadful_Gladiators_Cloak_of_Prowess,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Windwool_Cloth , 20}
                    },
                    137455,
                    6000)
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
                new ItemData(ItemInfo.Shaldorei_Silk, "Shal'dorei Silk | Шал'дорайский шелк"),
                new ItemData(ItemInfo.Stonehide_Leather, "Stonehide Leather | Твердокаменная кожа"),
                new ItemData(ItemInfo.Stormscale, "Stormscale | Штормовая чешуя"),
                new ItemData(ItemInfo.Tidespray_Linen, "Tidespray Linen | Морской лен"),
                new ItemData(ItemInfo.Shimmerscale, "Shimmerscale | Поблескивающая чешуя"),
                new ItemData(ItemInfo.BloodStainedBone, "Blood-Stained Bone | Окровавленная кость"),
                new ItemData(ItemInfo.CoarseLeather, "Coarse Leather | Шершавая кожа")
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
                    HouseId.Twisting_Nether,
                    getDefaultSpendingHorde()),
                new Server(
                    HouseId.Blackmoore,
                    getDefaultSpendingHorde()),
                new Server(
                    HouseId.Antonidas,
                    getDefaultSpendingAlliance()),
                new Server(
                    HouseId.Ravencrest,
                    getHonoredSpendingAlliance()),
                new Server(
                    HouseId.svezewatel,
                    getHonoredSpendingHorde()),
                new Server(
                    HouseId.gorduni,
                    getHonoredSpendingAlliance()),
                new Server(
                    HouseId.Azjol_Nerub,
                    getDefaultSpendingAlliance()),
                new Server(
                    HouseId.Kazzak,
                    getDefaultSpendingHorde()),
                new Server(
                    HouseId.Silvermoon,
                    getDefaultSpendingAlliance()),
                new Server(
                    HouseId.Tyrande,
                    new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_H, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_H, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 5000},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_H, 60000}
                    }),
                new Server(
                    HouseId.malganis,
                    getDefaultSpendingAlliance())
            };
            using (FileStream fs = new FileStream("servers.xml", FileMode.Create))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
                serverXmlSerializer.Serialize(fs, servers);
            }
        }

        private static XmlSerializableDictionary<int, long> getHonoredSpendingHorde()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_H, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_H, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 4500},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_H, 54000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
        }

        private static XmlSerializableDictionary<int, long> getHonoredSpendingAlliance()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_A, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_A, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 4500},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_A, 54000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
        }

        private static XmlSerializableDictionary<int, long> getFrendlySpendingHorde()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_H, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_H, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 4750},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_H, 57000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
        }

        private static XmlSerializableDictionary<int, long> getFrendlySpendingAlliance()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_A, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_A, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 4750},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_A, 57000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
        }

        private static XmlSerializableDictionary<int, long> getDefaultSpendingHorde()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_H, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_H, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 5000},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_H, 60000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
        }

        private static XmlSerializableDictionary<int, long> getDefaultSpendingAlliance()
        {
            return new XmlSerializableDictionary<int, long>{
                        {(int)RecipeInfo.Battlebound_Spaulders, 0},
                        {(int)RecipeInfo.Coarse_Leather_Cestus_A, 0},
                        {(int)RecipeInfo.Shimmerscale_Striker_A, 0},
                        {(int)RecipeInfo.Silkweave_Slippers, 5000},
                        {(int)RecipeInfo.Tidespray_Linen_Pants_A, 60000},
                        {(int)RecipeInfo.Warhide_Shoulderguard, 0}
                    };
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
                    AuctionPageHTMLParser parser = new AuctionPageHTMLParser(server.id, itemData.id);
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
                            recipe.SetIncome();
                            if (recipe.incomeGoldInHour >= settings.TARGET_INCOME_IN_HOUR)
                            {
                                recipesTree.Add(recipe);
                            }
                        }
                    }
                    if (recipesTree.Count > 0)
                    {
                        recipesTree.Sort();
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
            Dictionary<int, List<Recipe>> recipesById = new Dictionary<int, List<Recipe>>();
            Dictionary<int, double> summaryIncomeRecipesByRecipeId = new Dictionary<int, double>();
            Dictionary<int, long> summaryProfitRecipesByRecipeId = new Dictionary<int, long>();
            for (int i = recipes.Count - 1; i >= 0; i--)
            {
                if (!recipesById.ContainsKey(recipes[i].recipeData.ID))
                {
                    recipesById.Add(recipes[i].recipeData.ID, new List<Recipe>());
                }
                if (!summaryIncomeRecipesByRecipeId.ContainsKey(recipes[i].recipeData.ID))
                {
                    summaryIncomeRecipesByRecipeId.Add(recipes[i].recipeData.ID, 0f);
                }
                if (!summaryProfitRecipesByRecipeId.ContainsKey(recipes[i].recipeData.ID))
                {
                    summaryProfitRecipesByRecipeId.Add(recipes[i].recipeData.ID, 0);
                }
                globalProfit += recipes[i].profit;
                summaryProfitRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].profit;
                recipesById[recipes[i].recipeData.ID].Add(recipes[i]);
                summaryIncomeRecipesByRecipeId[recipes[i].recipeData.ID] += recipes[i].incomeGoldInHour;
                timeNeed = timeNeed.AddMilliseconds(recipes[i].recipeData.TIME_NEED);
            }

            double incomeInHour = Util.getIncomeGoldInHour(globalProfit, timeNeed);
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
                        recipesById[summaryIncomeRecipesPair.Key][0].recipeData.name,
                        Math.Floor(Util.convertCopperToGold(summaryProfitRecipesByRecipeId[summaryIncomeRecipesPair.Key])),
                        Math.Floor(summaryIncomeRecipesPair.Value / recipesById[summaryIncomeRecipesPair.Key].Count));
                    Console.WriteLine(recipeString);
                    File.AppendAllText("log.txt", recipeString + "\n");
                    foreach (var itemId in recipesById[summaryIncomeRecipesPair.Key][0].recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        long cost = 0;
                        List<Item> itemsInRecipe = new List<Item>();
                        foreach (var recipe in recipesById[summaryIncomeRecipesPair.Key])
                        {
                            foreach (var item in recipe.items[itemId])
                            {
                                cost += item.cost;
                                itemsInRecipe.Add(item);
                            }
                        }
                        itemsInRecipe.Sort();
                        int countItemsByMaxCost = 0;
                        foreach (var item in itemsInRecipe)
                        {
                            if (item.cost == itemsInRecipe[itemsInRecipe.Count - 1].cost)
                            {
                                countItemsByMaxCost++;
                            }
                        }
                        string printStr = String.Format(
                            "\t\t{0}\n\t\t\tМакс цена: {4:# ##} \t Кол-во: {5}",
                            ITEMS_DATA[itemId].itemName,
                            itemsInRecipe.Count,
                            Util.convertCopperToSilver(cost),
                            Util.convertCopperToSilver(itemsInRecipe[0].cost),
                            Util.convertCopperToSilver(itemsInRecipe[itemsInRecipe.Count - 1].cost),
                            countItemsByMaxCost);
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
        }
    }
}