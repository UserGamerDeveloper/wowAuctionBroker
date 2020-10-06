using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wowCalc;
using static info.AuctionData;

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

    public class AuctionData
    {
        public class Auction
        {
            public class Itemm
            {
                //public class Modifier
                //{
                //    [JsonProperty("type")]
                //    public int Type { get; set; }
                //    [JsonProperty("value")]
                //    public int Value { get; set; }
                //}

                [JsonProperty("id")]
                public int Id { get; set; }
                //[JsonProperty("context")]
                //public int Context { get; set; }
                //[JsonProperty("modifiers")]
                //public List<Modifier> Modifiers { get; set; }
                //[JsonProperty("pet_breed_id")]
                //public int PetBreedId { get; set; }
                //[JsonProperty("pet_level")]
                //public int PetLevel { get; set; }
                //[JsonProperty("pet_quality_id")]
                //public int PetQualityId { get; set; }
                //[JsonProperty("pet_species_id")]
                //public int PetSpeciesId { get; set; }
            }

            [JsonProperty("item")]
            public Itemm Item { get; set; }
            [JsonProperty("unit_price")]
            public long UnitPrice { get; set; }
            [JsonProperty("quantity")]
            public int Quantity { get; set; }
            //[JsonProperty("id")]
            //public int Id { get; set; }
            //[JsonProperty("bid")]
            //public long Bid { get; set; }
            //[JsonProperty("buyout")]
            //public long Buyout { get; set; }
            //[JsonProperty("time_left")]
            //public string TimeLeft { get; set; }
        }
        //public class Links
        //{
        //    [JsonProperty("self")]
        //    public Self Self { get; set; }
        //}
        //public class Self
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}
        //public class ConnectedRealm
        //{
        //    [JsonProperty("href")]
        //    public string Href { get; set; }
        //}

        [JsonProperty("auctions")]
        public List<Auction> Auctions { get; set; }
        //[JsonProperty("_links")]
        //public Links Links { get; set; }
        //[JsonProperty("connected_realm")]
        //public ConnectedRealm ConnectedRealm { get; set; }
    }

    class AuctionParser
    {
        public List<RecipesPage> RecipesPages { get; } = new List<RecipesPage>();
        public double ProfitInTargetIncome { get; } = 0;

        public long NormalProfit = 0;
        public double TimeCraftInMilliseconds = 0;
        public double globalRandomProfit = 0;
        public int recipesCount = 0;

        public AuctionParser(Server server)
        {
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
            string auctionDataStr = ParseService.GetAuctionDataStr(server.connectedRealmId);
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

            Dictionary<int, RecipesPage> recipesPagesById = new Dictionary<int, RecipesPage>();
            Dictionary<int, RecipesPage> notTargetIncomeRecipesPagesById = new Dictionary<int, RecipesPage>();
            long summaryCostCraft = 0L;
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
                            long spending = Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());
                            double needMillisecondsToCraft = recipeData.GetNeedMillisecondsToCraft(server.Money, summaryCostCraft, costCraft);
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
                        int recipeId = maxProfitableRecipe.RecipeData.ID;
                        RecipesPage recipesPage;
                        double incomeWithRandomProfit = ParseService.GetIncomeGoldInHour(
                            maxProfitableRecipe.Profit + maxProfitableRecipe.RecipeData.GetRandomProfit(),
                            maxProfitableRecipe.NeedMillisecondsToCraft);
                        if (incomeWithRandomProfit < server.GetTargetIncomeGoldInHour() /*ParseService.settings.TARGET_INCOME_IN_HOUR*/ /*&& server.farmMode*/)
                        {
                            notTargetIncomeRecipesPagesById.TryAdd(recipeId, new RecipesPage());
                            recipesPage = notTargetIncomeRecipesPagesById[recipeId];
                        }
                        else
                        {
                            recipesPagesById.TryAdd(recipeId, new RecipesPage());
                            recipesPage = recipesPagesById[recipeId];
                        }
                        summaryCostCraft += maxProfitableRecipe.CostCraft;
                        recipesPage.NormalProfit += maxProfitableRecipe.Profit;
                        recipesPage.TimeCraftInMilliseconds += maxProfitableRecipe.NeedMillisecondsToCraft;
                        recipesPage.Recipes.Add(maxProfitableRecipe);
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
            foreach (var recipesPage in recipesPagesById.Values)
            {
                SetData(recipesPage);
                ProfitInTargetIncome += recipesPage.NormalProfit + recipesPage.randomProfit;
            }
            foreach (var recipesPage in notTargetIncomeRecipesPagesById.Values)
            {
                SetData(recipesPage);
            }
        }

        private void SetData(RecipesPage recipesPage)
        {
            recipesPage.SetData();
            NormalProfit += recipesPage.NormalProfit;
            TimeCraftInMilliseconds += recipesPage.TimeCraftInMilliseconds;
            globalRandomProfit += recipesPage.randomProfit;
            recipesCount += recipesPage.Recipes.Count;
            RecipesPages.Add(recipesPage);
        }
    }
}
