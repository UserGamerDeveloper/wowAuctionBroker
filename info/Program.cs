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
        public static Settings settings;
        public static readonly object consoleLocker = new object();

        static void Main(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                Util.ExceptionLogAndAlert(e);
            }
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
                new Thread(new ThreadStart(server.Parse)).Start();
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

        //private static Dictionary<int, RecipeData> DeserializeRecipes()
        //{
        //    Dictionary<int, RecipeData> recipeDataById = new Dictionary<int, RecipeData>();
        //    using (FileStream fs = new FileStream("recipes.xml", FileMode.Open))
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(RecipeData[]));
        //        RecipeData[] recipeDataArray = (RecipeData[])xmlSerializer.Deserialize(fs);
        //        foreach (RecipeData recipe in recipeDataArray)
        //        {
        //            recipeDataById.Add(recipe.ID, recipe);
        //        }
        //    }
        //    Dictionary<int, ItemData> itemsDataById = DeserializeItemsData();
        //    foreach (var recipeData in recipeDataById.Values)
        //    {
        //        recipeData.SetData(itemsDataById);
        //    }
        //    return recipeDataById;
        //}

        private static Dictionary<int, RecipeData> GetRecipeDataById()
        {
            const double DefaultNeedMillisecondsToCraft = 2000d;

            List<RecipeData> recipesData = new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    561697,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    679654,
                    true),
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    574983,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    695731,
                    true),
                new RecipeData(
                    RecipeInfo.Silkweave_Slippers,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shaldorei_Silk , 12}
                    },
                    292241,
                    DefaultNeedMillisecondsToCraft,
                    5000,
                    394652),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    399124,
                    DefaultNeedMillisecondsToCraft,
                    60000,
                    482941,
                    true),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    390773,
                    DefaultNeedMillisecondsToCraft,
                    60000,
                    472837,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    563788,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    682185,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    577018,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    698193,
                    true),
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 12}
                    },
                    279504,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    377451),
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 12}
                    },
                    283778,
                    DefaultNeedMillisecondsToCraft,
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
            using (FileStream fs = new FileStream("recipes.xml", FileMode.Create))
            {
                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(List<RecipeData>));
                serverXmlSerializer.Serialize(fs, recipesData);
            }
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

        //private static Dictionary<int, ItemData> DeserializeItemsData()
        //{
        //    Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
        //    using (FileStream fs = new FileStream("items.xml", FileMode.Open))
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemData[]));
        //        ItemData[] items = (ItemData[])xmlSerializer.Deserialize(fs);
        //        foreach (ItemData item in items)
        //        {
        //            itemsDataById.Add(item.id, item);
        //        }
        //    }
        //    return itemsDataById;
        //}

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
            using (FileStream fs = new FileStream("items.xml", FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ItemData>));
                xmlSerializer.Serialize(fs, items);
            }
            Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
            foreach (var item in items)
            {
                itemsDataById.Add(item.id, item);
            }
            return itemsDataById;
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
    }
}