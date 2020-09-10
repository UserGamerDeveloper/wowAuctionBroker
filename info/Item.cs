using System;

namespace info
{
    public class Item : IComparable
    {
        public long cost;

        public Item(long cost)
        {
            this.cost = cost;
        }

        public int CompareTo(object obj)
        {
            Item comparableItem = obj as Item;
            return cost.CompareTo(comparableItem.cost);
        }
    }
}