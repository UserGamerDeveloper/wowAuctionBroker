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
        public long costPerItem;
        public long profit;
        public long profitPerItem;
        public string autor;

        public Bid(ItemType itemType, int count, long costPerItem, string autor, long profit)
        {
            this.itemType = itemType;
            this.count = count;
            this.costPerItem = costPerItem;
            this.profit = profit;
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
