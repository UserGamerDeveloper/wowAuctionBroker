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
        [XmlIgnore]
        public long SPENDING;

        public RecipeData(RecipeId shimmerscale_Striker, XmlSerializableDictionary<int, int> dictionary, int SELL_PRICE, double TIME_NEED)
        {
            ID = (int)shimmerscale_Striker;
            ID_ITEM_AND_NEED_AMOUNT = dictionary;
            this.SELL_PRICE = SELL_PRICE;
            this.TIME_NEED = TIME_NEED;
        }

        public RecipeData(RecipeData recipeData, long SPENDING)
        {
            ID = recipeData.ID;
            ID_ITEM_AND_NEED_AMOUNT = recipeData.ID_ITEM_AND_NEED_AMOUNT;
            SELL_PRICE = recipeData.SELL_PRICE;
            TIME_NEED = recipeData.TIME_NEED;
            this.SPENDING = SPENDING;
        }

        RecipeData() { }

    }
}