using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wowCalc;

namespace info
{
    public class Recipe
    {
        public RecipeData RecipeData { get; }
        public long Profit { get; }
        public Dictionary<int, List<Item>> Items { get; }
        public double IncomeGoldInHour { get; }
        public double NeedMillisecondsToCraft { get; private set; }
        public long CostCraft { get; }

        public Recipe(RecipeData recipeData, double needMillisecondsToCraft, Dictionary<int, List<Item>> items, long spending, long costCraft)
        {
            Items = items;
            RecipeData = recipeData;
            CostCraft = costCraft;
            NeedMillisecondsToCraft = needMillisecondsToCraft;
            Profit = RecipeData.SellNormalPrice - spending - CostCraft;
            IncomeGoldInHour = ParseService.GetIncomeGoldInHour(Profit, NeedMillisecondsToCraft);
        }

        internal void SetDefaultNeedMillisecondsToCraft()
        {
            NeedMillisecondsToCraft = RecipeData.GetNeedMillisecondsToCraft(false);
        }
    }
}