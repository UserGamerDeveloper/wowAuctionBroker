using Mvc.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wowCalc;
using static wowCalc.AuctionData;

namespace info
{
    public class RecipesPage
    {
        public RecipesPage() { }
        public RecipesPage(IEnumerable<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                AddRecipe(recipe);
            }
        }
        public List<Recipe> Recipes { get; } = new List<Recipe>();
        public long NormalProfit { get; private set; } = 0;
        public double IncomeRUBInHour { get; private set; }
        public double TimeCraftInMilliseconds { get; private set; } = 0;
        public double RandomProfit { get; private set; }
        public HashSet<RecipeData> RecipeData { get; private set; } = new HashSet<RecipeData>();
        public long CostCraft { get; private set; } = 0;
        public double Profit { get; set; }

        public void SetData()
        {
            foreach (var recipe in Recipes)
            {
                RecipeData.Add(recipe.RecipeData);
            }
            RandomProfit = Recipes.Count * RecipeData.First().GetRandomProfit();
            IncomeRUBInHour = ParseService.GetIncomeRUBInHour(NormalProfit + RandomProfit, TimeCraftInMilliseconds);
            Profit = NormalProfit + RandomProfit;
        }
        public void AddRecipe(Recipe recipe)
        {
            CostCraft += recipe.CostCraft;
            NormalProfit += recipe.NormalProfit;
            TimeCraftInMilliseconds += recipe.NeedMillisecondsToCraft;
            Recipes.Add(recipe);
        }
        internal string ItemsToString(string newLine, string tabulate)
        {
            Dictionary<int, Dictionary<long, int>> quantityByCostByItemId = new Dictionary<int, Dictionary<long, int>>();
            foreach (var recipe in Recipes)
            {
                foreach (var keyValuePair in recipe.Items)
                {
                    int idItem = keyValuePair.Key;
                    quantityByCostByItemId.TryAdd(idItem, new Dictionary<long, int>());
                    foreach (var item in keyValuePair.Value)
                    {
                        quantityByCostByItemId[idItem].TryAdd(item.cost, 0);
                        quantityByCostByItemId[idItem][item.cost]++;
                    }
                }
            }
            string messageStr = "";
            foreach (var recipeData in RecipeData)
            {
                foreach (var itemData in recipeData.ItemsData)
                {
                    messageStr += string.Format(
                        "{3}{3}{0}{2}{3}{3}{3}Макс цена: {3}{1:# ## ##.}{2}",
                        itemData.itemName,
                        ParseService.ConvertCopperToSilver(GetMaxPrice(quantityByCostByItemId[itemData.id], recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id])),
                        newLine,
                        tabulate);
                }
            }
            return messageStr;

