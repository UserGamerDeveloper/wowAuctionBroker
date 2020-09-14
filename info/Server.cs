using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Xml.Serialization;

namespace info
{
    public class RealmData
    {
        public long MoneyMax { get; set; }
        public long Money { get; set; }
        public Reputation Reputation { get; set; }
    }

    public class ClientData
    {
        public Dictionary<int, RealmData> RealmsDatasByIdHouse { get; set; }
        public long TokenPrice { get; set; }
    }

    [Serializable]
    public class Server
    {
        private class House
        {
            [JsonProperty("timestamps")]
            public Timestamps Timestamps { get; set; }
            [JsonProperty("mostAvailable")]
            public List<MostAvailable> MostAvailable { get; set; }
            [JsonProperty("deals")]
            public List<Deal> Deals { get; set; }
        }
        private class Timestamps
        {
            [JsonProperty("scheduled")]
            public int Scheduled { get; set; }
            [JsonProperty("delayednext")]
            public object Delayednext { get; set; }
            [JsonProperty("lastupdate")]
            public int Lastupdate { get; set; }
            [JsonProperty("mindelta", NullValueHandling = NullValueHandling.Ignore)]
            public int Mindelta { get; set; }
            [JsonProperty("avgdelta", NullValueHandling = NullValueHandling.Ignore)]
            public int Avgdelta { get; set; }
            [JsonProperty("maxdelta", NullValueHandling = NullValueHandling.Ignore)]
            public int Maxdelta { get; set; }
            [JsonProperty("lastcheck")]
            public int Lastcheck { get; set; }
            [JsonProperty("lastsuccess")]
            public int Lastsuccess { get; set; }
        }
        private class MostAvailable
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("requiredside")]
            public string Requiredside { get; set; }
            [JsonProperty("name_enus")]
            public string NameEnus { get; set; }
            [JsonProperty("name_dede")]
            public string NameDede { get; set; }
            [JsonProperty("name_eses")]
            public string NameEses { get; set; }
            [JsonProperty("name_frfr")]
            public string NameFrfr { get; set; }
            [JsonProperty("name_itit")]
            public string NameItit { get; set; }
            [JsonProperty("name_ptbr")]
            public string NamePtbr { get; set; }
            [JsonProperty("name_ruru")]
            public string NameRuru { get; set; }
            [JsonProperty("name_zhtw")]
            public string NameZhtw { get; set; }
            [JsonProperty("name_kokr")]
            public string NameKokr { get; set; }
        }
        private class Deal
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("requiredside")]
            public string Requiredside { get; set; }
            [JsonProperty("name_enus")]
            public string NameEnus { get; set; }
            [JsonProperty("name_dede")]
            public string NameDede { get; set; }
            [JsonProperty("name_eses")]
            public string NameEses { get; set; }
            [JsonProperty("name_frfr")]
            public string NameFrfr { get; set; }
            [JsonProperty("name_itit")]
            public string NameItit { get; set; }
            [JsonProperty("name_ptbr")]
            public string NamePtbr { get; set; }
            [JsonProperty("name_ruru")]
            public string NameRuru { get; set; }
            [JsonProperty("name_zhtw")]
            public string NameZhtw { get; set; }
            [JsonProperty("name_kokr")]
            public string NameKokr { get; set; }
        }

        private class ComparerByTime : IComparer<Server>
        {
            public int Compare(Server s1, Server s2)
            {
                return s1.timeUpdate.CompareTo(s2.timeUpdate);
            }
        }
        private class ComparerDescendingByMoney : IComparer<Server>
        {
            public int Compare(Server s1, Server s2)
            {
                return s2.Money.CompareTo(s1.Money);
            }
        }

        const string URI_FORMAT = "https://theunderminejournal.com/api/house.php?house={0}";

        public int id;
        public string name;
        public long timeUpdate;
        public List<int> idRecipes;
        public bool farmMode;
        [XmlIgnore]
        public List<HashSet<RecipeData>> RecipeDataTrees { get; } = new List<HashSet<RecipeData>>();
        [XmlIgnore]
        public long Money { get; private set; }
        [XmlIgnore]
        private long moneyMax;
        [XmlIgnore]
        private Reputation reputation;

        public Server() { }

        public Server(HouseId serverId, List<int> idRecipes)
        {
            this.id = (int)serverId;
            this.idRecipes = idRecipes;
            this.name = serverId.ToString();
            farmMode = false;
        }

        public void Parse()
        {
            try
            {
                //Server server = obj as Server;
                while (true)
                {
                    DateTime dateTime = Util.UnixTimeStampToDateTime(timeUpdate);
                    DateTime timeNextUpdate = dateTime.AddHours(1d).AddMinutes(Util.AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA);
                    if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
                    {
                        if (HasUpdate())
                        {
                            AuctionData auctionData = new AuctionData(this);
                            string printStr = GetNameAndTimeUpdate();
                            foreach (var keyValuePair in auctionData.recipesById)
                            {
                                RecipesPage recipesPage = keyValuePair.Value;
                                List<Recipe> recipes = recipesPage.Recipes;
                                RecipeData recipeData = recipesPage.recipeData;
                                printStr += string.Format(
                                    "\n\t {0,-40} Профит: {1:0.} + {3:0.} {2:0.}\n",
                                    string.Format("{0} x {1}", recipeData.name, recipes.Count),
                                    Util.ConvertCopperToGold(recipesPage.SummaryProfit),
                                    recipesPage.AverageIncome,
                                    Util.ConvertCopperToGold(recipesPage.randomProfit));
                                foreach (var itemData in recipeData.ItemsData)
                                {
                                    printStr += string.Format(
                                        "\t\t{0}\n\t\t\tМакс цена: \t{1:# ## ##.}\n",
                                        itemData.itemName,
                                        Util.ConvertCopperToSilver(recipesPage.GetMaxPrice(itemData)));
                                }
                            }
                            const string STRING = "Профит ";
                            string globalProfitString = string.Format("{0:0.} + {4:0.} {1:0.} {2:0.} мин, рецептов {3}",
                                Util.ConvertCopperToGold(auctionData.globalProfit),
                                Util.GetIncomeGoldInHour(auctionData.globalProfit, auctionData.timeCraft),
                                auctionData.timeCraft.TotalMinutes,
                                auctionData.recipesCount,
                                Util.ConvertCopperToGold(auctionData.globalRandomProfit));
                            lock (Program.consoleLocker)
                            {
                                Util.WriteLineAndLog(printStr);
                                if (Util.ConvertCopperToGold(auctionData.globalProfit + auctionData.globalRandomProfit) > Program.settings.TARGET_PROFIT)
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
                                    if (auctionData.globalProfit > 0)
                                    {
                                        Console.WriteLine(STRING + globalProfitString);
                                        SoundPlayer alert = new SoundPlayer("alert.wav");
                                        alert.Play();
                                    }
                                }
                                File.AppendAllText("log.txt", STRING + globalProfitString + "\n\n");
                                Console.WriteLine();
                            }
                            using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", name), FileMode.Create))
                            {
                                XmlSerializer serverXmlSerializer = new XmlSerializer(typeof(Server));
                                serverXmlSerializer.Serialize(fs, this);
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
                Util.ExceptionLogAndAlert(e);
            }
        }

        public float GetSpendingRate()
        {
            float discount = 0f;
            switch (reputation)
            {
                case Reputation.Neutral:
                    {
                        break;
                    }
                case Reputation.Frenly:
                    {
                        discount = 0.05f;
                        break;
                    }
                case Reputation.Honored:
                    {
                        discount = 0.1f;
                        break;
                    }
                case Reputation.Resived:
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
            House house = JsonConvert.DeserializeObject<House>(Util.GetResponse(string.Format(URI_FORMAT, id), "Exception_house.txt"));

            DateTime time = Util.UnixTimeStampToDateTime(house.Timestamps.Lastupdate);

            if (time.AddMinutes(Util.AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA).CompareTo(DateTime.Now) == -1)
            {
                timeUpdate = house.Timestamps.Lastupdate;
            }
        }

        public bool HasUpdate()
        {
            long oldTime = timeUpdate;
            RefreshTimeUpdate();
            return timeUpdate != oldTime;
        }

        public string GetNameAndTimeUpdate()
        {
            return string.Format("{2}\n{0} {1} {3}\n", name, Util.UnixTimeStampToDateTime(timeUpdate), DateTime.Now, farmMode.ToString());
        }

        internal static void SortByTime(List<Server> servers)
        {
            servers.Sort(new ComparerByTime());
        }

        internal static void SortByDescendingMoney(List<Server> servers)
        {
            servers.Sort(new ComparerDescendingByMoney());
        }

        internal void SetData(RealmData realmData, Dictionary<int, RecipeData> recipeDataByIdRecipe)
        {
            List<RecipeData> recipes = new List<RecipeData>();
            foreach (var idRecipe in idRecipes)
            {
                recipes.Add(recipeDataByIdRecipe[idRecipe]);
            }

            Money = realmData.Money;
            reputation = realmData.Reputation;
            moneyMax = realmData.MoneyMax;

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
                RecipeDataTrees.Add(recipeDataTree);
            }
        }

        internal string GetInfo()
        {
            long waitingMoney = moneyMax - Money;
            return String.Format("{0,-20}{1,20}{2,20}",
                name,
                Util.ConvertCopperToGold(Money).ToString("N0"),
                Util.ConvertCopperToGold(waitingMoney).ToString("N0"));
        }
    }
}