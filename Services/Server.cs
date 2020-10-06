using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Xml.Serialization;
using wowCalc;

namespace info
{
    public class CharacterData
    {
        public class Character
        {
            //[JsonProperty("key")]
            //public Key Key { get; set; }
            //[JsonProperty("name")]
            //public string Name { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            [JsonProperty("realm")]
            public Realm Realm { get; set; }
        }
        public class Realm
        {
            //[JsonProperty("key")]
            //public Key Key { get; set; }
            //[JsonProperty("name")]
            //public string Name { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            [JsonProperty("slug")]
            public string Slug { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //    [JsonProperty("user")]
        //    public User User { get; set; }
        //    [JsonProperty("profile")]
        //    public Profile Profile { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class User
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Profile
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Key
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class ProtectedStats
        //{
        //    [JsonProperty("total_number_deaths")]
        //    public int TotalNumberDeaths { get; set; }
        //    [JsonProperty("total_gold_gained")]
        //    public long TotalGoldGained { get; set; }
        //    [JsonProperty("total_gold_lost")]
        //    public long TotalGoldLost { get; set; }
        //    [JsonProperty("total_item_value_gained")]
        //    public int TotalItemValueGained { get; set; }
        //    [JsonProperty("level_number_deaths")]
        //    public int LevelNumberDeaths { get; set; }
        //    [JsonProperty("level_gold_gained")]
        //    public long LevelGoldGained { get; set; }
        //    [JsonProperty("level_gold_lost")]
        //    public long LevelGoldLost { get; set; }
        //    [JsonProperty("level_item_value_gained")]
        //    public int LevelItemValueGained { get; set; }
        //}
        //public class Position
        //{
        //    [JsonProperty("zone")]
        //    public Zone Zone { get; set; }
        //    [JsonProperty("map")]
        //    public Map Map { get; set; }
        //    [JsonProperty("x")]
        //    public double X { get; set; }
        //    [JsonProperty("y")]
        //    public double Y { get; set; }
        //    [JsonProperty("z")]
        //    public double Z { get; set; }
        //    [JsonProperty("facing")]
        //    public double Facing { get; set; }
        //}
        //public class Zone
        //{
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //}
        //public class Map
        //{
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //}
        //public class BindPosition
        //{
        //    [JsonProperty("zone")]
        //    public Zone Zone { get; set; }
        //    [JsonProperty("map")]
        //    public Map Map { get; set; }
        //    [JsonProperty("x")]
        //    public double X { get; set; }
        //    [JsonProperty("y")]
        //    public double Y { get; set; }
        //    [JsonProperty("z")]
        //    public double Z { get; set; }
        //    [JsonProperty("facing")]
        //    public double Facing { get; set; }
        //}

        [JsonProperty("money")]
        public int Money { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("character")]
        public Character Characterr { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("id")]
        //public int Id { get; set; }
        //[JsonProperty("protected_stats")]
        //public ProtectedStats ProtectedStats { get; set; }
        //[JsonProperty("position")]
        //public Position Position { get; set; }
        //[JsonProperty("bind_position")]
        //public BindPosition BindPosition { get; set; }
        //[JsonProperty("wow_account")]
        //public int WowAccount { get; set; }
    }

    public class TokenPriceData
    {
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}

        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("last_updated_timestamp")]
        //public long LastUpdatedTimestamp { get; set; }
        [JsonProperty("price")]
        public long Price { get; set; }
    }

    public class ReputationsData
    {
        public class ReputationData
        {
            public class Faction
            {
                //public class Key
                //{
                //    [JsonProperty("href")]
                //    public string Href { get; set; }
                //}

                //[JsonProperty("key")]
                //public Key Key { get; set; }
                //[JsonProperty("name")]
                //public string Name { get; set; }
                [JsonProperty("id")]
                public int Id { get; set; }
            }
            public class Standing
            {
                //[JsonProperty("raw")]
                //public int Raw { get; set; }
                //[JsonProperty("value")]
                //public int Value { get; set; }
                //[JsonProperty("max")]
                //public int Max { get; set; }
                [JsonProperty("tier")]
                public ReputationTier Tier { get; set; }
                //[JsonProperty("name")]
                //public string Name { get; set; }
            }

            [JsonProperty("faction")]
            public Faction Factionn { get; set; }
            [JsonProperty("standing")]
            public Standing Standingg { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class Character
        //{
        //    [JsonProperty("key")]
        //    public Key Key { get; set; }
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //    [JsonProperty("realm")]
        //    public Realm Realm { get; set; }
        //}
        //public class Realm
        //{
        //    [JsonProperty("key")]
        //    public Key Key { get; set; }
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    [JsonProperty("id")]
        //    public int Id { get; set; }
        //    [JsonProperty("slug")]
        //    public string Slug { get; set; }
        //}

        [JsonProperty("reputations")]
        public List<ReputationData> Reputations { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("character")]
        //public Character Character { get; set; }
    }

