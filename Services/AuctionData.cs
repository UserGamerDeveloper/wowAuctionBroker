using Mvc.Client.Models;
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
        public List<Recipe> Recipes = new List<Recipe>();
        public long NormalProfit = 0;
        public double IncomeGoldInHour = 0;
        public double TimeCraftInMilliseconds = 0;
        public double randomProfit = 0;
        public RecipeData recipeData;

        public long CostCraft { get; set; } = 0;

        public void SetData()
        {
            recipeData = Recipes[0].RecipeData;
            randomProfit = Recipes.Count * recipeData.GetRandomProfit();
            IncomeGoldInHour = ParseService.GetIncomeGoldInHour(NormalProfit + randomProfit, TimeCraftInMilliseconds);
        }
        public long GetMaxPrice(ItemData itemData)
        {
            Dictionary<long, List<Item>> bidsItemInRecipeByCost = new Dictionary<long, List<Item>>();
            foreach (var recipe in Recipes)
            {
                foreach (var item in recipe.Items[itemData.id])
                {
                    if (!bidsItemInRecipeByCost.ContainsKey(item.cost))
                    {
                        bidsItemInRecipeByCost.Add(item.cost, new List<Item>());
                    }
                    bidsItemInRecipeByCost[item.cost].Add(item);
                }
            }
            long maxPrice = 0;
            foreach (var bidsItemInRecipeByCostPair in bidsItemInRecipeByCost.OrderByDescending(pair => pair.Key))
            {
                maxPrice = bidsItemInRecipeByCostPair.Key;
                if (bidsItemInRecipeByCostPair.Value.Count >= recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id])
                {
                    break;
                }
            }

            return maxPrice;
        }
    }
    public class FactionPage
    {
        public string Name { get;}
        public List<RecipesPage> RecipesPages { get; } = new List<RecipesPage>();
        public double ProfitInTargetIncome { get; } = 0;
        public double TargetIncomeNormalProfit { get; } = 0;
        public double TargetIncomeRandomProfit { get; } = 0;
        public double TargetIncomeTimeCraftInMilliseconds { get; } = 0;
        public int TargetIncomeRecipesCount { get; } = 0;
        public double ProfitOutTargetIncome { get; }
        public long NotTargetIncomeNormalProfit { get; } = 0;
        public double NotTargetIncomeTimeCraftInMilliseconds { get; } = 0;
        public double NotTargetIncomeRandomProfit { get; } = 0;
        public int NotTargetIncomeRecipesCount { get; } = 0;

        public FactionPage(Faction faction, Dictionary<int, ItemPageParser> parsersByIdItem)
        {
            Name = faction.factionType.ToString();
            List<Recipe> recipes = new List<Recipe>();
            foreach (var recipeDataTree in faction.RecipeDataTrees)
            {
                while (true)
                {
                    List<Recipe> profitableRecipes = new List<Recipe>();
                    foreach (var recipeData in recipeDataTree.Value)
                    {
                        bool enoughItemsForRecipe = true;
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            enoughItemsForRecipe = enoughItemsForRecipe &&
                                parsersByIdItem[itemData.id].HasRequiredAmount(recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]);
                        }
                        if (enoughItemsForRecipe)
                        {
                            long costCraft = 0;
                            Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();
                            foreach (var itemData in recipeData.ItemsData)
                            {
                                items.Add(itemData.id, new List<Item>());
                                for (int i = 0; i < recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]; i++)
                                {
                                    Item item = parsersByIdItem[itemData.id].GetItem(i);
                                    costCraft += item.cost;
                                    items[itemData.id].Add(item);
                                }
                            }
                            long spending = Convert.ToInt64(recipeData.SPENDING * faction.GetSpendingRate());
                            double needMillisecondsToCraft = recipeData.GetNeedMillisecondsToCraft(/*faction.moneyMax, summaryCostCraft, costCraft*/);
                            Recipe recipe = new Recipe(recipeData, needMillisecondsToCraft, items, spending, costCraft);
                            if (recipe.IncomeGoldInHour >= 0)
                            {
                                profitableRecipes.Add(recipe);
                            }
                        }
                    }
                    if (profitableRecipes.Count > 0)
                    {
                        Recipe maxProfitableRecipe = profitableRecipes.OrderByDescending(recipe => recipe.IncomeGoldInHour).First();
                        recipes.Add(maxProfitableRecipe);
                        foreach (var idItem in maxProfitableRecipe.Items.Keys)
                        {
                            foreach (var item in maxProfitableRecipe.Items[idItem])
                            {
                                parsersByIdItem[idItem].Remove(item);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Dictionary<int, RecipesPage> recipesPagesById = new Dictionary<int, RecipesPage>();
            Dictionary<int, RecipesPage> notTargetIncomeRecipesPagesById = new Dictionary<int, RecipesPage>();
            long summaryCostCraft = 0;
            foreach (var recipe in recipes.OrderByDescending(recipe => recipe.IncomeGoldInHour))
            {
                if (summaryCostCraft + recipe.CostCraft > faction.moneyMax)
                {
                    recipe.SetDefaultNeedMillisecondsToCraft();
                }
                else
                {
                    summaryCostCraft += recipe.CostCraft;
                }
                double incomeGoldInHourWithRandomProfit = ParseService.GetIncomeGoldInHour(
                    recipe.Profit + recipe.RecipeData.GetRandomProfit(),
                    recipe.NeedMillisecondsToCraft);
                RecipesPage recipesPage;
                int recipeId = recipe.RecipeData.ID;
                if (incomeGoldInHourWithRandomProfit < ParseService.settings.TARGET_INCOME_IN_HOUR)
                {
                    notTargetIncomeRecipesPagesById.TryAdd(recipeId, new RecipesPage());
                    recipesPage = notTargetIncomeRecipesPagesById[recipeId];
                }
                else
                {
                    recipesPagesById.TryAdd(recipeId, new RecipesPage());
                    recipesPage = recipesPagesById[recipeId];
                }
                recipesPage.CostCraft += recipe.CostCraft;
                recipesPage.NormalProfit += recipe.Profit;
                recipesPage.TimeCraftInMilliseconds += recipe.NeedMillisecondsToCraft;
                recipesPage.Recipes.Add(recipe);
            }
            foreach (var recipesPage in recipesPagesById.Values)
            {
                recipesPage.SetData();
                RecipesPages.Add(recipesPage);
                TargetIncomeNormalProfit += recipesPage.NormalProfit;
                TargetIncomeRandomProfit += recipesPage.randomProfit;
                TargetIncomeTimeCraftInMilliseconds += recipesPage.TimeCraftInMilliseconds;
                TargetIncomeRecipesCount += recipesPage.Recipes.Count;
            }
            ProfitInTargetIncome = TargetIncomeNormalProfit + TargetIncomeRandomProfit;
            foreach (var recipesPage in notTargetIncomeRecipesPagesById.Values)
            {
                recipesPage.SetData();
                RecipesPages.Add(recipesPage);
                NotTargetIncomeNormalProfit += recipesPage.NormalProfit;
                NotTargetIncomeRandomProfit += recipesPage.randomProfit;
                NotTargetIncomeTimeCraftInMilliseconds += recipesPage.TimeCraftInMilliseconds;
                NotTargetIncomeRecipesCount += recipesPage.Recipes.Count;
            }
            ProfitOutTargetIncome = NotTargetIncomeNormalProfit + NotTargetIncomeRandomProfit;
        }
    }
    class AuctionParser
    {
        public List<FactionPage> Factions { get; } = new List<FactionPage>();
        public double ProfitInTargetIncome { get; set; } = 0;
        public double TargetIncomeNormalProfit { get; set; } = 0;
        public double TargetIncomeRandomProfit { get; set; } = 0;
        public double TargetIncomeTimeCraftInMilliseconds { get; set; } = 0;
        public int TargetIncomeRecipesCount { get; set; } = 0;
        public double ProfitOutTargetIncome { get; set; }
        public long NotTargetIncomeNormalProfit { get; set; } = 0;
        public double NotTargetIncomeTimeCraftInMilliseconds { get; set; } = 0;
        public double NotTargetIncomeRandomProfit { get; set; } = 0;
        public int NotTargetIncomeRecipesCount { get; set; } = 0;
        public AuctionParser(Server server)
        {
            HashSet<int> itemsId = new HashSet<int>();
            HashSet<int> itemsIdOneItemInRecipe = new HashSet<int>();
            foreach (var faction in server.factions.Values)
            {
                foreach (var recipeDataTree in faction.RecipeDataTrees)
                {
                    foreach (var recipeData in recipeDataTree.Value)
                    {
                        if (recipeData.ItemsData.Count == 1)
                        {
                            itemsIdOneItemInRecipe.Add(recipeData.ItemsData.First().id);
                        }
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            itemsId.Add(itemData.id);
                        }
                    }
                }
            }
            Dictionary<int, List<Auction>> auctionsByIdItem = new Dictionary<int, List<Auction>>();
            Dictionary<int, Dictionary<long, int>> quatityItemsByPriceItemByIdItem = new Dictionary<int, Dictionary<long, int>>();
            foreach (var itemId in itemsId)
            {
                auctionsByIdItem.Add(itemId, new List<Auction>());
            }
            foreach (var itemId in itemsIdOneItemInRecipe)
            {
                quatityItemsByPriceItemByIdItem.Add(itemId, new Dictionary<long, int>());
            }
            AuctionData auctionData = ParseService.GetAuctionData(server.connectedRealmId);
            foreach (var auction in auctionData.Auctions)
            {
                if (auctionsByIdItem.Keys.Contains(auction.Item.Id))
                {
                    auctionsByIdItem[auction.Item.Id].Add(auction);
                }
                if (quatityItemsByPriceItemByIdItem.Keys.Contains(auction.Item.Id))
                {
                    if (!quatityItemsByPriceItemByIdItem[auction.Item.Id].ContainsKey(auction.UnitPrice))
                    {
                        quatityItemsByPriceItemByIdItem[auction.Item.Id].Add(auction.UnitPrice, 0);
                    }
                    quatityItemsByPriceItemByIdItem[auction.Item.Id][auction.UnitPrice] += auction.Quantity;
                }
            }
            Dictionary<int, ItemPageParser> parsersByIdItem = new Dictionary<int, ItemPageParser>();
            foreach (var keyValuePair in auctionsByIdItem)
            {
                parsersByIdItem.Add(keyValuePair.Key, new ItemPageParser(keyValuePair.Value));
            }

            foreach (var faction in server.factions.Values)
            {
                var a = new FactionPage(faction, parsersByIdItem);
                SetData(a);
                Factions.Add(a);
            }
        }
        private void SetData(FactionPage alliance)
        {
            TargetIncomeNormalProfit += alliance.TargetIncomeNormalProfit;
            TargetIncomeRandomProfit += alliance.TargetIncomeRandomProfit;
            TargetIncomeTimeCraftInMilliseconds += alliance.TargetIncomeTimeCraftInMilliseconds;
            TargetIncomeRecipesCount += alliance.TargetIncomeRecipesCount;
            NotTargetIncomeNormalProfit += alliance.NotTargetIncomeNormalProfit;
            NotTargetIncomeRandomProfit += alliance.NotTargetIncomeRandomProfit;
            NotTargetIncomeTimeCraftInMilliseconds += alliance.NotTargetIncomeTimeCraftInMilliseconds;
            NotTargetIncomeRecipesCount += alliance.NotTargetIncomeRecipesCount;
            ProfitInTargetIncome += alliance.ProfitInTargetIncome;
            ProfitOutTargetIncome += alliance.ProfitOutTargetIncome;
        }
    }
}