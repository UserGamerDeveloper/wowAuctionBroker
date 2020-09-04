using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace info
{
    [Serializable]
    public class RecipeData
    {
        public int ID;
        public XmlSerializableDictionary<int,int> ID_ITEM_AND_NEED_AMOUNT;
        public long SELL_PRICE;
        public double TIME_NEED;
        public string name;
        public long SPENDING;
        [XmlIgnore]
        public HashSet<ItemData> ItemsData = new HashSet<ItemData>();

        public RecipeData(RecipeInfo recipeInfo, XmlSerializableDictionary<int, int> dictionary, int SELL_PRICE, double TIME_NEED, long SPENDING)
        {
            ID = (int)recipeInfo;
            ID_ITEM_AND_NEED_AMOUNT = dictionary;
            this.SELL_PRICE = SELL_PRICE;
            this.TIME_NEED = TIME_NEED;
            this.SPENDING = SPENDING;
            name = recipeInfo.ToString();
        }

        RecipeData() { }

        internal void SetData(Dictionary<int, ItemData> itemsDataById)
        {
            foreach (var itemId in ID_ITEM_AND_NEED_AMOUNT.Keys)
            {
                ItemsData.Add(itemsDataById[itemId]);
            }
        }
    }
}