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

    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class Server : IComparable
    {
        const string URI_STRING = "https://theunderminejournal.com/api/house.php?house={0}";
        public const double AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA = 6d;
        const int timeOutDBPage = 5000;

        public int id;
        public string name;
        public long timeUpdate;
        public XmlSerializableDictionary<int, long> idRecipeAndSpending;
        [XmlIgnore]
        public List<RecipeData> recipes;
        [XmlIgnore]
        public List<HashSet<RecipeData>> recipeDataTrees = new List<HashSet<RecipeData>>();

        public Server() { }

        public Server(HouseId serverId, XmlSerializableDictionary<int, long> idRecipeAndSpending)
        {
            this.id = (int)serverId;
            this.idRecipeAndSpending = idRecipeAndSpending;
            this.name = serverId.ToString();
        }

        internal void SetRecipes(Dictionary<int, RecipeData> recipeData)
        {
            recipes = new List<RecipeData>();
            foreach (var idRecipe in idRecipeAndSpending.Keys)
            {
                recipes.Add(new RecipeData(recipeData[idRecipe], idRecipeAndSpending[idRecipe]));
            }
        }

        private string getHouse()
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(String.Format(URI_STRING, id));
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                httpWebRequest.Timeout = timeOutDBPage;
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            //timeOutDBPage = 2000;
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("Exception_house.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                //timeOutDBPage += 1000;
                //if (timeOutDBPage >= 10000)
                //{
                //    idTSMServer++;
                //    if (idTSMServer == 8)
                //    {
                //        idTSMServer = 4;
                //    }
                //}
                return getHouse();
            }
        }

        public long getTimeUpdate()
        {
            House house = JsonConvert.DeserializeObject<House>(getHouse());

            DateTime time = UnixTimeStampToDateTime(house.Timestamps.Lastupdate);

            if (time.AddMinutes(AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA).CompareTo(DateTime.Now) != -1)
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
            string str = String.Format("{2}\n{0} {1}", name, UnixTimeStampToDateTime(timeUpdate), DateTime.Now);
            Console.WriteLine(str);
            File.AppendAllText("log.txt", str + "\n");
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public int CompareTo(object obj)
        {
            Server server = obj as Server;
            return server.timeUpdate.CompareTo(timeUpdate);
        }

        internal string getUri()
        {
            return URI_STRING + name;
        }
    }
}