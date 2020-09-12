using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                return s2.money.CompareTo(s1.money);
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
        private long money;
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

            money = realmData.Money;
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
            long waitingMoney = moneyMax - money;
            return String.Format("{0,-20}{1,20}{2,20}",
                name,
                Util.ConvertCopperToGold(money).ToString("N0"),
                Util.ConvertCopperToGold(waitingMoney).ToString("N0"));
        }
    }
}