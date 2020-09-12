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
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace info
{
    public class Program
    {
        //static Server[] servers;
        static Settings settings;
        public static readonly object consoleLocker = new object();
        private const double ChanceRandomProfit = 0.165562913907285d;

        static void Main(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                ExceptionLogAndAlert(e);
            }
        }

        private static void ExceptionLogAndAlert(Exception e)
        {
            Exception ex = e;
            string log = DateTime.Now.ToString() + "\n";
            while (ex != null)
            {
                log += String.Format("{0} {1} \n", ex.TargetSite.Name, ex.Message);
                ex = ex.InnerException;
            }
            File.AppendAllText("Exception.txt", log + e.StackTrace + "\n\n");
            Console.WriteLine("Надо перезагрузить\n");
            SoundPlayer alert = new SoundPlayer("music.wav");
            alert.PlayLooping();
            Console.ReadLine();
        }

        private static void Start()
        {
            //SerializeServers();
            List<Server> servers = DeserializeServers();
            DeserializeSettings();
            ClientData clientData = DeserializeClientData();
            Dictionary<int, RecipeData> recipeDataById = GetRecipeDataById();
            foreach (var server in servers)
            {
                server.SetData(clientData.RealmsDatasByIdHouse[server.id], recipeDataById);
            }
            Server.SortByDescendingMoney(servers);
            string serversInfo = "";
            foreach (var server in servers)
            {
                serversInfo += server.GetInfo() + "\n";
            }
            Util.WriteLineAndLog(serversInfo);
            const string DELIMETR = "--------------------------------------------------------------------------------";
            Util.WriteLineAndLog(DELIMETR);

            Util.WaitEndMaintenance();

            foreach (var server in servers)
            {
                new Thread(new ParameterizedThreadStart(ParseServer)).Start(server);
            }
        }

        private static ClientData DeserializeClientData()
        {
            string getGoldAndRepStr = File.ReadAllText(settings.WOW_PATH + @"\_retail_\WTF\Account\449681846#1\SavedVariables\getGoldAndRep.lua");
            ClientData clientData = JsonConvert.DeserializeObject<ClientData>(ConvertLuaDataToJSON(getGoldAndRepStr));
            return clientData;
        }

        private static void DeserializeSettings()
        {
            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)xmlSerializer.Deserialize(fs);
            }
        }

        private static string ConvertLuaDataToJSON(string getGoldAndRepStr)
        {
            getGoldAndRepStr = getGoldAndRepStr.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("DB = ", "").Replace("[", "").Replace("]", "").Replace("=", ":");
            return getGoldAndRepStr;
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
            Dictionary<int, ItemData> itemsDataById = DeserializeItemsData();
            foreach (var recipeData in recipeDataById.Values)
            {
                recipeData.SetData(itemsDataById);
            }
            return recipeDataById;
        }

        private static Dictionary<int, RecipeData> GetRecipeDataById()
        {
            const double BFATimeNeed = 15000d / 70d;
            List<RecipeData> recipesData = new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    561697,
                    BFATimeNeed,
                    0,
                    679654),
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    574983,
                    BFATimeNeed,
                    0,
                    695731),
                new RecipeData(
                    RecipeInfo.Silkweave_Slippers,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shaldorei_Silk , 12}
                    },
                    292241,
                    2000,
                    5000,
                    292241),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    399124,
                    BFATimeNeed,
                    60000,
                    482941),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    390773,
                    BFATimeNeed,
                    60000,
                    472837),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    563788,
                    BFATimeNeed,
                    0,
                    682185),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    577018,
                    BFATimeNeed,
                    0,
                    698193),
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 12}
                    },
                    279504,
                    2000,
                    0,
                    377451),
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 12}
                    },
                    283778,
                    2000,
                    0,
                    283778),
                new RecipeData(
                    RecipeInfo.Crafted_Dreadful_Gladiators_Cloak_of_Prowess,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Windwool_Cloth , 20}
                    },
                    137455,
                    6000,
                    0,
                    137455)
            };
            //using (FileStream fs = new FileStream("recipes.xml", FileMode.Create))
            //{
            //    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(RecipeData[]));
            //    serverXmlSerializer.Serialize(fs, recipeData);
            //}
            Dictionary<int, RecipeData> recipeDataById = new Dictionary<int, RecipeData>();
            foreach (RecipeData recipe in recipesData)
            {
                recipeDataById.Add(recipe.ID, recipe);
            }
            Dictionary<int, ItemData> itemsDataById = SerializeItemsData();
            foreach (var recipeData in recipeDataById.Values)
            {
                recipeData.SetData(itemsDataById);
            }
            return recipeDataById;
        }

        private static List<Server> DeserializeServers()
        {
            List<Server> servers = new List<Server>();
            DirectoryInfo directoryInfo = new DirectoryInfo("realms");
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                //servers.Add(JsonConvert.DeserializeObject<Server>(File.ReadAllText(file.FullName)));
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                {
                    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                    servers.Add((Server)serverXmlSerializer.Deserialize(fs));
                }
            }
            return servers;
        }

        private static Dictionary<int, ItemData> DeserializeItemsData()
        {
            Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
            using (FileStream fs = new FileStream("items.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemData[]));
                ItemData[] items = (ItemData[])xmlSerializer.Deserialize(fs);
                foreach (ItemData item in items)
                {
                    itemsDataById.Add(item.id, item);
                }
            }
            return itemsDataById;
        }

        private static Dictionary<int, ItemData> SerializeItemsData()
        {
            List<ItemData> items = new List<ItemData>
            {
                new ItemData(ItemInfo.Shaldorei_Silk, "Shal'dorei Silk | Шал'дорайский шелк"),
                new ItemData(ItemInfo.Stonehide_Leather, "Stonehide Leather | Твердокаменная кожа"),
                new ItemData(ItemInfo.Stormscale, "Stormscale | Штормовая чешуя"),
                new ItemData(ItemInfo.Tidespray_Linen, "Tidespray Linen | Морской лен"),
                new ItemData(ItemInfo.Shimmerscale, "Shimmerscale | Поблескивающая чешуя"),
                new ItemData(ItemInfo.BloodStainedBone, "Blood-Stained Bone | Окровавленная кость"),
                new ItemData(ItemInfo.CoarseLeather, "Coarse Leather | Шершавая кожа"),
                new ItemData(ItemInfo.Windwool_Cloth, "Windwool Cloth | Ветрошерстяная ткань")
            };
            Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
            foreach (var item in items)
            {
                itemsDataById.Add(item.id, item);
            }
            return itemsDataById;
            //using (FileStream fs = new FileStream("items.xml", FileMode.Create))
            //{
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemData[]));
            //    xmlSerializer.Serialize(fs, items);
            //}
        }

        private static void SerializeServers()
        {
            List<Server> servers = new List<Server> {
                new Server(
                    HouseId.Twisting_Nether,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.Blackmoore,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.Antonidas,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.Ravencrest,
                    getAllRecipesAlliance()),
                new Server(
                    HouseId.svezewatel,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.gorduni,
                    getAllRecipesAlliance()),
                new Server(
                    HouseId.Azjol_Nerub,
                    getAllRecipesAlliance()),
                new Server(
                    HouseId.Kazzak,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.Silvermoon,
                    getAllRecipesAlliance()),
                new Server(
                    HouseId.Tyrande,
                    new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_H,
                        (int)RecipeInfo.Shimmerscale_Striker_H,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_H
                    }),
                new Server(
                    HouseId.Hyjal,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.Howling_Fjord,
                    getAllRecipesHorde()),
                new Server(
                    HouseId.malganis,
                    getAllRecipesAlliance())
            };
            foreach (var server in servers)
            {
                //File.WriteAllText(string.Format(@"realms\{0}.json", server.name), JsonConvert.SerializeObject(server, Formatting.Indented));
                using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", server.name), FileMode.Create))
                {
                    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                    serverXmlSerializer.Serialize(fs, server);
                }
            }
            //using (FileStream fs = new FileStream("servers.xml", FileMode.Create))
            //{
            //    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server[]));
            //    serverXmlSerializer.Serialize(fs, servers);
            //}
        }

        private static List<int> getAllRecipesHorde()
        {
            return new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_H,
                        (int)RecipeInfo.Shimmerscale_Striker_H,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_H,
                        (int)RecipeInfo.Warhide_Shoulderguard
            };
        }

        private static List<int> getAllRecipesAlliance()
        {
            return new List<int>{
                        (int)RecipeInfo.Battlebound_Spaulders,
                        (int)RecipeInfo.Coarse_Leather_Cestus_A,
                        (int)RecipeInfo.Shimmerscale_Striker_A,
                        (int)RecipeInfo.Silkweave_Slippers,
                        (int)RecipeInfo.Tidespray_Linen_Pants_A,
                        (int)RecipeInfo.Warhide_Shoulderguard
            };
        }

        private static void ParseServer(object obj)
        {
            try
            {
                Server server = obj as Server;
                while (true)
                {
                    DateTime dateTime = Util.UnixTimeStampToDateTime(server.timeUpdate);
                    DateTime timeNextUpdate = dateTime.AddHours(1d).AddMinutes(Util.AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA);
                    if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
                    {
                        if (server.HasUpdate())
                        {
                            Dictionary<int, List<Recipe>> recipesById = GetRecipes(server);

                            long globalProfit = 0;
                            int recipesCount = 0;
                            TimeSpan timeCraft = new TimeSpan();
                            Dictionary<int, double> summaryIncomeRecipesByRecipeId = new Dictionary<int, double>();
                            Dictionary<int, long> summaryProfitRecipesByRecipeId = new Dictionary<int, long>();
                            foreach (var recipeId in recipesById.Keys)
                            {
                                summaryIncomeRecipesByRecipeId.Add(recipeId, 0d);
                                summaryProfitRecipesByRecipeId.Add(recipeId, 0L);
                            }
                            foreach (var recipeList in recipesById.Values)
                            {
                                foreach (var recipe in recipeList)
                                {
                                    globalProfit += recipe.profit;
                                    summaryProfitRecipesByRecipeId[recipe.recipeData.ID] += recipe.profit;
                                    summaryIncomeRecipesByRecipeId[recipe.recipeData.ID] += recipe.IncomeGoldInHour;
                                    timeCraft += TimeSpan.FromMilliseconds(recipe.recipeData.NeedMillisecondsToCraft);
                                }
                            }

                            Dictionary<int, double> averageIncomeRecipesByRecipeId = new Dictionary<int, double>();
                            foreach (var summaryIncomeRecipeByRecipeIdKey in summaryIncomeRecipesByRecipeId.Keys)
                            {
                                averageIncomeRecipesByRecipeId.Add(
                                    summaryIncomeRecipeByRecipeIdKey,
                                    summaryIncomeRecipesByRecipeId[summaryIncomeRecipeByRecipeIdKey] / recipesById[summaryIncomeRecipeByRecipeIdKey].Count);
                            }

                            string printStr = server.GetNameAndTimeUpdate();
                            double globalRandomProfit = 0d;
                            foreach (var averageIncomeRecipeByRecipeIdPair in averageIncomeRecipesByRecipeId.OrderByDescending(pair => pair.Value))
                            {
                                int recipeId = averageIncomeRecipeByRecipeIdPair.Key;
                                RecipeData recipeData = recipesById[recipeId][0].recipeData;
                                double randomProfit = recipesById[recipeId].Count * recipeData.GetRandomProfit() * ChanceRandomProfit;
                                globalRandomProfit += randomProfit;
                                recipesCount += recipesById[recipeId].Count;
                                printStr += string.Format(
                                    "\n\t {0,-40} Профит: {1:0.} + {3:0.} {2:0.}\n",
                                    string.Format("{0} x {1}", recipeData.name, recipesById[recipeId].Count),
                                    Util.ConvertCopperToGold(summaryProfitRecipesByRecipeId[recipeId]),
                                    averageIncomeRecipeByRecipeIdPair.Value,
                                    Util.ConvertCopperToGold(randomProfit));
                                foreach (var itemData in recipeData.ItemsData)
                                {
                                    Dictionary<long, List<Item>> bidsItemInRecipeByCost = new Dictionary<long, List<Item>>();
                                    foreach (var recipe in recipesById[recipeId])
                                    {
                                        foreach (var item in recipe.items[itemData.id])
                                        {
                                            if (!bidsItemInRecipeByCost.ContainsKey(item.cost))
                                            {
                                                bidsItemInRecipeByCost.Add(item.cost, new List<Item>());
                                            }
                                            bidsItemInRecipeByCost[item.cost].Add(item);
                                        }
                                    }
                                    long maxPrice = 0;
                                    foreach (var bidsItemInRecipeByCostPair in bidsItemInRecipeByCost.OrderByDescending(pair => pair.Key))
                                    {
                                        maxPrice = bidsItemInRecipeByCostPair.Key;
                                        if (bidsItemInRecipeByCostPair.Value.Count >= recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id])
                                        {
                                            break;
                                        }
                                    }
                                    printStr += string.Format(
                                        "\t\t{0}\n\t\t\tМакс цена: \t{1:# ## ##.}\n",
                                        itemData.itemName,
                                        Util.ConvertCopperToSilver(maxPrice));
                                }
                            }
                            const string STRING = "Профит ";
                            string globalProfitString = string.Format("{0:0.} + {4:0.} {1:0.} {2:0.} мин, рецептов {3}",
                                Util.ConvertCopperToGold(globalProfit),
                                Util.GetIncomeGoldInHour(globalProfit, timeCraft),
                                timeCraft.TotalMinutes,
                                recipesCount,
                                Util.ConvertCopperToGold(globalRandomProfit));
                            lock (consoleLocker)
                            {
                                Util.WriteLineAndLog(printStr);
                                if (Util.ConvertCopperToGold(globalProfit + globalRandomProfit) > settings.TARGET_PROFIT)
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
                                    if (globalProfit > 0)
                                    {
                                        Console.WriteLine(STRING + globalProfitString);
                                        SoundPlayer alert = new SoundPlayer("alert.wav");
                                        alert.Play();
                                    }
                                }
                                File.AppendAllText("log.txt", STRING + globalProfitString + "\n\n");
                                Console.WriteLine();
                            }
                            using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", server.name), FileMode.Create))
                            {
                                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                                serverXmlSerializer.Serialize(fs, server);
                            }
                        }
                        else
                        {
                            Thread.Sleep(60000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogAndAlert(e);
            }
        }

        private static Dictionary<int, List<Recipe>> GetRecipes(Server server)
        {
            int targetIncomeInHour;
            if (server.farmMode)
            {
                targetIncomeInHour = 0;
            }
            else
            {
                targetIncomeInHour = settings.TARGET_INCOME_IN_HOUR;
            }
            Dictionary<int, List<Recipe>> recipesById = new Dictionary<int, List<Recipe>>();
            foreach (var recipeDataTree in server.RecipeDataTrees)
            {
                HashSet<ItemData> itemsDataTree = new HashSet<ItemData>();
                foreach (var recipeData in recipeDataTree)
                {
                    foreach (var itemData in recipeData.ItemsData)
                    {
                        itemsDataTree.Add(itemData);
                    }
                }
                Dictionary<int, ItemPageParser> parsersForTree = new Dictionary<int, ItemPageParser>();
                foreach (var itemData in itemsDataTree)
                {
                    ItemPageParser parser = new ItemPageParser(server.id, itemData.id);
                    parsersForTree.Add(itemData.id, parser);
                }
                while (true)
                {
                    List<Recipe> profitableRecipes = new List<Recipe>();
                    foreach (var recipeData in recipeDataTree)
                    {
                        bool enoughItemsForRecipe = true;
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            enoughItemsForRecipe = enoughItemsForRecipe &&
                                parsersForTree[itemData.id].HasRequiredAmount(recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]);
                        }
                        if (enoughItemsForRecipe)
                        {
                            Recipe recipe = new Recipe(recipeData, server, parsersForTree);
                            if (recipe.IncomeGoldInHour >= targetIncomeInHour)
                            {
                                profitableRecipes.Add(recipe);
                            }
                        }
                    }
                    if (profitableRecipes.Count > 0)
                    {
                        Recipe.SortByDescendingIncomeGoldInHour(profitableRecipes);
                        Recipe maxProfitableRecipe = profitableRecipes[0];
                        if (!recipesById.ContainsKey(maxProfitableRecipe.recipeData.ID))
                        {
                            recipesById.Add(maxProfitableRecipe.recipeData.ID, new List<Recipe>());
                        }
                        recipesById[maxProfitableRecipe.recipeData.ID].Add(maxProfitableRecipe);
                        foreach (var idItem in maxProfitableRecipe.items.Keys)
                        {
                            foreach (var item in maxProfitableRecipe.items[idItem])
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
            return recipesById;
        }
    }
}