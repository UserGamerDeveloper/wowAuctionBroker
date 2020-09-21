using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wowCalc;

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

    public class AuctionData
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }
        [JsonProperty("connected_realm")]
        public ConnectedRealm ConnectedRealm { get; set; }
        [JsonProperty("auctions")]
        public List<Auction> Auctions { get; set; }
    }
    public class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }
    public class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
    public class ConnectedRealm
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
    public class Auction
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("item")]
        public Itemm Item { get; set; }
        [JsonProperty("bid")]
        public long Bid { get; set; }
        [JsonProperty("buyout")]
        public long Buyout { get; set; }
        [JsonProperty("unit_price")]
        public long UnitPrice { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("time_left")]
        public string TimeLeft { get; set; }
    }
    public class Itemm
    {
        public class Modifier
        {
            [JsonProperty("type")]
            public int Type { get; set; }
            [JsonProperty("value")]
            public int Value { get; set; }
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("context")]
        public int Context { get; set; }
        [JsonProperty("modifiers")]
        public List<Modifier> Modifiers { get; set; }
        [JsonProperty("pet_breed_id")]
        public int PetBreedId { get; set; }
        [JsonProperty("pet_level")]
        public int PetLevel { get; set; }
        [JsonProperty("pet_quality_id")]
        public int PetQualityId { get; set; }
        [JsonProperty("pet_species_id")]
        public int PetSpeciesId { get; set; }
    }

    class AuctionParser
    {
        private const double ChanceRandomProfit = 0.177637947725073d;

        public Dictionary<int, RecipesPage> recipesById { get; } = new Dictionary<int, RecipesPage>();
        public long globalProfit = 0L;
        public TimeSpan timeCraft = new TimeSpan();
        public double globalRandomProfit = 0d;
        public int recipesCount = 0;

        public AuctionParser(Server server)
        {
            double targetIncomeInHour;
            if (server.farmMode)
            {
                targetIncomeInHour = 0d;
            }
            else
            {
                targetIncomeInHour = Program.settings.TARGET_INCOME_IN_HOUR;
            }
            long summaryCostCraft = 0L;

            HashSet<int> itemsId = new HashSet<int>();
            foreach (var recipeDataTree in server.RecipeDataTrees)
            {
                foreach (var recipeData in recipeDataTree.Value)
                {
                    foreach (var itemData in recipeData.ItemsData)
                    {
                        itemsId.Add(itemData.id);
                    }
                }
            }
            Dictionary<int, List<Auction>> auctionsByIdItem = new Dictionary<int, List<Auction>>();
            foreach (var itemId in itemsId)
            {
                auctionsByIdItem.Add(itemId, new List<Auction>());
            }
            string auctionDataStr = Util.GetAuctionDataStr(server.id);
            AuctionData auctionData = JsonConvert.DeserializeObject<AuctionData>(auctionDataStr);
            foreach (var auction in auctionData.Auctions)
            {
                if (auctionsByIdItem.Keys.Contains(auction.Item.Id))
                {
                    auctionsByIdItem[auction.Item.Id].Add(auction);
                }
            }
            Dictionary<int, ItemPageParser> parsersByIdItem = new Dictionary<int, ItemPageParser>();
            foreach (var keyValuePair in auctionsByIdItem)
            {
                parsersByIdItem.Add(keyValuePair.Key, new ItemPageParser(keyValuePair.Value));
            }

            Dictionary<int, double> summaryIncomeRecipesByRecipeId = new Dictionary<int, double>();
            foreach (var recipeDataTree in server.RecipeDataTrees)
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
                            Recipe recipe = new Recipe(recipeData, server, parsersByIdItem, summaryCostCraft);
                            if (recipe.IncomeGoldInHour >= targetIncomeInHour)
                            {
                                profitableRecipes.Add(recipe);
                            }
                        }
                    }
                    if (profitableRecipes.Count > 0)
                    {
                        Recipe maxProfitableRecipe = profitableRecipes.OrderByDescending(recipe => recipe.IncomeGoldInHour).First();
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
        }
    }
}
