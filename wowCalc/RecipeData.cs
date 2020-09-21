using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace info
{
    [Serializable]
    public class RecipeData
    {
        public const double BFANeedMillisecondsToCraft = 15000d / 70d;

        public int ID;
        public XmlSerializableDictionary<int,int> ID_ITEM_AND_NEED_AMOUNT;
        public long SellNormalPrice { get; set; }
        public long SellRandomPrice { get; set; }
        public bool DropToMail { get; }
        public double NeedMillisecondsToCraft;
        public string name;
        public long SPENDING;
        [XmlIgnore]
        public HashSet<ItemData> ItemsData = new HashSet<ItemData>();

        public RecipeData(
            RecipeInfo recipeInfo,
            XmlSerializableDictionary<int, int> dictionary,
            int SELL_PRICE, double TIME_NEED, long SPENDING, long sellRandomPrice, bool dropToMail = false)
        {
            ID = (int)recipeInfo;
            ID_ITEM_AND_NEED_AMOUNT = dictionary;
            this.SellNormalPrice = SELL_PRICE;
            this.NeedMillisecondsToCraft = TIME_NEED;
            this.SPENDING = SPENDING;
            name = recipeInfo.ToString();
            SellRandomPrice = sellRandomPrice;
            DropToMail = dropToMail;
        }

        RecipeData() { }

        internal void SetData(Dictionary<int, ItemData> itemsDataById)
        {
            foreach (var itemId in ID_ITEM_AND_NEED_AMOUNT.Keys)
            {
                ItemsData.Add(itemsDataById[itemId]);
            }
        }

        internal long GetRandomProfit()
        {
            return SellRandomPrice - SellNormalPrice;
        }
    }
}