    public enum RealmId
    {
        Twisting_Nether = 625,
        Ravencrest = 554,
        Blackmoore = 580,
        Antonidas = 564,
        malganis = 590,
        svezewatel = 1604,
        gorduni = 1602,
        Kazzak = 1305,
        Azjol_Nerub = 503,
        Silvermoon = 549,
        Tyrande = 1384,
        Hyjal = 542,
        Howling_Fjord = 1615
    }

    [Serializable]
    public class Server
    {
        const string TokenPriceURL = "https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale=en_US";
        const string CharacterDataURLFormat = "https://eu.api.blizzard.com/profile/user/wow/protected-character/{0}-{1}?namespace=profile-eu&locale=en_US";
        const string ReputationsDataURLFormat = "https://eu.api.blizzard.com/profile/wow/character/{0}/{1}/reputations?namespace=profile-eu&locale=en_US";

        public int id;
        public int connectedRealmId;
        public string Name { get; set; }
        public long characterId;
        public DateTime timeUpdate;
        public List<int> idRecipes;
        public bool farmMode;
        public long moneyMax;
        [XmlIgnore]
        public Dictionary<string, HashSet<RecipeData>> RecipeDataTrees { get; } = new Dictionary<string, HashSet<RecipeData>>();
        [XmlIgnore]
        public long Money { get; private set; }
        [XmlIgnore]
        private ReputationTier reputationTier;
        [XmlIgnore]
        public static long TokenPrice;

        public Server() { }

        public Server(ConnectedRealmId connectedRealmId, List<int> idRecipes, long characterId, RealmId realmId)
        {
            this.connectedRealmId = (int)connectedRealmId;
            this.idRecipes = idRecipes;
            this.Name = connectedRealmId.ToString();
            farmMode = true;
            timeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT");
            this.characterId = characterId;
            id = (int)realmId;
        }

