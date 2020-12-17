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
        public long NormalProfit { get; }
        public Dictionary<int, List<Item>> Items { get; }
        public double NeedMillisecondsToCraft { get; private set; }
        public long CostCraft { get; }

        public Recipe(RecipeData recipeData, Dictionary<int, ItemPageParser> parsersByIdItem, Faction faction)
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
            Items = items;
            RecipeData = recipeData;
            CostCraft = costCraft;
            NeedMillisecondsToCraft = recipeData.GetNeedMillisecondsToGetProfit();
            NormalProfit = RecipeData.SellNormalPrice - spending - CostCraft;
        }
        internal void ReserveItems(Dictionary<int, ItemPageParser> parsersByIdItem)
        {
            foreach (var idItem in Items.Keys)
            {
                foreach (var item in Items[idItem])
                {
                    parsersByIdItem[idItem].Remove(item);
                }
            }
        }
        internal double GetIncomeGoldInHour()
        {
            return ParseService.GetIncomeGoldInHour(
                NormalProfit + RecipeData.GetRandomProfit(),
                NeedMillisecondsToCraft);
        }
        internal double GetIncomeRUBInHour()
        {
            return ParseService.GetIncomeRUBInHour(
                NormalProfit + RecipeData.GetRandomProfit(),
                NeedMillisecondsToCraft);
        }

        internal double GetProfit()
        {
            return NormalProfit + RecipeData.GetRandomProfit();
        }
        //internal void SetNeedMillisecondsForLongCraft()
        //{
        //    NeedMillisecondsToCraft = RecipeData.GetNeedMillisecondsForLongCraft();
        //}
        internal void SetDefaultNeedMillisecondsToProfit()
        {
            NeedMillisecondsToCraft = RecipeData.GetNeedMillisecondsToGetProfit();
        }
    }
}