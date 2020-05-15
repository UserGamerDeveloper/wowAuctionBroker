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
    public class ItemData
    {
        public int id;
        public string itemName;
        public string uri;

        public ItemData() { }

        public ItemData(ItemId id, string itemName, string uri)
        {
            this.id = (int)id;
            this.itemName = itemName;
            this.uri = uri;
        }

        public Uri getUri()
        {
            return new Uri(uri);
        }
    }
}
