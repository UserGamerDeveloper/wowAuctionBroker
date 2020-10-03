using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    [Serializable]
    public class ItemData
    {
        public int id;
        public string itemName;

        public ItemData() { }

        public ItemData(ItemInfo itemInfo, string itemName)
        {
            this.id = (int)itemInfo;
            this.itemName = itemName;
        }
    }
}
