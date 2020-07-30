using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace info
{
    public class House
    {
        [JsonProperty("timestamps")]
        public Timestamps Timestamps { get; set; }
        [JsonProperty("mostAvailable")]
        public List<MostAvailable> MostAvailable { get; set; }
        [JsonProperty("deals")]
        public List<Deal> Deals { get; set; }
    }
    public class Timestamps
    {
        [JsonProperty("scheduled")]
        public int Scheduled { get; set; }
        [JsonProperty("delayednext")]
        public object Delayednext { get; set; }
        [JsonProperty("lastupdate")]
        public int Lastupdate { get; set; }
        [JsonProperty("mindelta")]
        public int Mindelta { get; set; }
        [JsonProperty("avgdelta")]
        public int Avgdelta { get; set; }
        [JsonProperty("maxdelta")]
        public int Maxdelta { get; set; }
        [JsonProperty("lastcheck")]
        public int Lastcheck { get; set; }
        [JsonProperty("lastsuccess")]
        public int Lastsuccess { get; set; }
    }
    public class MostAvailable
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("requiredside")]
        public string Requiredside { get; set; }
        [JsonProperty("name_enus")]
        public string NameEnus { get; set; }
        [JsonProperty("name_dede")]
        public string NameDede { get; set; }
        [JsonProperty("name_eses")]
        public string NameEses { get; set; }
        [JsonProperty("name_frfr")]
        public string NameFrfr { get; set; }
        [JsonProperty("name_itit")]
        public string NameItit { get; set; }
        [JsonProperty("name_ptbr")]
        public string NamePtbr { get; set; }
        [JsonProperty("name_ruru")]
        public string NameRuru { get; set; }
        [JsonProperty("name_zhtw")]
        public string NameZhtw { get; set; }
        [JsonProperty("name_kokr")]
        public string NameKokr { get; set; }
    }
    public class Deal
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("requiredside")]
        public string Requiredside { get; set; }
        [JsonProperty("name_enus")]
        public string NameEnus { get; set; }
        [JsonProperty("name_dede")]
        public string NameDede { get; set; }
        [JsonProperty("name_eses")]
        public string NameEses { get; set; }
        [JsonProperty("name_frfr")]
        public string NameFrfr { get; set; }
        [JsonProperty("name_itit")]
        public string NameItit { get; set; }
        [JsonProperty("name_ptbr")]
        public string NamePtbr { get; set; }
        [JsonProperty("name_ruru")]
        public string NameRuru { get; set; }
        [JsonProperty("name_zhtw")]
        public string NameZhtw { get; set; }
        [JsonProperty("name_kokr")]
        public string NameKokr { get; set; }
    }

    public class RealmData
    {
        public long moneyMax { get; set; }
        public long money { get; set; }
        public Reputation reputation { get; set; }
    }

    public class TokenAndRealmsDatas
    {
        public Dictionary<int, RealmData> realmsDatasByIdHouse { get; set; }
        public long tokenPrice { get; set; }
    }

    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class Server
    {
        class ComparerByTime : IComparer<Server>
        {
            public int Compare(Server s1, Server s2)
            {
                return s1.timeUpdate.CompareTo(s2.timeUpdate);
            }
        }
        class ComparerByMoney : IComparer<Server>
        {
            public int Compare(Server s1, Server s2)
            {
                return s2.money.CompareTo(s1.money);
            }
        }

        const string URI_FORMAT = "https://theunderminejournal.com/api/house.php?house={0}";

        public int id;
        public string name;
        public long timeUpdate;
        public List<int> idRecipes;
        [XmlIgnore]
        public List<RecipeData> recipes;
        [XmlIgnore]
        public List<HashSet<RecipeData>> recipeDataTrees = new List<HashSet<RecipeData>>();
        [XmlIgnore]
        public long money;
        [XmlIgnore]
        public long moneyMax;
        [XmlIgnore]
        public Reputation reputation;

        public Server() { }

        public Server(HouseId serverId, List<int> idRecipes)
        {
            this.id = (int)serverId;
            this.idRecipes = idRecipes;
            this.name = serverId.ToString();
        }

        internal void SetRecipes(Dictionary<int, RecipeData> recipeData)
        {
            recipes = new List<RecipeData>();
            foreach (var idRecipe in idRecipes)
            {
                recipes.Add(recipeData[idRecipe]);
            }
        }

        public long getTimeUpdate()
        {
            House house = JsonConvert.DeserializeObject<House>(Util.GetResponse(String.Format(URI_FORMAT, id), "Exception_house.txt"));

            DateTime time = Util.UnixTimeStampToDateTime(house.Timestamps.Lastupdate);

            if (time.AddMinutes(Util.AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA).CompareTo(DateTime.Now) != -1)
            {
                return timeUpdate;
            }
            else
            {
                return new DateTimeOffset(time).ToUnixTimeSeconds();
            }
        }

        public bool hasUpdate()
        {
            long oldTime = timeUpdate;
            setTimeUpdate();
            return timeUpdate != oldTime;
        }

        public void setTimeUpdate()
        {
            timeUpdate = getTimeUpdate();
        }

        public void printAndLog()
        {
            string str = String.Format("{2}\n{0} {1}", name, Util.UnixTimeStampToDateTime(timeUpdate), DateTime.Now);
            Console.WriteLine(str);
            File.AppendAllText("log.txt", str + "\n");
        }

        internal static void SortByTime(Server[] servers)
        {
            Array.Sort(servers, new ComparerByTime());
        }

        internal static void SortByMoney(Server[] servers)
        {
            Array.Sort(servers, new ComparerByMoney());
        }

        internal string getUri()
        {
            return URI_FORMAT + name;
        }

        internal void SetData()
        {
            string s = File.ReadAllText(@"C:\Games\World of Warcraft\_retail_\WTF\Account\449681846#1\SavedVariables\getGoldAndRep.lua");
            s = s.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("DB = ", "").Replace("[", "").Replace("]", "").Replace("=", ":");
            TokenAndRealmsDatas tokenAndRealmsDatas = JsonConvert.DeserializeObject<TokenAndRealmsDatas>(s);
            RealmData realmData = tokenAndRealmsDatas.realmsDatasByIdHouse[id];
            money = realmData.money;
            reputation = realmData.reputation;
            moneyMax = realmData.moneyMax;
        }

        internal void Print()
        {
            long deltaMoney = moneyMax - money;
            string s = GetStringMitTab(name);
            Console.WriteLine(
                "{0}{1}{2:#,###}",
                s,
                GetStringMitTab(String.Format("{0:#,###}", Util.convertAndFloorCopperToGold(money))),
                Util.convertAndFloorCopperToGold(deltaMoney));
        }

        private string GetStringMitTab(string str)
        {
            int count = str.Length;
            for (int i = 0; i < 20 - count; i++)
            {
                str += " ";
            }

            return str;
        }
    }
}