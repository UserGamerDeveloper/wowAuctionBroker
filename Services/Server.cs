using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public class Message
    {
        public string MessageStr { get; set; }
        public string ColorName { get; set; }
    }
    public class ResponseFaction{
        public string Name { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public bool PlayMusic { get; set; }
    }
    public class Response {
        public string Head { get; set; }
        public List<ResponseFaction> Factions { get; set; } = new List<ResponseFaction>();

        public override string ToString()
        {
            string str = Head;
            foreach (var faction in Factions)
            {
                str += $"{faction.Name}\n";
                foreach (var message in faction.Messages)
                {
                    str += message.MessageStr;
                    str += $"DisplayPriority { message.ColorName }\n";
                }
                str += $"PlayMusic {faction.PlayMusic}\n";
            }
            return str;
        }
    }
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
        public bool farmMode;

        public Faction(FactionModel fraction)
        {
            id = fraction.Id;
            //factionType = Enum.Parse<FactionType>(fraction.FactionType);
            factionType = fraction.FactionType;
            moneyMax = fraction.MoneyMax;
            this.farmMode = fraction.FarmMode;
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
        public int id;
        public int connectedRealmId;
        public string Name { get; set; }
        public DateTime timeUpdate;
        public Dictionary<FactionType, Faction> factions = new Dictionary<FactionType, Faction>();
        public static long TokenPrice;

        public Server(RealmModel realm, Dictionary<int, RecipeData> recipeDataById)
        {
            this.id = realm.Id;
            this.connectedRealmId = realm.ConnectedRealmId;
            Name = realm.Name;
            this.timeUpdate = realm.TimeUpdate;
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
                        Response response = new Response
                        {
                            Head = string.Format("{3}{2}{3}{0} {1}{3}", Name, timeUpdate, DateTime.Now, newLine)
                        };
                        foreach (var faction in auctionData.Factions)
                        {
                            if (faction.RecipesCount > 0)
                            {
                                response.Factions.Add(GetInfoFaction(faction, newLine, tabulate));
                            }
                        }
                        if (response.Factions.Count > 0)
                        {
                            ParseService.SendAndLog(response);
                        }
                        else
                        {
                            ParseService.Log(response);
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
        ResponseFaction GetInfoFaction(FactionPage factionPage, string newLine, string tabulate)
        {
            var s = factionPage.FactionType;
            ResponseFaction faction = new ResponseFaction { 
                Name = string.Format(
                    "{1}{2} {3:0.} + {4:0.} = {5:0.}",
                    newLine,
                    tabulate,
                    factionPage.FactionType,
                    ParseService.ConvertCopperToGold(factions[s].Money),
                    ParseService.ConvertCopperToGold(factions[s].moneyMax - factions[s].Money),
                    ParseService.ConvertCopperToGold(factions[s].moneyMax)),
                PlayMusic = factionPage.PlayMusic
            };
            foreach (var target in factionPage.Targets.OrderBy(x => x.PriorityDisplay))
            {
                Message message = new Message
                {
                    MessageStr = target.ToString(newLine, tabulate),
                    ColorName = target.GetColor()
                };
                faction.Messages.Add(message);
            }
            return faction;
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
        public double ScallingTargetProfitFromRemainingPersentUntilToken(double moneyMax)
        {
            return ParseService.settings.TARGET_PROFIT + (ParseService.settings.TARGET_PROFIT * (((double)moneyMax / TokenPrice) - 1d));
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