        public void Parse()
        {
            try
            {
                while (true)
                {
                    DateTime timeNextUpdate = timeUpdate.AddHours(1d);
                    if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
                    {
                        if (HasUpdate())
                        {
                            UpdateData();
                            AuctionParser auctionData = new AuctionParser(this);
                            //string newLine = Environment.NewLine;
                            string newLine = "<br>";
                            string tabulate = "&#9;";
                            string printStr = string.Format("{3}{2}{3}{0} {1}{3}", Name, timeUpdate, DateTime.Now, newLine);
                            foreach (var recipesPage in auctionData.RecipesPages.OrderByDescending(pair => pair.IncomeGoldInHour))
                            {
                                List<Recipe> recipes = recipesPage.Recipes;
                                RecipeData recipeData = recipesPage.recipeData;
                                printStr += string.Format(
                                    "{5} {0,-50} Профит: {6:0.} ({1:0.} + {3:0.}) {2:0.}{4}",
                                    string.Format("{0} x {1}", recipeData.Name, recipes.Count),
                                    ParseService.ConvertCopperToGold(recipesPage.SummaryProfit),
                                    recipesPage.IncomeGoldInHour,
                                    ParseService.ConvertCopperToGold(recipesPage.randomProfit),
                                    newLine,
                                    tabulate,
                                    ParseService.ConvertCopperToGold(recipesPage.SummaryProfit + recipesPage.randomProfit));
                                foreach (var itemData in recipeData.ItemsData)
                                {
                                    printStr += string.Format(
                                        "{3}{3}{0}{2}{3}{3}{3}Макс цена: {3}{1:# ## ##.}{2}",
                                        itemData.itemName,
                                        ParseService.ConvertCopperToSilver(recipesPage.GetMaxPrice(itemData)),
                                        newLine,
                                        tabulate);
                                }
                            }
                            const string STRING = "Профит ";
                            string globalProfitString = string.Format("{6:0.} ({0:0.} + {4:0.}) {1:0.} {2:0.} мин, рецептов {3}{5}",
                                ParseService.ConvertCopperToGold(auctionData.globalProfit),
                                ParseService.GetIncomeGoldInHour(auctionData.globalProfit + auctionData.globalRandomProfit, auctionData.timeCraft),
                                auctionData.timeCraft.TotalMinutes,
                                auctionData.recipesCount,
                                ParseService.ConvertCopperToGold(auctionData.globalRandomProfit),
                                newLine,
                                ParseService.ConvertCopperToGold(auctionData.globalProfit + auctionData.globalRandomProfit));
                            lock (ParseService.consoleLocker)
                            {
                                //ParseService.SendAndLog(printStr);
                                object alertId = null;
                                if (auctionData.globalProfit > 0)
                                {
                                    if (ParseService.ConvertCopperToGold(auctionData.globalProfit + auctionData.globalRandomProfit) >=
                                        ScallingValueFromRemainingPersentUntilToken(ParseService.settings.TARGET_PROFIT))
                                    {
                                        alertId = 1;
                                    }
                                    else
                                    {
                                        alertId = 0;
                                    }
                                }
                                //else
                                //{
                                //    File.AppendAllText("log.txt", STRING + globalProfitString);
                                //}
                                ParseService.SendAndLog(printStr + STRING + globalProfitString, alertId);
                            }
                            using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", Name), FileMode.Create))
                            {
                                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                                serverXmlSerializer.Serialize(fs, this);
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
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
                ParseService.ExceptionLogAndAlert(e);
            }
        }

        internal async static void RefreshTokenPrice()
        {
            string TokenPriceDataStr = await ParseService.GetResponseStringAsync(TokenPriceURL);
            TokenPrice = JsonConvert.DeserializeObject<TokenPriceData>(TokenPriceDataStr).Price;
        }

        private async void UpdateData()
        {
            string CharacterDataStr = await ParseService.GetResponseStringAsync(string.Format(CharacterDataURLFormat, id, characterId));
            CharacterData characterData = JsonConvert.DeserializeObject<CharacterData>(CharacterDataStr);
            Money = characterData.Money;
            if (Money > moneyMax)
            {
                moneyMax = Money;
            }

            string ReputationsDataStr = await ParseService.GetResponseStringAsync(
                string.Format(ReputationsDataURLFormat, characterData.Characterr.Realm.Slug, characterData.Name.ToLower()));
            ReputationsData reputationsData = JsonConvert.DeserializeObject<ReputationsData>(ReputationsDataStr);

            reputationTier = reputationsData.Reputations.FindAll(rep => rep.Factionn.Id == 2103 || rep.Factionn.Id == 2160)[0].Standingg.Tier;
        }

        public float GetSpendingRate()
        {
            float discount = 0f;
            switch (reputationTier)
            {
                case ReputationTier.Neutral:
                    {
                        break;
                    }
                case ReputationTier.Friendly:
                    {
                        discount = 0.05f;
                        break;
                    }
                case ReputationTier.Honored:
                    {
                        discount = 0.1f;
                        break;
                    }
                case ReputationTier.Revered:
                    {
                        discount = 0.15f;
                        break;
                    }
                default:
                    throw new Exception("неизвестная репутация");
            }

            return 1f - discount;
        }

        private void RefreshTimeUpdate()
        {
            string timeStr = ParseService.GetTimeUpdateStr(connectedRealmId);
            timeUpdate = DateTime.Parse(timeStr);
        }

        public bool HasUpdate()
        {
            DateTime oldTime = timeUpdate;
            RefreshTimeUpdate();
            return timeUpdate != oldTime;
        }

        internal void SetData(Dictionary<int, RecipeData> recipeDataByIdRecipe)
        {
            List<RecipeData> recipes = new List<RecipeData>();
            foreach (var idRecipe in idRecipes)
            {
                recipes.Add(recipeDataByIdRecipe[idRecipe]);
            }
            UpdateData();
            List<RecipeData> serverRecipesList = new List<RecipeData>(recipes);
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
                recipeDataList.Remove(recipes[0]);
                while (recipeDataList.Count > 0)
                {
                    RecipeData recipeData = recipeDataList[0];
                    recipeDataList.Remove(recipeData);

                    foreach (var idItem in recipeData.ID_ITEM_AND_NEED_AMOUNT.Keys)
                    {
                        List<RecipeData> recipeDataListNotInTree = new List<RecipeData>(recipes);
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
                string name = "";
                foreach (var item in recipeDataTree)
                {
                    serverRecipesList.Remove(item);
                    name += item.Name + "    ";
                }
                RecipeDataTrees.Add(name, recipeDataTree);
            }
        }

        public double ScallingValueFromRemainingPersentUntilToken(double value)
        {
            return value + (value * (((double)moneyMax / TokenPrice) - 1d));
        }

        internal double GetTargetIncomeGoldInHour()
        {
            if (farmMode)
            {
                return 0d;
            }
            else
            {
                return ParseService.settings.TARGET_INCOME_IN_HOUR;
            }
        }
    }
}