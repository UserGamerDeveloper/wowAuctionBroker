using System;

namespace info
{
    public class Item : IComparable
    {
        int id;
        public long cost;

        public Item(int id, long cost)
        {
            this.id = id;
            this.cost = cost;
        }

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