            long GetMaxPrice(Dictionary<long, int> quantityByCost, int needAmountItemsForCraft)
            {
                long maxPrice = 0;
                foreach (var bidsItemInRecipeByCostPair in quantityByCost.OrderByDescending(pair => pair.Key))
                {
                    maxPrice = bidsItemInRecipeByCostPair.Key;
                    if (bidsItemInRecipeByCostPair.Value >= needAmountItemsForCraft)
                    {
                        break;
                    }
                }

                return maxPrice;
            }
        }
    }
    public class FactionPage
    {
        public enum PriorityDisplay
        {
            BuyForEnableMoney,
            BuyForMaxMoney,
            CraftTargetIncome,
            RemoteCraft,
            CraftNotTargetIncome
        }
        public abstract class Target
        {
            public PriorityDisplay PriorityDisplay { get; protected set; }
            public string Name { get; set; }
            public List<RecipesPage> RecipesPages { get; } = new List<RecipesPage>();
            public double Profit { get; set; } = 0;
            public double NormalProfit { get; set; } = 0;
            public double RandomProfit { get; set; } = 0;
            public double TimeCraftInMilliseconds { get; set; } = 0;
            public int RecipesCount { get; set; } = 0;
            public bool IsReached { get; set; }

            protected virtual void SetData(List<RecipesPage> recipesPages)
            {
                foreach (var recipesPage in recipesPages)
                {
                    recipesPage.SetData();
                    SetDataa(recipesPage);
                }
            }

            protected void SetDataa(RecipesPage recipesPage)
            {
                if (ParseService.ConvertCopperToRUB(recipesPage.Profit) >= ParseService.settings.TargetProfitInRUB * 0.1)
                {
                    RecipesPages.Add(recipesPage);
                    NormalProfit += recipesPage.NormalProfit;
                    RandomProfit += recipesPage.RandomProfit;
                    TimeCraftInMilliseconds += recipesPage.TimeCraftInMilliseconds;
                    RecipesCount += recipesPage.Recipes.Count;
                    Profit += recipesPage.Profit;
                }
            }

            internal virtual string GetColor()
            {
                if (IsReached)
                {
                    return "green";
                }
                else
                {
                    return "yellow";
                }
            }
            internal virtual string ToString(string newLine, string tabulate)
            {
                string messageStr = "";
                foreach (var recipesPage in RecipesPages.OrderByDescending(pair => pair.IncomeRUBInHour))
                {
                    List<Recipe> recipes = recipesPage.Recipes;
                    RecipeData recipeData = recipesPage.RecipeData.First();
                    messageStr += string.Format(
                        "{5} {0,-50} Профит: {6:0.} ({1:0.} + {3:0.}) {2:0.}{4}",
                        string.Format("{0} x {1} = {2:0.}", recipeData.Name, recipes.Count, ParseService.ConvertCopperToGold(recipesPage.CostCraft)),
                        ParseService.ConvertCopperToRUB(recipesPage.NormalProfit),
                        recipesPage.IncomeRUBInHour,
                        ParseService.ConvertCopperToRUB(recipesPage.RandomProfit),
                        newLine,
                        tabulate,
                        ParseService.ConvertCopperToRUB(recipesPage.NormalProfit + recipesPage.RandomProfit));
                    messageStr += recipesPage.ItemsToString(newLine, tabulate);
                }
                messageStr += string.Format("{7} {8,-50} Профит: {6:0.} ({0:0.} + {4:0.}) {1:0.} {2:0.} мин{5}",
                    ParseService.ConvertCopperToRUB(NormalProfit),
                    ParseService.GetIncomeRUBInHour(
                        Profit,
                        TimeCraftInMilliseconds),
                    TimeSpan.FromMilliseconds(TimeCraftInMilliseconds).TotalMinutes,
                    RecipesCount,
                    ParseService.ConvertCopperToRUB(RandomProfit),
                    newLine,
                    ParseService.ConvertCopperToRUB(Profit),
                    tabulate,
                    Name);
                return messageStr;
            }
        }
        public class RemoteCraft : Target
        {
            public RemoteCraft(IEnumerable<List<Recipe>> recipesByType, long moneyEnable)
            {
                var recipesPages = new List<RecipesPage>();
                var timeCraftMax = TimeSpan.FromMinutes(30);
                foreach (var recipeList in recipesByType)
                {
                    RecipesPage recipesPage = new RecipesPage();
                    foreach (var recipe in recipeList.OrderByDescending(x => x.GetIncomeGoldInHour()))
                    {
                        if (recipesPage.CostCraft + recipe.CostCraft > moneyEnable ||
                            TimeSpan.FromMilliseconds(recipesPage.TimeCraftInMilliseconds) >= timeCraftMax)
                        {
                            break;
                        }
                        recipesPage.AddRecipe(recipe);
                    }
                    if (recipesPage.Recipes.Count > 0)
                    {
                        recipesPages.Add(recipesPage);
                    }
                }

                PriorityDisplay = PriorityDisplay.RemoteCraft;
                Name = $"Remote craft { timeCraftMax.TotalMinutes } мин";
                SetData(recipesPages);
            }

            internal override string GetColor()
            {
                return "none";
            }
            internal override string ToString(string newLine, string tabulate)
            {
                string messageStr = "";
                foreach (var recipesPage in RecipesPages.OrderByDescending(pair => pair.IncomeRUBInHour))
                {
                    List<Recipe> recipes = recipesPage.Recipes;
                    RecipeData recipeData = recipesPage.RecipeData.First();
                    messageStr += string.Format(
                        "{5} {0,-50} Профит: {6:0.} ({1:0.} + {3:0.}){4}",
                        string.Format("{0} x {1} = {2:0.}", recipeData.Name, recipes.Count, ParseService.ConvertCopperToGold(recipesPage.CostCraft)),
                        ParseService.ConvertCopperToRUB(recipesPage.NormalProfit),
                        recipesPage.IncomeRUBInHour,
                        ParseService.ConvertCopperToRUB(recipesPage.RandomProfit),
                        newLine,
                        tabulate,
                        ParseService.ConvertCopperToRUB(recipesPage.NormalProfit + recipesPage.RandomProfit));
                    messageStr += recipesPage.ItemsToString(newLine, tabulate);
                }
                messageStr += string.Format("{1} {2}{0}",
                    newLine,
                    tabulate,
                    Name);
                return messageStr;
            }
        }
        public class CraftTargetIncome : Target
        {
            public CraftTargetIncome(List<Recipe> recipes)
            {
                Dictionary<int, RecipesPage> recipesPagesById = new Dictionary<int, RecipesPage>();
                foreach (var recipe in recipes)
                {
                    int recipeId = recipe.RecipeData.ID;
                    recipesPagesById.TryAdd(recipeId, new RecipesPage());
                    recipesPagesById[recipeId].AddRecipe(recipe);
                }
                PriorityDisplay = PriorityDisplay.CraftTargetIncome;
                Name = $"Target Income { ParseService.settings.TargetIncomeInHourInRub } RUB";
                SetData(recipesPagesById.Values.ToList());
            }
        }
        public class CraftNotTargetIncome : Target
        {
            public CraftNotTargetIncome(List<Recipe> recipes, double minIncome)
            {
                PriorityDisplay = PriorityDisplay.CraftNotTargetIncome;
                Name = string.Format("Craft Not Target Income {0:0.} gold", minIncome);
                Dictionary<int, List<Recipe>> recipesById = new Dictionary<int, List<Recipe>>();
                foreach (var recipe in recipes.OrderByDescending(recipe => recipe.GetIncomeGoldInHour()))
                {
                    int recipeId = recipe.RecipeData.ID;
                    recipesById.TryAdd(recipeId, new List<Recipe>());
                    recipesById[recipeId].Add(recipe);
                }
                var recipesPagesAll = new List<RecipesPage>();
                foreach (var recipesList in recipesById.Values)
                {
                    double[] difference = new double[recipesList.Count];
                    for (int i = 0; i < recipesList.Count - 1; i++)
                    {
                        difference[i] = recipesList[i].GetIncomeGoldInHour() - recipesList[i + 1].GetIncomeGoldInHour();
                    }
                    double average = GetAverage(difference);
                    List<Range> clusters = new List<Range>();
                    for (int i = 1; i < difference.Length; i++)
                    {
                        if (difference[i - 1] > difference[i])
                        {
                            clusters.Add(..i);
                            break;
                        }
                    }
                    if (clusters.Count != 0)
                    {
                        bool touchAverage = false;
                        for (int i = clusters.Last().End.Value + 1; i < difference.Length; i++)
                        {
                            if (difference[i - 1] > difference[i] && touchAverage && difference[i - 1] > average)
                            {
                                clusters.Add(clusters.Last().End.Value..i);
                                touchAverage = false;
                            }
                            else
                            {
                                if (difference[i - 1] <= average)
                                {
                                    touchAverage = true;
                                }
                            }
                        }
                        clusters.Add(clusters.Last().End.Value..difference.Length);
                    }
                    else
                    {
                        clusters.Add(..difference.Length);
                    }
                    var recipesArray = recipesList.ToArray();
                    var recipesPages = new List<RecipesPage>();
                    foreach (var cluster in clusters)
                    {
                        recipesPages.Add(new RecipesPage(recipesArray[cluster]));
                    }
                    while (recipesPages.Count > 1)
                    {
                        var recipesPagesDelete = new List<RecipesPage>();
                        var recipesPagesOld = new List<RecipesPage>();
                        for (int i = 0; i < recipesPages.Count - 1; i++)
                        {
                            recipesPages[i].SetData();
                            if (ParseService.ConvertCopperToRUB(recipesPages[i].Profit) < ParseService.settings.TargetProfitInRUB / 2)
                            {
                                var s = new List<Recipe>();
                                s.AddRange(recipesPages[i].Recipes);
                                s.AddRange(recipesPages[i + 1].Recipes);
                                recipesPagesOld.Add(new RecipesPage(s.ToArray()));
                                recipesPagesDelete.Add(recipesPages[i]);
                                recipesPagesDelete.Add(recipesPages[i + 1]);
                                break;
                            }
                            else
                            {
                                recipesPagesDelete.Add(recipesPages[i]);
                                recipesPagesAll.Add(recipesPages[i]);
                            }
                        }
                        foreach (var recipesPage in recipesPagesDelete)
                        {
                            recipesPages.Remove(recipesPage);
                        }
                        recipesPagesOld.AddRange(recipesPages);
                        recipesPages = recipesPagesOld;
                    }
                    var recipePage = recipesPages.Last();
                    recipePage.SetData();
                    recipesPagesAll.Add(recipePage);
                }
                SetData(recipesPagesAll);
                static double GetAverage(double[] difference)
                {
                    double[] differenceSort = (double[])difference.Clone();
                    Array.Sort(differenceSort);
                    double average = (differenceSort[0] + differenceSort[^1]) / 2;
                    return average;
                }
            }
            protected override void SetData(List<RecipesPage> recipesPages)
            {
                foreach (var recipesPage in recipesPages)
                {
                    SetDataa(recipesPage);
                }
            }
        }
        public FactionType FactionType { get; }
        public List<Target> Targets { get; } = new List<Target>();
        public int RecipesCount { get; } = 0;
        public bool PlayMusic { get; private set; } = false;

        public FactionPage(Faction faction, Dictionary<int, ItemPageParser> parsersByIdItem/*, Dictionary<int, Dictionary<long, int>> items*/)
        {
            HashSet<RecipeData> recipesDatasDropToMail = new HashSet<RecipeData>();
            Dictionary<int, ItemPageParser> parsersDropToMailItemsByIdItem = new Dictionary<int, ItemPageParser>();
            foreach (var recipesDataSet in faction.RecipeDataTrees)
            {
                foreach (var recipeData in recipesDataSet)
                {
                    if (recipeData.DropToMail)
                    {
                        recipesDatasDropToMail.Add(recipeData);
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            parsersDropToMailItemsByIdItem.TryAdd(itemData.id, parsersByIdItem[itemData.id]);
                        }
                    }
                }
            }
            Task[] tasks = new Task[]
            {
                //SetBuyTarget(faction, items),
                SetRemoteCraftTarget(faction, ParseService.Clone<Dictionary<int, ItemPageParser>>(parsersDropToMailItemsByIdItem), recipesDatasDropToMail),
                SetDefaultTarget(faction, parsersByIdItem)
            };
            FactionType = faction.factionType;
            var temp = new List<Target>();
            Task.WaitAll(tasks);
            foreach (var target in Targets)
            {
                if (target.RecipesCount == 0)
                {
                    temp.Add(target);
                }
                else
                {
                    RecipesCount += target.RecipesCount;
                }
            }
            foreach (var target in temp)
            {
                Targets.Remove(target);
            }

            Task SetBuyTarget(Faction faction, Dictionary<int, Dictionary<long, int>> items)
            {
                return Task.Run(() => {

                    //Targets.Add(new RemoteCraft(recipesById.Values, faction.Money));
                });
            }

            Task SetRemoteCraftTarget(Faction faction, Dictionary<int, ItemPageParser> parsersByIdItem, IEnumerable<RecipeData> recipesDatasDropToMail)
            {
                return Task.Run(() => {
                    Dictionary<RecipeData, List<Recipe>> recipesByRecipeData = new Dictionary<RecipeData, List<Recipe>>();
                    foreach (var recipeData in recipesDatasDropToMail)
                    {
                        recipesByRecipeData.Add(recipeData, new List<Recipe>());
                        Dictionary<int, ItemPageParser> cloneParsersByIdItem = new Dictionary<int, ItemPageParser>();
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            cloneParsersByIdItem.Add(itemData.id, ParseService.Clone<ItemPageParser>(parsersByIdItem[itemData.id]));
                        }
                        while (EnoughItemsForCraftRecipe(cloneParsersByIdItem, recipeData))
                        {
                            Recipe recipe = new Recipe(recipeData, cloneParsersByIdItem, faction);
                            if (recipe.NormalProfit < 0)
                            {
                                break;
                            }
                            recipesByRecipeData[recipe.RecipeData].Add(recipe);
                            recipe.ReserveItems(cloneParsersByIdItem);
                        }
                    }
                    Targets.Add(new RemoteCraft(recipesByRecipeData.Values, faction.Money));
                });
            }

            Task SetDefaultTarget(Faction faction, Dictionary<int, ItemPageParser> parsersByIdItem)
            {
                return Task.Run(async () =>
                {
                    double minIncome = ParseService.settings.TargetIncomeInHourInRub * 0.8;
                    List<Recipe> recipesForCraftTargetIncome = new List<Recipe>();
                    List<Recipe> recipesForCraftNotTargetIncome = new List<Recipe>();
                    foreach (var recipeDataTree in faction.RecipeDataTrees)
                    {
                        while (true)
                        {
                            List<Recipe> profitableRecipes = new List<Recipe>();
                            foreach (var recipeData in recipeDataTree)
                            {
                                if (EnoughItemsForCraftRecipe(parsersByIdItem, recipeData))
                                {
                                    Recipe recipe = new Recipe(recipeData, parsersByIdItem, faction);
                                    if (ParseService.GetIncomeRUBInHour(recipe.NormalProfit, recipe.NeedMillisecondsToCraft) >= minIncome)
                                    {
                                        profitableRecipes.Add(recipe);
                                    }
                                }
                            }
                            if (profitableRecipes.Count > 0)
                            {
                                Recipe maxProfitableRecipe = profitableRecipes.OrderByDescending(recipe => recipe.GetIncomeGoldInHour()).First();
                                if (maxProfitableRecipe.GetIncomeRUBInHour() < ParseService.settings.TargetIncomeInHourInRub)
                                {
                                    recipesForCraftNotTargetIncome.Add(maxProfitableRecipe);
                                }
                                else
                                {
                                    recipesForCraftTargetIncome.Add(maxProfitableRecipe);
                                }
                                maxProfitableRecipe.ReserveItems(parsersByIdItem);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    var craftTargetIncome = await Task.Run(() => {
                        return new CraftTargetIncome(recipesForCraftTargetIncome);
                    });
                    Targets.Add(craftTargetIncome);
                    var craftNotTargetIncome = await Task.Run(() => {
                        return new CraftNotTargetIncome(recipesForCraftNotTargetIncome, minIncome);
                    });
                    Targets.Add(craftNotTargetIncome);
                    if (craftTargetIncome.RecipesCount + craftNotTargetIncome.RecipesCount > 0)
                    {
                        if (ParseService.ConvertCopperToRUB(craftTargetIncome.Profit) >= ParseService.settings.TargetProfitInRUB)
                        {
                            if (faction.farmMode)
                            {
                                craftNotTargetIncome.IsReached = true;
                            }
                            PlayMusic = true;
                            craftTargetIncome.IsReached = true;
                        }
                        else
                        {
                            if (ParseService.ConvertCopperToRUB(craftTargetIncome.Profit + craftNotTargetIncome.Profit) >=
                                ParseService.settings.TargetProfitInRUB && faction.farmMode)
                            {
                                PlayMusic = true;
                                craftTargetIncome.IsReached = true;
                                craftNotTargetIncome.IsReached = true;
                            }
                        }
                    }
                });
            }

            static bool EnoughItemsForCraftRecipe(Dictionary<int, ItemPageParser> parsersByIdItem, RecipeData recipeData)
            {
                bool enoughItemsForRecipe = true;
                foreach (var itemData in recipeData.ItemsData)
                {
                    enoughItemsForRecipe = enoughItemsForRecipe &&
                        parsersByIdItem[itemData.id].HasRequiredAmount(recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]);
                }

                return enoughItemsForRecipe;
            }
        }

    }
    class AuctionParser
    {
        public List<FactionPage> Factions { get; } = new List<FactionPage>();
        public AuctionParser(Server server)
        {
            Dictionary<FactionType, Dictionary<int, List<Auction>>> auctionsByIdItem = new Dictionary<FactionType, Dictionary<int, List<Auction>>>();
            //Dictionary<int, Dictionary<long, int>> quatityItemsByPriceItemByIdItem = new Dictionary<int, Dictionary<long, int>>();
            //HashSet<int> itemsIdOneItemInRecipe = new HashSet<int>();
            foreach (var faction in server.factions.Values)
            {
                auctionsByIdItem.Add(faction.factionType, new Dictionary<int, List<Auction>>());
                HashSet<int> activeItemsId = new HashSet<int>();
                foreach (var recipeDataTree in faction.RecipeDataTrees)
                {
                    foreach (var recipeData in recipeDataTree)
                    {
                        //if (recipeData.ItemsData.Count == 1)
                        //{
                        //    itemsIdOneItemInRecipe.Add(recipeData.ItemsData.First().id);
                        //}
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            activeItemsId.Add(itemData.id);
                        }
                    }
                }
                foreach (var itemId in activeItemsId)
                {
                    auctionsByIdItem[faction.factionType].Add(itemId, new List<Auction>());
                }
            }
            //foreach (var itemId in itemsIdOneItemInRecipe)
            //{
            //    quatityItemsByPriceItemByIdItem.Add(itemId, new Dictionary<long, int>());
            //}
            AuctionData auctionData = ParseService.GetAuctionData(server.connectedRealmId);
            foreach (var auction in auctionData.Auctions)
            {
                foreach (var faction in server.factions.Values)
                {
                    if (auctionsByIdItem[faction.factionType].Keys.Contains(auction.Item.Id))
                    {
                        auctionsByIdItem[faction.factionType][auction.Item.Id].Add(auction);
                    }
                }
                //if (quatityItemsByPriceItemByIdItem.Keys.Contains(auction.Item.Id))
                //{
                //    if (!quatityItemsByPriceItemByIdItem[auction.Item.Id].ContainsKey(auction.UnitPrice))
                //    {
                //        quatityItemsByPriceItemByIdItem[auction.Item.Id].Add(auction.UnitPrice, 0);
                //    }
                //    quatityItemsByPriceItemByIdItem[auction.Item.Id][auction.UnitPrice] += auction.Quantity;
                //}
            }
            foreach (var faction in server.factions.Values)
            {
                Dictionary<int, ItemPageParser> parsersByIdItem = new Dictionary<int, ItemPageParser>();
                foreach (var keyValuePair in auctionsByIdItem[faction.factionType])
                {
                    parsersByIdItem.Add(keyValuePair.Key, new ItemPageParser(keyValuePair.Value));
                }
                var a = new FactionPage(faction, parsersByIdItem);
                Factions.Add(a);
            }
        }
    }
}