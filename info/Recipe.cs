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
        public double incomeGoldInHour;
        public long spending;

        public Recipe(RecipeData recipeData, Server server)
        {
            this.recipeData = recipeData;
            float discount = 0f;
            switch (server.reputation)
            {
                case Reputation.Neutral:
                    {
                        break;
                    }
                case Reputation.Frenly:
                    {
                        discount = 0.05f;
                        break;
                    }
                case Reputation.Honored:
                    {
                        discount = 0.1f;
                        break;
                    }
                case Reputation.Resived:
                    {
                        discount = 0.15f;
                        break;
                    }
                default:
                    throw new Exception("неизвестная репутация");
            }
            spending = Convert.ToInt64(recipeData.SPENDING * (1f - discount));
        }

        public void SetProfit(long costCraft)
        {
            profit = recipeData.SELL_PRICE - spending - costCraft;
        }

        public int CompareTo(Recipe comparableRecipe)
        {
            return comparableRecipe.incomeGoldInHour.CompareTo(incomeGoldInHour);
        }

        internal void SetIncome()
        {
            incomeGoldInHour = Util.getIncomeGoldInHour(profit, new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(recipeData.TIME_NEED));
        }
    }
}