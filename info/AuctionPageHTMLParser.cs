using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;

namespace info
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
        public int Lookfor { get; set; }
        [JsonProperty("ids")]
        public List<long> Ids { get; set; }
    }

    class AuctionPageHTMLParser
    {
        enum Race
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

        const string URL = "https://theunderminejournal.com/api/item.php?house={0}&item={1}";
        const int timeOutDBPage = 5000;

        ItemPage itemPage;
        List<Item> items = new List<Item>();
        int idBid;

        public AuctionPageHTMLParser(int house, int idItem)
        {
            Construct(house, idItem);
        }

        private void Construct(int house, int idItem)
        {
            string responseStr = getItemPage(String.Format(URL, house, idItem)).Replace("[[", "[").Replace("]]", "]");
            itemPage = JsonConvert.DeserializeObject<ItemPage>(responseStr);
            if (itemPage.auctions != null)
            {
                if (itemPage.auctions.data.Count > 0)
                {
                    idBid = itemPage.auctions.data.Count - 1;
                    foreach (var item in itemPage.auctions.data)
                    {
                        item.setCostPerItem();
                    }
                    itemPage.auctions.data.Sort();
                }
                else
                {
                    throw new NotImplementedException("нет бидов");
                }
                Thread.Sleep(1000);
            }
            else
            {
                Response response = JsonConvert.DeserializeObject<Response>(responseStr);

                const string url = "https://theunderminejournal.com/captcha/{0}.jpg";
                List<string> images = new List<string>();
                using (WebClient client = new WebClient())
                {
                    int i = 1;
                    foreach (var id in response.Captcha.Ids)
                    {
                        string image = String.Format("Captcha/{0}.jpg", i);
                        images.Add(image);
                        client.DownloadFile(String.Format(url, id), image);
                        i++;
                    }
                }

                Race race;
                Enum.TryParse<Race>(response.Captcha.Lookfor.ToString(), out race);
                //switch (race)
                //{
                //    case Races.Blood_Elves:
                //        {
                //            Console.WriteLine(race.ToString());
                //            break;
                //        }
                //    default:
                //        break;
                //}

                System.Diagnostics.Process.Start("ImageGallery.exe", race.ToString());

                const string CAPTCHA = "\t\t\t\t Капча";
                Console.WriteLine(CAPTCHA);
                File.AppendAllText("log.txt", CAPTCHA + "\n");
                SoundPlayer simpleSound = new SoundPlayer("music.wav");
                simpleSound.PlayLooping();
                //bool captchaFalse = true;
                //while (captchaFalse)
                //{
                //    Util.DeleteFile();
                //}
                Console.ReadLine();
                simpleSound.Stop();
                foreach (var image in images)
                {
                    Util.DeleteFile(image);
                }
                Construct(house, idItem);
            }
        }

        private string getItemPage(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                httpWebRequest.Timeout = timeOutDBPage;
                //httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
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
                return getItemPage(url);
            }
        }

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