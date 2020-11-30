using Mvc.Client.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace info
{
    [Serializable]
    public class RecipeData
    {
        public const double NeedMillisecondsToSell = 22000d / 136d;
        public const double NeedMillisecondsDropToMail = 48000d / 136d;
        //public const double DropToMailNeedMillisecondsToGetProfit = DropToMailNeedMillisecondsToCraft + NeedMillisecondsToSell;
        private const double ChanceRandomProfit = 0.177637947725073d;

        public int ID;
        public XmlSerializableDictionary<int,int> ID_ITEM_AND_NEED_AMOUNT;
        public long SellNormalPrice { get; set; }
        public long SellRandomPrice { get; set; }
        public bool DropToMail { get; }
        public double NeedMillisecondsToCraft;
        public string Name { get; set; }
        public long SPENDING;
        public FactionType Faction;
        [XmlIgnore]
        public HashSet<ItemData> ItemsData = new HashSet<ItemData>();

        public RecipeData(
            RecipeInfo recipeInfo,
            XmlSerializableDictionary<int, int> dictionary,
            int SELL_PRICE, double TIME_NEED, long SPENDING, long sellRandomPrice, FactionType faction = FactionType.NONE, bool dropToMail = false)
        {
            ID = (int)recipeInfo;
            ID_ITEM_AND_NEED_AMOUNT = dictionary;
            this.SellNormalPrice = SELL_PRICE;
            this.NeedMillisecondsToCraft = TIME_NEED;
            this.SPENDING = SPENDING;
            Name = recipeInfo.ToString();
            SellRandomPrice = sellRandomPrice;
            Faction = faction;
            DropToMail = dropToMail;
        }
        public RecipeData(
            RecipeInfo recipeInfo,
            XmlSerializableDictionary<int, int> dictionary,
            int SELL_PRICE, double NeedMillisecondsToCraft, long SPENDING, FactionType faction = FactionType.NONE, bool dropToMail = false)
        {
            ID = (int)recipeInfo;
            ID_ITEM_AND_NEED_AMOUNT = dictionary;
            this.SellNormalPrice = SELL_PRICE;
            this.NeedMillisecondsToCraft = NeedMillisecondsToCraft;
            this.SPENDING = SPENDING;
            Name = recipeInfo.ToString();
            SellRandomPrice = SellNormalPrice;
            Faction = faction;
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

        internal double GetRandomProfit()
        {
            return (SellRandomPrice - SellNormalPrice) * ChanceRandomProfit;
        }

        internal double GetNeedMillisecondsToGetProfit()
        {
            return NeedMillisecondsToCraft;
        }
        //internal double GetNeedMillisecondsForLongCraft()
        //{
        //    if (DropToMail)
        //    {
        //        return NeedMillisecondsToSell + NeedMillisecondsDropToMail;
        //    }
        //    else
        //    {
        //        throw new Exception("recipe not DropToMail");
        //    }
        //}
    }
}