using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace info
{
    class ItemPageParser
    {
        public class ItemPage
        {
            public class Globalmonthly
            {
                [JsonProperty("date")]
                public string Date { get; set; }
                [JsonProperty("silver")]
                public double Silver { get; set; }
                [JsonProperty("quantity")]
                public int Quantity { get; set; }
            }
            public class Globalnow
            {
                [JsonProperty("house")]
                public int House { get; set; }
                [JsonProperty("price")]
                public long Price { get; set; }
                [JsonProperty("quantity")]
                public int Quantity { get; set; }
                [JsonProperty("lastseen")]
                public int Lastseen { get; set; }
            }
            public class History
            {
                [JsonProperty("snapshot")]
                public int Snapshot { get; set; }
                [JsonProperty("silver")]
                public int Silver { get; set; }
                [JsonProperty("quantity")]
                public int Quantity { get; set; }
            }
            public class Stat
            {
                [JsonProperty("id")]
                public int Id { get; set; }
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
                [JsonProperty("icon")]
                public string Icon { get; set; }
                [JsonProperty("display")]
                public object Display { get; set; }
                [JsonProperty("classid")]
                public int Classid { get; set; }
                [JsonProperty("subclass")]
                public int Subclass { get; set; }
                [JsonProperty("quality")]
                public int Quality { get; set; }
                [JsonProperty("baselevel")]
                public int Baselevel { get; set; }
                [JsonProperty("stacksize")]
                public int Stacksize { get; set; }
                [JsonProperty("binds")]
                public int Binds { get; set; }
                [JsonProperty("buyfromvendor")]
                public int Buyfromvendor { get; set; }
                [JsonProperty("selltovendor")]
                public int Selltovendor { get; set; }
                [JsonProperty("auctionable")]
                public int Auctionable { get; set; }
                [JsonProperty("requiredside")]
                public string Requiredside { get; set; }
                [JsonProperty("price")]
                public long Price { get; set; }
                [JsonProperty("quantity")]
                public int Quantity { get; set; }
                [JsonProperty("lastseen")]
                public string Lastseen { get; set; }
                [JsonProperty("level")]
                public int Level { get; set; }
                [JsonProperty("vendorprice")]
                public object Vendorprice { get; set; }
                [JsonProperty("vendornpc")]
                public object Vendornpc { get; set; }
                [JsonProperty("vendornpccount")]
                public object Vendornpccount { get; set; }
            }
            public class Daily
            {
                [JsonProperty("date")]
                public string Date { get; set; }
                [JsonProperty("silvermin")]
                public int Silvermin { get; set; }
                [JsonProperty("silveravg")]
                public int Silveravg { get; set; }
                [JsonProperty("silvermax")]
                public int Silvermax { get; set; }
                [JsonProperty("silverstart")]
                public int Silverstart { get; set; }
                [JsonProperty("silverend")]
                public int Silverend { get; set; }
                [JsonProperty("quantitymin")]
                public int Quantitymin { get; set; }
                [JsonProperty("quantityavg")]
                public int Quantityavg { get; set; }
                [JsonProperty("quantitymax")]
                public int Quantitymax { get; set; }
            }
            public class Auctions
            {
                public class Data : IComparable
                {
                    [JsonProperty("id")]
                    public int Id { get; set; }
                    [JsonProperty("quantity")]
                    public int Quantity { get; set; }
                    [JsonProperty("bid")]
                    public int Bid { get; set; }
                    [JsonProperty("buy")]
                    public long Buy { get; set; }
                    [JsonProperty("rand")]
                    public int Rand { get; set; }
                    [JsonProperty("seed")]
                    public int Seed { get; set; }
                    [JsonProperty("lootedlevel")]
                    public int Lootedlevel { get; set; }
                    [JsonProperty("level")]
                    public int Level { get; set; }
                    [JsonProperty("bonuses")]
                    public object Bonuses { get; set; }
                    [JsonIgnore]
                    public long costPerItem { get; set; }

                    public int CompareTo(object obj)
                    {
                        Data data = obj as Data;
                        return data.costPerItem.CompareTo(costPerItem);
                    }

                    public void setCostPerItem()
                    {
                        costPerItem = Buy / Quantity;
                    }
                }

                [JsonProperty("data")]
                public List<Data> data { get; set; }
                [JsonProperty("hydrate")]
                public List<object> Hydrate { get; set; }

                public void deleteInvalidDatas()
                {
                    List<Data> invalidDatas = new List<Data>();
                    foreach (var item in data)
                    {
                        if (item.Quantity == 0)
                        {
                            invalidDatas.Add(item);
                        }
                    }
                    foreach (var item in invalidDatas)
                    {
                        data.Remove(item);
                    }
                }
            }

            [JsonProperty("stats")]
            public List<Stat> Stats { get; set; }
            [JsonProperty("history")]
            public List<History> history { get; set; }
            [JsonProperty("daily")]
            public List<Daily> daily { get; set; }
            [JsonProperty("monthly")]
            public List<Globalmonthly> Monthly { get; set; }
            [JsonProperty("auctions")]
            public Auctions auctions { get; set; }
            [JsonProperty("globalnow")]
            public List<Globalnow> globalnow { get; set; }
            [JsonProperty("globalmonthly")]
            public List<Globalmonthly> globalmonthly { get; set; }
            [JsonProperty("region")]
            public string Region { get; set; }
        }

        public class Response
        {
            [JsonProperty("captcha")]
            public Captcha Captcha { get; set; }
        }
        public class Captcha
        {
            [JsonProperty("lookfor")]
            public Race Lookfor { get; set; }
            [JsonProperty("ids")]
            public List<long> Ids { get; set; }
        }
        public enum Race
        {
            Undead = 5,
            Trolls = 8,
            Tauren = 6,
            Orcs = 2,
            Night_Elves = 4,
            Humans = 1,
            Goblins = 9,
            Gnomes = 7,
            Dwarves = 3,
            Draenei = 11,
            Blood_Elves = 10
        }

        const string URL_ITEM_PAGE_FORMAT = "https://theunderminejournal.com/api/item.php?house={0}&item={1}";
        const string URL_CAPTCHA_ANSWER_FORMAT = "https://theunderminejournal.com/api/captcha.php?answer={0}";
        const string URL_CAPTCHA_IMAGES_FORMAT = "https://theunderminejournal.com/captcha/{0}.jpg";
        const int timeOutDBPage = 5000;

        static object locker = new object();
        ItemPage itemPage;
        List<Item> items = new List<Item>();
        int idBid;

        public ItemPageParser(int house, int idItem)
        {
            lock (locker)
            {
                while (true)
                {
                    string responseStr = Util.GetResponse(
                        String.Format(URL_ITEM_PAGE_FORMAT, house, idItem),
                        "Exception_house.txt").Replace("[[", "[").Replace("]]", "]");
                    itemPage = JsonConvert.DeserializeObject<ItemPage>(responseStr);
                    bool isCaptcha = itemPage.auctions == null;
                    if (!isCaptcha)
                    {
                        if (itemPage.auctions.data.Count > 0)
                        {
                            itemPage.auctions.deleteInvalidDatas();
                            foreach (var item in itemPage.auctions.data)
                            {
                                item.setCostPerItem();
                            }
                            itemPage.auctions.data.Sort();
                        }
                        idBid = itemPage.auctions.data.Count - 1;
                        break;
                    }
                    else
                    {
                        const string CAPTCHA = "\t\t\t Капча ";
                        const string SUCCESS = "пройдена";
                        const string DENIED = "не пройдена";
                        const string DIRECTORY = "Captcha";

                        Dictionary<Race, HashSet<string>> imagesHashesByRace =
                            JsonConvert.DeserializeObject<Dictionary<Race, HashSet<string>>>(Util.ReadFile("captcha_hashes.txt"));
                        SoundPlayer simpleSound = new SoundPlayer("music.wav");
                        bool captchaSuccess = false;
                        Response response = null;
                        List<string> images = null;
                        while (!captchaSuccess)
                        {
                            Util.WriteAndLog(CAPTCHA);
                            string answer = "";
                            string answerFormat = "";
                            images = new List<string>();
                            response = JsonConvert.DeserializeObject<Response>(responseStr);
                            Race race = response.Captcha.Lookfor;
                            Directory.CreateDirectory(DIRECTORY);
                            using (WebClient client = new WebClient())
                            {
                                int i = 1;
                                foreach (var id in response.Captcha.Ids)
                                {
                                    string image = String.Format("{0}/{1}.jpg", DIRECTORY, i);
                                    images.Add(image);
                                    client.DownloadFile(String.Format(URL_CAPTCHA_IMAGES_FORMAT, id), image);
                                    string hash = ComputeMD5Checksum(image);
                                    if (imagesHashesByRace[race].Contains(hash))
                                    {
                                        answer += i;
                                        answerFormat += i + " ";
                                    }
                                    i++;
                                }
                            }
                            string urlCaptchaAnswer = String.Format(URL_CAPTCHA_ANSWER_FORMAT, answer);
                            responseStr = Util.GetResponse(urlCaptchaAnswer, "Exception_captcha.txt");
                            captchaSuccess = responseStr.Equals("[]");
                            if (!captchaSuccess)
                            {
                                Util.WriteLineAndLog(DENIED);
                                Directory.Move(DIRECTORY, String.Format("{0} {1}{2}", race.ToString(), answerFormat, DateTime.Now.ToString().Replace(":", " ")));
                            }
                            else
                            {
                                Util.WriteLineAndLog(SUCCESS);
                                foreach (var image in images)
                                {
                                    Util.DeleteFile(image);
                                }
                                Directory.Delete(DIRECTORY);
                            }
                        }
                    }
                }
            }
        }

        private string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] checkSum = md5.ComputeHash(fs);
                string result = BitConverter.ToString(checkSum);
                return result;
            }
        }

        //private string getItemPage(string url)
        //{
        //    try
        //    {
        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
        //        httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
        //        httpWebRequest.Timeout = timeOutDBPage;
        //        //httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
        //        using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
        //        {
        //            using (var stream = httpWebResponse.GetResponseStream())
        //            {
        //                using (var reader = new StreamReader(stream, Encoding.UTF8))
        //                {
        //                    //timeOutDBPage = 2000;
        //                    return reader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        File.WriteAllText("Exception_house.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
        //        return getItemPage(url);
        //    }
        //}

        public long getCostPerItem()
        {
            return itemPage.auctions.data[idBid].Buy / itemPage.auctions.data[idBid].Quantity;
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
                        for (int j = 0; j < itemPage.auctions.data[idBid].Quantity; j++)
                        {
                            items.Add(new Item(getCostPerItem()));
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