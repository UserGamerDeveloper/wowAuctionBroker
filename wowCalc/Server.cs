using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using wowCalc;

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
        public Dictionary<string, HashSet<RecipeData>> RecipeDataTrees { get; } = new Dictionary<string, HashSet<RecipeData>>();
        [XmlIgnore]
        public long Money { get; private set; }
        [XmlIgnore]
        public long moneyMax;
        [XmlIgnore]
        private Reputation reputation;
        [XmlIgnore]
        public static long TokenPrice;

        public Server() { }

        public Server(RealmID serverId, List<int> idRecipes)
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
                while (true)
                {
                    DateTime timeNextUpdate = timeUpdate.AddHours(1d);
                    if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
                    {
                        if (HasUpdate())
                        {
                            AuctionParser auctionData = new AuctionParser(this);
                            string newLine = Environment.NewLine;
                            string printStr = string.Format("{4}{2}{4}{0} {1} {3}{4}", name, timeUpdate, DateTime.Now, farmMode.ToString(), newLine);
                            foreach (var keyValuePair in auctionData.recipesById.OrderByDescending(pair => pair.Value.AverageIncome))
                            {
                                RecipesPage recipesPage = keyValuePair.Value;
                                List<Recipe> recipes = recipesPage.Recipes;
                                RecipeData recipeData = recipesPage.recipeData;
                                printStr += string.Format(
                                    "\t {0,-80} Профит: {1:0.} + {3:0.} {2:0.}{4}",
                                    string.Format("{0} x {1}", recipeData.name, recipes.Count),
                                    Util.ConvertCopperToGold(recipesPage.SummaryProfit),
                                    recipesPage.AverageIncome,
                                    Util.ConvertCopperToGold(recipesPage.randomProfit),
                                    newLine);
                                foreach (var itemData in recipeData.ItemsData)
                                {
                                    printStr += string.Format(
                                        "\t\t{0}{2}\t\t\tМакс цена: \t{1:# ## ##.}{2}",
                                        itemData.itemName,
                                        Util.ConvertCopperToSilver(recipesPage.GetMaxPrice(itemData)),
                                        newLine);
                                }
                            }
                            const string STRING = "Профит ";
                            string globalProfitString = string.Format("{0:0.} + {4:0.} {1:0.} {2:0.} мин, рецептов {3}{5}",
                                Util.ConvertCopperToGold(auctionData.globalProfit),
                                Util.GetIncomeGoldInHour(auctionData.globalProfit, auctionData.timeCraft),
                                auctionData.timeCraft.TotalMinutes,
                                auctionData.recipesCount,
                                Util.ConvertCopperToGold(auctionData.globalRandomProfit),
                                newLine);
                            lock (Program.consoleLocker)
                            {
                                Util.WriteAndLog(printStr);
                                if (auctionData.globalProfit > 0)
                                {
                                    Util.WriteAndLog(STRING + globalProfitString);
                                    if (Util.ConvertCopperToGold(auctionData.globalProfit + auctionData.globalRandomProfit) >
                                        ScallingValueFromRemainingPersentUntilToken(Program.settings.TARGET_PROFIT))
                                    {
                                        using (var audioFile = new AudioFileReader("music.aac"))
                                        using (var outputDevice = new WaveOutEvent())
                                        {
                                            outputDevice.Init(audioFile);
                                            outputDevice.Play();
                                            DialogResult result = MessageBox.Show(
                                                "Есть профит",
                                                "Сообщение",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information,
                                                MessageBoxDefaultButton.Button1,
                                                MessageBoxOptions.DefaultDesktopOnly);
                                            outputDevice.Stop();
                                        }
                                    }
                                    else
                                    {
                                        SoundPlayer alert = new SoundPlayer("alert.wav");
                                        alert.Play();
                                    }
                                }
                                else
                                {
                                    File.AppendAllText("log.txt", STRING + globalProfitString);
                                }
                            }
                            using (FileStream fs = new FileStream(string.Format(@"realms\{0}.xml", name), FileMode.Create))
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
                string name = "";
                foreach (var item in recipeDataTree)
                {
                    serverRecipesList.Remove(item);
                    name += item.name + "    ";
                }
                RecipeDataTrees.Add(name, recipeDataTree);
            }
        }

        public double ScallingValueFromRemainingPersentUntilToken(double value)
        {
            return value + (value * (((double)moneyMax / TokenPrice) - 1d));
        }
    }
}