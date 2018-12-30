using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    class Item : IComparable
    {
        public string itemName;
        public int profit;
        public List<Bid> bids = new List<Bid>();
        public int count;
        public long costSell;
        public Uri uri;

        public Item(string itemName, long costSell, string uri)
        {
            this.itemName = itemName;
            this.costSell = costSell;
            this.uri = new Uri(uri);
        }

        public int CompareTo(object obj)
        {
            Item item = obj as Item;
            return getAverageProfit().CompareTo(item.getAverageProfit());
        }

        public double getAverageProfit()
        {
            return profit / 10000f / count;
        }

        public void print()
        {
            if (bids.Count > 0)
            {
                int minCount = 0;
                int maxCount = 0;
                foreach (var bid in bids)
                {
                    if ((bid.cost == bids[0].cost))
                    {
                        minCount++;
                    }
                    if ((bid.cost == bids[bids.Count - 1].cost) && bids[bids.Count - 1].autor.Equals(bid.autor))
                    {
                        maxCount++;
                    }
                }
                string printStr = itemName + "               " + String.Format("{0:# ## ##} x{1}", bids[0].cost, minCount) + "             " + String.Format("{0:# ## ##} x{1}    {2}", bids[bids.Count - 1].cost, maxCount, bids[bids.Count - 1].autor);
                Console.WriteLine(printStr);
                File.AppendAllText("log.txt", printStr + "\n");
            }
        }
    }
}
