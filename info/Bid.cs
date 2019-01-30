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
        public double timeNeed;

        public Bid(ItemType itemType, int count, long costPerItem, string autor, long profit, double timeNeedPerItem)
        {
            this.itemType = itemType;
            this.count = count;
            this.costPerItem = costPerItem;
            this.profit = profit;
            profitPerItem = profit / count;
            this.autor = autor;
            timeNeed = timeNeedPerItem * count;
        }

        public int CompareTo(object obj)
        {
            Bid bid = obj as Bid;
            double income = profit / timeNeed;
            double incomee = bid.profit / bid.timeNeed;
            return income.CompareTo(incomee);
        }
    }
}
