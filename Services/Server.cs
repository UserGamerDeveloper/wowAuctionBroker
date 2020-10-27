using Mvc.Client.Data;
using Mvc.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using wowCalc;

namespace info
{
    public class Faction
    {
        public int id;
        public FactionType factionType;
        public long moneyMax;
        [XmlIgnore]
        public long Money { get; set; } = 0;
        [XmlIgnore]
        public ReputationTier reputationTier = ReputationTier.Neutral;
        [XmlIgnore]
        public Dictionary<string, HashSet<RecipeData>> RecipeDataTrees { get; } = new Dictionary<string, HashSet<RecipeData>>();

        public Faction(FactionModel fraction)
        {
            id = fraction.Id;
            //factionType = Enum.Parse<FactionType>(fraction.FactionType);
            factionType = fraction.FactionType;
            moneyMax = fraction.MoneyMax;
        }
        internal void SetMoneyMax()
        {
            if (Money > moneyMax)
            {
                moneyMax = Money;
            }
        }
        internal void SetRecipeDataTrees(List<RecipeData> recipes)
        {
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
                    throw new Exception($"неизвестная репутация {reputationTier}");
            }

            return 1f - discount;
        }
    }
    public class Server
    {
        private const string Alert = "0";
        private const string Music = "1";
        public int id;
        public int connectedRealmId;
        public string Name { get; set; }
        public DateTime timeUpdate;
        public bool farmMode;
        public Dictionary<FactionType, Faction> factions = new Dictionary<FactionType, Faction>();
        public static long TokenPrice;

        public Server(RealmModel realm, Dictionary<int, RecipeData> recipeDataById)
        {
            this.id = realm.Id;
            this.connectedRealmId = realm.ConnectedRealmId;
            Name = realm.Name;
            this.timeUpdate = realm.TimeUpdate;
            this.farmMode = realm.FarmMode;
            SetFactionData(realm, recipeDataById);
        }
        internal void SetFactionData(RealmModel realm, Dictionary<int, RecipeData> recipeDataById)
        {
            foreach (var fraction in realm.Fractions)
            {
                factions.Add(fraction.FactionType, new Faction(fraction));
            }
            foreach (var character in realm.Characters)
            {
                CharacterProtectedData characterProtectedData = ParseService.GetCharacterProtectedData(realm.Id, character.Id);
                CharacterData characterData = ParseService.GetCharacterData(characterProtectedData.Characterr.Realm.Slug, characterProtectedData.Name);
                Faction faction = factions[characterData.faction.type];
                faction.Money += characterProtectedData.Money;
                ReputationsData reputationsData = ParseService.GetReputationsData(characterProtectedData.Characterr.Realm.Slug, characterProtectedData.Name);
                int factionId = int.MinValue;
                switch (faction.factionType)
                {
                    case FactionType.ALLIANCE:
                        {
                            factionId = 2160;
                            break;
                        }
                    case FactionType.HORDE:
                        {
                            factionId = 2103;
                            break;
                        }
                    default:
                        throw new Exception($"неизвестная фракция {faction.factionType}");
                }
                var reputationData = reputationsData.Reputations.Find(rep => rep.Factionn.Id == factionId);
                if (reputationData != null)
                {
                    ReputationTier reputationTier = reputationData.Standingg.Tier;
                    if (reputationTier > faction.reputationTier)
                    {
                        faction.reputationTier = reputationTier;
                    }
                }
            }
            Dictionary<FactionType, List<RecipeData>> a = new Dictionary<FactionType, List<RecipeData>>();
            foreach (var faction in factions.Values)
            {
                faction.SetMoneyMax();
                a.Add(faction.factionType, new List<RecipeData>());
            }
            foreach (var activeRecipe in realm.ActiveRecipes)
            {
                switch (recipeDataById[activeRecipe.IdRecipe].Faction)
                {
                    case FactionType.ALLIANCE:
                        {
                            a[FactionType.ALLIANCE].Add(recipeDataById[activeRecipe.IdRecipe]);
                            break;
                        }
                    case FactionType.HORDE:
                        {
                            a[FactionType.HORDE].Add(recipeDataById[activeRecipe.IdRecipe]);
                            break;
                        }
                    case FactionType.NONE:
                        {
                            if (factions.Count == 1)
                            {
                                a[factions.Values.First().factionType].Add(recipeDataById[activeRecipe.IdRecipe]);
                            }
                            else
                            {
                                var factionHorde = factions[FactionType.HORDE];
                                var repHorde = factionHorde.reputationTier;
                                var factionAlliance = factions[FactionType.ALLIANCE];
                                var repAlliance = factionAlliance.reputationTier;
                                if (repAlliance > repHorde)
                                {
                                    a[FactionType.ALLIANCE].Add(recipeDataById[activeRecipe.IdRecipe]);
                                }
                                else
                                {
                                    if (repAlliance < repHorde)
                                    {
                                        a[FactionType.HORDE].Add(recipeDataById[activeRecipe.IdRecipe]);
                                    }
                                    else
                                    {
                                        a[FactionType.HORDE].Add(recipeDataById[activeRecipe.IdRecipe]);
                                        a[FactionType.ALLIANCE].Add(recipeDataById[activeRecipe.IdRecipe]);
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        throw new Exception($"неизвестная фракция {recipeDataById[activeRecipe.Id].Faction}");
                }
            }
            foreach (var faction in factions.Values)
            {
                faction.SetRecipeDataTrees(a[faction.factionType]);
            }
        }
        public TimeSpan Parse()
        {
            DateTime timeNextUpdate = timeUpdate.AddHours(1d);
            if (timeNextUpdate.CompareTo(DateTime.Now) == -1)
            {
                while (true)
                {
                    if (HasUpdate())
                    {
                        AuctionParser auctionData = new AuctionParser(this);
                        //string newLine = Environment.NewLine;
                        string newLine = "<br>";
                        string tabulate = "&#9;";
                        string printStr = string.Format("{3}{2}{3}{0} {1}{3}", Name, timeUpdate, DateTime.Now, newLine);
                        List<string> response = new List<string>
                        {
                            printStr
                        };
                        foreach (var faction in auctionData.Factions)
                        {
                            if (faction.RecipesPages.Count > 0)
                            {
                                response.AddRange(GetInfoFaction(faction, newLine, tabulate));
                            }
                        }
                        if (response.Count == 1)
                        {
                            ParseService.Log(response);
                        }
                        else
                        {
                            ParseService.SendAndLog(response);
                        }
                        using (var db = new DatabaseContext())
                        {
                            var realm = db.Realms.Where(x => x.Id == id).First();
                            realm.TimeUpdate = timeUpdate;
                            foreach (var faction in realm.Fractions)
                            {
                                faction.MoneyMax = factions[faction.FactionType].moneyMax;
                            }
                            db.SaveChanges();
                        }
                        return timeUpdate.AddHours(1d).Subtract(DateTime.Now);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                return timeNextUpdate.Subtract(DateTime.Now);
            }

        }
        List<string> GetInfoFaction(FactionPage factionPage, string newLine, string tabulate)
        {
            string printStr = string.Format(
                "{1}{2}{0}",
                newLine,
                tabulate,
                factionPage.Name);
            foreach (var recipesPage in factionPage.RecipesPages.OrderByDescending(pair => pair.IncomeGoldInHour))
            {
                List<Recipe> recipes = recipesPage.Recipes;
                RecipeData recipeData = recipesPage.recipeData;
                printStr += string.Format(
                    "{5} {0,-50} Профит: {6:0.} ({1:0.} + {3:0.}) {2:0.}{4}",
                    string.Format("{0} x {1} = {2:0.}", recipeData.Name, recipes.Count, ParseService.ConvertCopperToGold(recipesPage.CostCraft)),
                    ParseService.ConvertCopperToGold(recipesPage.NormalProfit),
                    recipesPage.IncomeGoldInHour,
                    ParseService.ConvertCopperToGold(recipesPage.randomProfit),
                    newLine,
                    tabulate,
                    ParseService.ConvertCopperToGold(recipesPage.NormalProfit + recipesPage.randomProfit));
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
            string alertId = null;
            if (factionPage.ProfitInTargetIncome + factionPage.ProfitOutTargetIncome > 0)
            {
                if (factionPage.ProfitInTargetIncome > 0)
                {
                    printStr += string.Format("{7}Профит в таргете {6:0.} ({0:0.} + {4:0.}) {1:0.} {2:0.} мин, рецептов {3}{5}",
                        ParseService.ConvertCopperToGold(factionPage.TargetIncomeNormalProfit),
                        ParseService.GetIncomeGoldInHour(
                            factionPage.ProfitInTargetIncome,
                            factionPage.TargetIncomeTimeCraftInMilliseconds),
                        TimeSpan.FromMilliseconds(factionPage.TargetIncomeTimeCraftInMilliseconds).TotalMinutes,
                        factionPage.TargetIncomeRecipesCount,
                        ParseService.ConvertCopperToGold(factionPage.TargetIncomeRandomProfit),
                        newLine,
                        ParseService.ConvertCopperToGold(factionPage.ProfitInTargetIncome),
                        tabulate);
                }
                printStr += string.Format("{7}Профит вне таргета {6:0.} ({0:0.} + {4:0.}) {1:0.} {2:0.} мин, рецептов {3}{5}",
                    ParseService.ConvertCopperToGold(factionPage.NotTargetIncomeNormalProfit),
                    ParseService.GetIncomeGoldInHour(
                        factionPage.ProfitOutTargetIncome,
                        factionPage.NotTargetIncomeTimeCraftInMilliseconds),
                    TimeSpan.FromMilliseconds(factionPage.NotTargetIncomeTimeCraftInMilliseconds).TotalMinutes,
                    factionPage.NotTargetIncomeRecipesCount,
                    ParseService.ConvertCopperToGold(factionPage.NotTargetIncomeRandomProfit),
                    newLine,
                    ParseService.ConvertCopperToGold(factionPage.ProfitOutTargetIncome),
                    tabulate);

                if (ParseService.ConvertCopperToGold(factionPage.ProfitInTargetIncome) >=
                    ScallingValueFromRemainingPersentUntilToken(ParseService.settings.TARGET_PROFIT))
                {
                    alertId = Music;
                }
                else
                {
                    if (ParseService.ConvertCopperToGold(factionPage.ProfitInTargetIncome + factionPage.ProfitOutTargetIncome) >=
                        ScallingValueFromRemainingPersentUntilToken(ParseService.settings.TARGET_PROFIT) && farmMode)
                    {
                        alertId = Music;
                    }
                    else
                    {
                        alertId = Alert;
                    }
                }
            }
            List<string> response = new List<string>
                        {
                            printStr,
                            alertId
                        };
            return response;
        }
        internal static void RefreshTokenPrice()
        {
            TokenPrice = ParseService.GetTokenPrice();
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
        public double ScallingValueFromRemainingPersentUntilToken(double value)
        {
            double moneyMax = 0;
            foreach (var faction in factions.Values)
            {
                moneyMax += faction.moneyMax;
            }
            return value + (value * (((double)moneyMax / TokenPrice) - 1d));
        }

        //internal double GetTargetIncomeGoldInHour()
        //{
        //    if (farmMode)
        //    {
        //        return 0d;
        //    }
        //    else
        //    {
        //        return ParseService.settings.TARGET_INCOME_IN_HOUR;
        //    }
        //}
    }
}