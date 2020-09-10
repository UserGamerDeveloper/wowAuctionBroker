using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    public class Recipe : IComparable<Recipe>
    {
        public RecipeData recipeData;
        public long profit;
        public Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();
        public double IncomeGoldInHour { get; }
        public long spending;

        public Recipe(RecipeData recipeData, Server server, Dictionary<int, ItemPageParser> parsersForTree)
        {
            this.recipeData = recipeData;
            spending = Convert.ToInt64(recipeData.SPENDING * server.GetSpendingRate());

            long costCraft = 0;
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
            IncomeGoldInHour = Util.GetIncomeGoldInHour(profit, TimeSpan.FromMilliseconds(recipeData.NeedMillisecondsToCraft));
        }

        public int CompareTo(Recipe comparableRecipe)
        {
            return IncomeGoldInHour.CompareTo(comparableRecipe.IncomeGoldInHour);
        }
    }
}