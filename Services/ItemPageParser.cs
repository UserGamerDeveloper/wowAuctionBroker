using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using static wowCalc.AuctionData;

namespace info
{
    public class ItemPageParser
    {
        private readonly List<Item> items = new List<Item>();
        private readonly List<Auction> itemPage;
        private int idBid;

        public ItemPageParser(List<Auction> auctions)
        {
            itemPage = auctions/*.FindAll(x => x.UnitPrice > 0)*/.OrderByDescending(x => x.UnitPrice).ToList();
            idBid = itemPage.Count - 1;
        }

        internal bool HasRequiredAmount(int amount)
        {
            if (items.Count >= amount)
            {
                return true;
            }
            else
            {
                while (items.Count < amount)
                {
                    if (idBid >= 0)
                    {
                        for (int j = 0; j < itemPage[idBid].Quantity; j++)
                        {
                            items.Add(new Item(itemPage[idBid].UnitPrice));
                        }
                        idBid--;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal Item GetItem(int id)
        {
            return items[id];
        }

        internal void Remove(Item item)
        {
            items.Remove(item);
        }
    }
}