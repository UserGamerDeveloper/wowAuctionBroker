using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    public class Recipe
    {
        private class ComparerByDescendingIncomeGoldInHour : IComparer<Recipe>
        {
            public int Compare(Recipe obj1, Recipe obj2)
            {
                return obj2.IncomeGoldInHour.CompareTo(obj1.IncomeGoldInHour);
            }
        }

        public RecipeData recipeData;
        public long profit;
        public Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();
        public double IncomeGoldInHour { get; }
        public long spending;
        public long costCraft = 0;
        public double needMillisecondsToCraft;

        public Recipe(RecipeData recipeData, Server server, Dictionary<int, ItemPageParser> parsersForTree, long summaryCostCraft)
        {
            this.recipeData = recipeData;
            spending = Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());

            foreach (var itemData in recipeData.ItemsData)
            {
                items.Add(itemData.id, new List<Item>());
                for (int i = 0; i < recipeData.ID_ITEM_AND_NEED_AMOUNT[itemData.id]; i++)
                {
                    Item item = parsersForTree[itemData.id].GetItem(i);
                    costCraft += item.cost;
                    items[itemData.id].Add(item);
                }
            }
            profit = recipeData.SellNormalPrice - spending - costCraft;
            if (recipeData.DropToMail && server.Money >= summaryCostCraft + costCraft)
            {
                needMillisecondsToCraft = RecipeData.BFANeedMillisecondsToCraft;
            }
            else
            {
                needMillisecondsToCraft = recipeData.NeedMillisecondsToCraft;
            }
            IncomeGoldInHour = Util.GetIncomeGoldInHour(profit, TimeSpan.FromMilliseconds(needMillisecondsToCraft));
        }

        internal static void SortByDescendingIncomeGoldInHour(List<Recipe> profitableRecipes)
        {
            profitableRecipes.Sort(new ComparerByDescendingIncomeGoldInHour());
        }
    }
}