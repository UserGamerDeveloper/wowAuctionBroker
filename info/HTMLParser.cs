using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace info
{
    class AuctionPageHTMLParser
    {
        int timeOutAHPage = 5000;
        HtmlDocument htmlDocument = new HtmlDocument();
        HtmlNodeCollection bidsHtmlNodeCollection;

        public AuctionPageHTMLParser(Uri uri, string cookie)
        {
            htmlDocument.LoadHtml(getAHPageHTML(uri, cookie));
            bidsHtmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='summary']");
        }

        public int getBidCount()
        {
            string[] countTemp = bidsHtmlNodeCollection[0].InnerText.Split(' ')[1].Split(',');
            string countTempa = "";
            foreach (var a in countTemp)
            {
                countTempa += a;
            }
            int count = Convert.ToInt32(countTempa);
            return count;
        }

        public long getCostBid(int idBid)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(String.Format("//tr[@data-key='{0}']", idBid));
            HtmlDocument bid = new HtmlDocument();
            bid.LoadHtml(htmlNodeCollection[0].InnerHtml);
            htmlNodeCollection = bid.DocumentNode.SelectNodes("//td[@data-col-seq='3']");
            string[] a = htmlNodeCollection[0].InnerText.Split(new char[] { 'g', 's', 'c', ' ', ',' });
            string costBidStr = "";
            foreach (var c in a)
            {
                costBidStr += c;
            }
            return Convert.ToInt64(costBidStr);
        }

        public int getCountInBid(int idBid)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(String.Format("//tr[@data-key='{0}']", idBid));
            HtmlDocument bid = new HtmlDocument();
            bid.LoadHtml(htmlNodeCollection[0].InnerHtml);
            htmlNodeCollection = bid.DocumentNode.SelectNodes("//td[@data-col-seq='0']");
            return Convert.ToInt32(htmlNodeCollection[0].InnerText);
        }

        public string getAutor(int idBid)
        {
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(String.Format("//tr[@data-key='{0}']", idBid));
            HtmlDocument bid = new HtmlDocument();
            bid.LoadHtml(htmlNodeCollection[0].InnerHtml);
            htmlNodeCollection = bid.DocumentNode.SelectNodes("//td[@data-col-seq='4']");
            return htmlNodeCollection[0].InnerText;
        }

        private string getAHPageHTML(Uri uri, string cookie)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                httpWebRequest.Timeout = timeOutAHPage;
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.CookieContainer.Add(new Cookie("realmId", cookie) { Domain = uri.Host });
                httpWebRequest.CookieContainer.Add(new Cookie("regionId", Program.regionidValue) { Domain = uri.Host });
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            timeOutAHPage = 5000;
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("Exception_AH_page.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                timeOutAHPage += 1000;
                return getAHPageHTML(uri, cookie);
            }
        }

        public bool hasBid()
        {
            return bidsHtmlNodeCollection != null;
        }
    }
}
