using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    public class RecipesPage
    {
        public List<Recipe> Recipes = new List<Recipe>();
        public long SummaryProfit = 0L;
        public double AverageIncome = 0d;
        public TimeSpan TimeCraft = new TimeSpan();
        public double randomProfit = 0d;
        public RecipeData recipeData;

        public long GetMaxPrice(ItemData itemData)
        {
            Dictionary<long, List<Item>> bidsItemInRecipeByCost = new Dictionary<long, List<Item>>();
            foreach (var recipe in Recipes)
            {
                foreach (var item in recipe.items[itemData.id])
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

    class AuctionData
    {
        private const double ChanceRandomProfit = 0.165562913907285d;

        public Dictionary<int, RecipesPage> recipesById { get; } = new Dictionary<int, RecipesPage>();
        public long globalProfit = 0L;
        public TimeSpan timeCraft = new TimeSpan();
        public double globalRandomProfit = 0d;
        public int recipesCount = 0;

        public AuctionData(Server server)
        {
            int targetIncomeInHour;
            if (server.farmMode)
            {
                targetIncomeInHour = 0;
            }
            else
            {
                targetIncomeInHour = Program.settings.TARGET_INCOME_IN_HOUR;
            }
            long summaryCostCraft = 0L;
            Dictionary<int, double> summaryIncomeRecipesByRecipeId = new Dictionary<int, double>();
            //recipesById = new Dictionary<int, RecipesPage>();
            foreach (var recipeDataTree in server.RecipeDataTrees)
            {
                HashSet<ItemData> itemsDataTree = new HashSet<ItemData>();
                foreach (var recipeData in recipeDataTree)
                {
                    foreach (var itemData in recipeData.ItemsData)
                    {
                        itemsDataTree.Add(itemData);
                    }
                }
                Dictionary<int, ItemPageParser> parsersForTree = new Dictionary<int, ItemPageParser>();
                foreach (var itemData in itemsDataTree)
                {
                    ItemPageParser parser = new ItemPageParser(server.id, itemData.id);
                    parsersForTree.Add(itemData.id, parser);
                }
                while (true)
                {
                    List<Recipe> profitableRecipes = new List<Recipe>();
                    foreach (var recipeData in recipeDataTree)
                    {
                        bool enoughItemsForRecipe = true;
                        foreach (var itemData in recipeData.ItemsData)
                        {
                            enoughItemsForRecipe = enoughItemsForRecipe &&
                                parsersForTree[itemData.id].HasRequiredAmount(recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]);
                        }
                        if (enoughItemsForRecipe)
                        {
                            Recipe recipe = new Recipe(recipeData, server, parsersForTree, summaryCostCraft);
                            if (recipe.IncomeGoldInHour >= targetIncomeInHour)
                            {
                                profitableRecipes.Add(recipe);
                            }
                        }
                    }
                    if (profitableRecipes.Count > 0)
                    {
                        Recipe.SortByDescendingIncomeGoldInHour(profitableRecipes);
                        Recipe maxProfitableRecipe = profitableRecipes[0];
                        int recipeId = maxProfitableRecipe.recipeData.ID;
                        if (!recipesById.ContainsKey(recipeId))
                        {
                            recipesById.Add(recipeId, new RecipesPage());
                            summaryIncomeRecipesByRecipeId.Add(recipeId, 0d);
                        }
                        RecipesPage recipesPage = recipesById[recipeId];
                        summaryCostCraft += maxProfitableRecipe.costCraft;
                        recipesPage.SummaryProfit += maxProfitableRecipe.profit;
                        recipesPage.TimeCraft += TimeSpan.FromMilliseconds(maxProfitableRecipe.needMillisecondsToCraft);
                        recipesPage.Recipes.Add(maxProfitableRecipe);
                        summaryIncomeRecipesByRecipeId[recipeId] += maxProfitableRecipe.IncomeGoldInHour;
                        foreach (var idItem in maxProfitableRecipe.items.Keys)
                        {
                            foreach (var item in maxProfitableRecipe.items[idItem])
                            {
                                parsersForTree[idItem].Remove(item);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            foreach (var keyValuePair in recipesById)
            {
                RecipesPage recipesPage = keyValuePair.Value;
                recipesPage.recipeData = recipesPage.Recipes[0].recipeData;
                recipesPage.AverageIncome += summaryIncomeRecipesByRecipeId[keyValuePair.Key] / recipesPage.Recipes.Count;
                globalProfit += recipesPage.SummaryProfit;
                timeCraft += recipesPage.TimeCraft;
                recipesPage.randomProfit = recipesPage.Recipes.Count * recipesPage.recipeData.GetRandomProfit() * ChanceRandomProfit;
                globalRandomProfit += recipesPage.randomProfit;
                recipesCount += recipesPage.Recipes.Count;
            }
            Dictionary<int, RecipesPage> tempRecipesById = recipesById;
            recipesById = new Dictionary<int, RecipesPage>();
            foreach (var keyValuePair in tempRecipesById.OrderByDescending(pair => pair.Value.AverageIncome))
            {
                recipesById.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
