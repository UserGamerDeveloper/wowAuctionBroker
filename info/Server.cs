using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public int id;
        public string name;
        public DateTime timeUpdate;
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
            timeUpdate = DateTime.Parse("Sat, 18 Aug 2018 07:22:16 GMT");
        }

        public void Parse()
        {
            try
            {
                //Server server = obj as Server;
                while (true)
                {
                    DateTime timeNextUpdate = timeUpdate.AddHours(1d);
                    if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
                    {
                        if (HasUpdate())
                        {
                            AuctionParser auctionData = new AuctionParser(this);
                            string printStr = GetNameAndTimeUpdate();
                            foreach (var keyValuePair in auctionData.recipesById.OrderByDescending(pair => pair.Value.AverageIncome))
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
            string timeStr = Util.GetTimeUpdateStr(id);
            timeUpdate = DateTime.Parse(timeStr);
        }

        public bool HasUpdate()
        {
            DateTime oldTime = timeUpdate;
            RefreshTimeUpdate();
            return timeUpdate != oldTime;
        }

        public string GetNameAndTimeUpdate()
        {
            return string.Format("{2}\n{0} {1} {3}\n", name, timeUpdate, DateTime.Now, farmMode.ToString());
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
            return string.Format("{0,-20}{1,20}{2,20}",
                name,
                Util.ConvertCopperToGold(Money).ToString("N0"),
                Util.ConvertCopperToGold(waitingMoney).ToString("N0"));
        }
    }
}