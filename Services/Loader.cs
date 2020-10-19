using Microsoft.Extensions.Hosting;
using Mvc.Client.Data;
using Mvc.Client.Models;
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
        public static Settings DeserializeSettings()
        {
            using (FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                return (Settings)xmlSerializer.Deserialize(fs);
            }
        }
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
                    389706,
                    BFANeedMillisecondsToCraft,
                    0,
                    389706,
                    true),
                new RecipeData(
                    RecipeInfo.Shimmerscale_Striker_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shimmerscale , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    398924,
                    BFANeedMillisecondsToCraft,
                    0,
                    398924,
                    true),
                new RecipeData(
                    RecipeInfo.Silkweave_Slippers,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Shaldorei_Silk , 12}
                    },
                    139747,
                    DefaultNeedMillisecondsToCraft,
                    5000,
                    139747),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    276913,
                    BFANeedMillisecondsToCraft,
                    60000,
                    276913,
                    true),
                new RecipeData(
                    RecipeInfo.Tidespray_Linen_Pants_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Tidespray_Linen , 17}
                    },
                    271119,
                    BFANeedMillisecondsToCraft,
                    60000,
                    271119,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_H,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    391157,
                    BFANeedMillisecondsToCraft,
                    0,
                    391157,
                    true),
                new RecipeData(
                    RecipeInfo.Coarse_Leather_Cestus_A,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.CoarseLeather , 10}, {(int)ItemInfo.BloodStainedBone , 8}
                    },
                    400336,
                    BFANeedMillisecondsToCraft,
                    0,
                    400336,
                    true),
                new RecipeData(
                    RecipeInfo.Battlebound_Spaulders,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stormscale , 12}
                    },
                    133656,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    133656),
                new RecipeData(
                    RecipeInfo.Warhide_Shoulderguard,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Stonehide_Leather , 12}
                    },
                    135700,
                    DefaultNeedMillisecondsToCraft,
                    0,
                    135700),
                new RecipeData(
                    RecipeInfo.Crafted_Dreadful_Gladiators_Cloak_of_Prowess,
                    new XmlSerializableDictionary<int, int>(){
                        {(int)ItemInfo.Windwool_Cloth , 20}
                    },
                    0,
                    6000,
                    0,
                    0)
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

        public static Dictionary<string, Server> DeserializeServers(IHostEnvironment hostingEnvironment)
        {
            Dictionary<string, Server> serversByName = new Dictionary<string, Server>();
            Server.RefreshTokenPrice();
            Dictionary<int, RecipeData> recipeDataById = GetRecipeDataById();
            if (hostingEnvironment.IsDevelopment())
            {
                foreach (var server in Saver.GetServers())
                {
                    server.SetData(recipeDataById);
                    serversByName.Add(server.Name, server);
                }
            }
            else
            {
                using (var db = new DatabaseContext())
                {
                    foreach (var file in new DirectoryInfo("realms").GetFiles())
                    {
                        using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                        {
                            XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                            Server server = (Server)serverXmlSerializer.Deserialize(fs);
                            var d = new List<ActiveRecipe>();
                            foreach (var idRecipe in server.idRecipes)
                            {
                                d.Add(new ActiveRecipe { IdRecipe = idRecipe });
                            }
                            db.Add(new RealmModel
                            {
                                Id = server.id,
                                ConnectedRealmId = server.connectedRealmId,
                                CharacterId = server.characterId,
                                Name = server.Name,
                                TimeUpdate = server.timeUpdate,
                                ActiveRecipes = d,
                                FarmMode = server.farmMode,
                                MoneyMax = server.moneyMax
                            });
                        }
                        file.Delete();
                    }
                    db.SaveChanges();

                    var realms = db.Realms;
                    foreach (var realm in realms)
                    {
                        var server = new Server(realm);
                        server.SetData(recipeDataById);
                        serversByName.Add(server.Name, server);
                    }
                }
            }
            return serversByName;
        }

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
