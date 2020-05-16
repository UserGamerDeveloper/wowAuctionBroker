using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    public class Recipe : IComparable
    {
        public RecipeData recipeData;
        public long profit;
        public Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();
        public double income;

        public Recipe(RecipeData recipeData)
        {
            this.recipeData = recipeData;
        }

        public void SetProfit(long costCraft)
        {
            profit = recipeData.SELL_PRICE - recipeData.SPENDING - costCraft;
        }

        public int CompareTo(object obj)
        {
            Recipe comparableRecipe = obj as Recipe;
            return income.CompareTo(comparableRecipe.income);
        }

        internal void SetIncome()
        {
            income = Util.getIncomeGoldInHour(profit, new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(recipeData.TIME_NEED));
        }
    }
}