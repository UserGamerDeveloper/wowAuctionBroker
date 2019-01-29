using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    class Recipe
    {
        public List<ItemType> itemList;
        public Dictionary<ItemType, int> coutItem;
        public long cost;

        public Recipe(List<ItemType> itemList, Dictionary<ItemType, int> coutItem, long cost)
        {
            this.itemList = itemList;
            this.coutItem = coutItem;
            this.cost = cost;
        }
    }
}
