using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using wowCalc;

namespace info
{

    static class Loader
    {
        //private static ClientData GetData()
        //{
        //    //string getGoldAndRepStr = File.ReadAllText(Program.settings.WOW_PATH + @"\_retail_\WTF\Account\449681846#1\SavedVariables\getGoldAndRep.lua");
        //    //ClientData clientData = JsonConvert.DeserializeObject<ClientData>(ConvertLuaDataToJSON(getGoldAndRepStr));
        //    //return clientData;


        //}

        public static Settings DeserializeSettings()
        {
            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                return (Settings)xmlSerializer.Deserialize(fs);
            }
        }

        //private static string ConvertLuaDataToJSON(string getGoldAndRepStr)
        //{
        //    getGoldAndRepStr = getGoldAndRepStr.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("DB = ", "").Replace("[", "").Replace("]", "").Replace("=", ":");
        //    return getGoldAndRepStr;
        //}

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
            const double BFANeedMillisecondsToCraft = 1000d;

            List<RecipeData> recipesData = new List<RecipeData>{
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    561697,
                    BFANeedMillisecondsToCraft,
                    0,
                    679654,
                    true),
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    574983,
                    BFANeedMillisecondsToCraft,
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
                    BFANeedMillisecondsToCraft,
                    60000,
                    482941,
                    true),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    390773,
                    BFANeedMillisecondsToCraft,
                    60000,
                    472837,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    563788,
                    BFANeedMillisecondsToCraft,
                    0,
                    682185,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    577018,
                    BFANeedMillisecondsToCraft,
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
            Dictionary<int, RecipeData> recipeDataById = new Dictionary<int, RecipeData>();
            foreach (RecipeData recipe in recipesData)
            {
                recipeDataById.Add(recipe.ID, recipe);
            }
            Dictionary<int, ItemData> itemsDataById = GetItemsData();
            foreach (var recipeData in recipeDataById.Values)
            {
                recipeData.SetData(itemsDataById);
            }
            return recipeDataById;
        }

        public static Dictionary<string, Server> DeserializeServers()
        {
            Dictionary<string, Server> serversByName = new Dictionary<string, Server>();
            DirectoryInfo directoryInfo = new DirectoryInfo("realms");
            FileInfo[] files = directoryInfo.GetFiles();
            //ClientData clientData = GetData();
            Server.RefreshTokenPrice();
            Dictionary<int, RecipeData> recipeDataById = GetRecipeDataById();
            foreach (var file in files)
            {
                //servers.Add(JsonConvert.DeserializeObject<Server>(File.ReadAllText(file.FullName)));
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                {
                    XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                    Server server = (Server)serverXmlSerializer.Deserialize(fs);
                    server.SetData(recipeDataById);
                    serversByName.Add(server.Name, server);
                }
            }
            return serversByName;
        }

        //private static Dictionary<int, ItemData> DeserializeItemsData()
        //{
        //    Dictionary<int, ItemData> itemsDataById = new Dictionary<int, ItemData>();
        //    using (FileStream fs = new FileStream("items.xml", FileMode.Open))
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ItemData>));
        //        List<ItemData> items = (List<ItemData>)xmlSerializer.Deserialize(fs);
        //        foreach (ItemData item in items)
        //        {
        //            itemsDataById.Add(item.id, item);
        //        }
        //    }
        //    return itemsDataById;
        //}

        private static Dictionary<int, ItemData> GetItemsData()
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
        }
    }
}
