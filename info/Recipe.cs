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
            double income = profit / recipeData.TIME_NEED;
            double incomeComparableRecipe = comparableRecipe.profit / comparableRecipe.recipeData.TIME_NEED;
            return income.CompareTo(incomeComparableRecipe);
        }
    }
}