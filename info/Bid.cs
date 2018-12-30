using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    class Bid : IComparable
    {
        public ItemType itemType;
        public int count;
        public long cost;
        public int profit;
        public int profitPerItem;
        public string autor;

        public Bid(ItemType itemType, int count, long cost, string autor)
        {
            this.itemType = itemType;
            this.count = count;
            this.cost = cost;
            profit = (int)(Program.costsItem[itemType] - cost) * count;
            profitPerItem = profit / count;
            this.autor = autor;
        }

        public int CompareTo(object obj)
        {
            Bid bid = obj as Bid;
            return profitPerItem.CompareTo(bid.profitPerItem);
        }
    }
}
