using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    class Item : IComparable
    {
        public string itemName;
        public long profit;
        [XmlIgnore]
        public List<Bid> bids = new List<Bid>();
        [XmlIgnore]
        public int count;
        public long costSell;
        public Uri uri;
        public double TIME_NEED;

        public Item(string itemName, long costSell, string uri, double TIME_NEED)
        {
            this.itemName = itemName;
            this.costSell = costSell;
            this.uri = new Uri(uri);
            this.TIME_NEED = TIME_NEED;
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
                    if ((bid.costPerItem == bids[0].costPerItem))
                    {
                        minCount++;
                    }
                    if ((bid.costPerItem == bids[bids.Count - 1].costPerItem) && bids[bids.Count - 1].autor.Equals(bid.autor))
                    {
                        maxCount++;
                    }
                }
                string printStr = String.Format("\t{0}\t{1:# ## ##} x{2}\t{3:# ## ##} x{4}\t{5}", itemName, bids[0].costPerItem, minCount, bids[bids.Count - 1].costPerItem, maxCount, bids[bids.Count - 1].autor);
                Console.WriteLine(printStr);
            }
        }

        public void log()
        {
            if (bids.Count > 0)
            {
                int minCount = 0;
                int maxCount = 0;
                foreach (var bid in bids)
                {
                    if ((bid.costPerItem == bids[0].costPerItem))
                    {
                        minCount++;
                    }
                    if ((bid.costPerItem == bids[bids.Count - 1].costPerItem) && bids[bids.Count - 1].autor.Equals(bid.autor))
                    {
                        maxCount++;
                    }
                }
                string printStr = String.Format("\t{0}\t{1:# ## ##} x{2}\t{3:# ## ##} x{4}\t{5}", itemName, bids[0].costPerItem, minCount, bids[bids.Count - 1].costPerItem, maxCount, bids[bids.Count - 1].autor);
                File.AppendAllText("log.txt", printStr + "\n");
            }
        }

    }
}
