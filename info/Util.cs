using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace info
{
    static class Util
    {
        class Maintenance
        {
            [JsonProperty("maintenance")]
            public int TimeEnd { get; set; }
        }

        private const int TimeOut = 5000;
        public const double AMOUNT_MINUTS_FOR_GET_ACTUAL_DATA = 1d;
        private const int AmountCopperInGold = 10000;
        private const int AmountCopperInSilver = 100;

        public static double GetIncomeGoldInHour(double profit, TimeSpan timeSpan)
        {
            if (timeSpan.Ticks != 0)
            {
                return ConvertCopperToGold(profit / timeSpan.Ticks * TimeSpan.TicksPerHour);
            }
            else
            {
                return double.NaN;
            }
        }

        public static double ConvertCopperToGold(double copper)
        {
            return copper / AmountCopperInGold;
        }

        //public static double getTimeInSeconds(DateTime tempTimeNeed)
        //{
        //    return tempTimeNeed.Millisecond / 1000f + tempTimeNeed.Second * 1f + tempTimeNeed.Minute * 60f + tempTimeNeed.Hour * 3600f;
        //}

        public static double ConvertCopperToSilver(double copper)
        {
            return copper / AmountCopperInSilver;
        }

        //public static double getTimeInMinuts(DateTime timeNeed)
        //{
        //    return getTimeInSeconds(timeNeed) / 60f;
        //}

        //public static void KillGoogleChrome()
        //{
        //    string strCmdText = "/C taskkill /im chrome.exe /f";
        //    RunProcess(strCmdText);
        //}

        //public static void RunProcess(string strCmdText)
        //{
        //    System.Diagnostics.Process process = new System.Diagnostics.Process();
        //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        //    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //    startInfo.FileName = "cmd.exe";
        //    startInfo.Arguments = strCmdText;
        //    process.StartInfo = startInfo;
        //    process.Start();

        //    //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        //}

        //public static string getResponse(string path)
        //{
        //    string responce = ReadFile(path);
        //    Util.KillGoogleChrome();
        //    DeleteFile(path);
        //    return responce;
        //}

        //public static void DeleteFile(string path)
        //{
        //    try
        //    {
        //        File.Delete(path);
        //    }
        //    catch (Exception e)
        //    {
        //        File.AppendAllText("Exception.txt", DateTime.Now.ToString() + "\n" + e.ToString() + "\n\n");
        //    }
        //}

        //public static string ReadFile(string path)
        //{
        //    bool readed = false;
        //    while (!readed)
        //    {
        //        try
        //        {
        //            string data = File.ReadAllText(path);
        //            readed = true;
        //            return data;
        //        }
        //        catch (Exception)
        //        {
        //            Thread.Sleep(1000);
        //        }
        //    }
        //    return null;
        //}

        public static void WriteAndLog(string str)
        {
            Console.Write(str);
            File.AppendAllText("log.txt", str);
        }

        public static void WriteLineAndLog(string str)
        {
            Console.WriteLine(str);
            File.AppendAllText("log.txt", str + "\n");
        }
        public static void WriteLineAndLogWhithTime(string str)
        {
            Console.WriteLine(DateTime.Now + "\n" + str);
            File.AppendAllText("log.txt", DateTime.Now + "\n"+str + "\n");
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void ExceptionLogAndAlert(Exception e)
        {
            Exception ex = e;
            string log = DateTime.Now.ToString() + "\n";
            while (ex != null)
            {
                log += string.Format("{0} {1} \n", ex.TargetSite.Name, ex.Message);
                ex = ex.InnerException;
            }
            File.AppendAllText("Exception.txt", log + e.StackTrace + "\n\n");
            Console.WriteLine("Надо перезагрузить\n");
            SoundPlayer alert = new SoundPlayer("music.wav");
            alert.PlayLooping();
            Console.ReadLine();
        }

        static public string GetResponse(string url, string pathLogFile)
        {
            string response = null;
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.AllowAutoRedirect = false;
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = TimeOut;
                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var stream = httpWebResponse.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                //timeOutDBPage = 2000;
                                response = reader.ReadToEnd();
                                //Thread.Sleep(1000);
                                break;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    try
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            Maintenance maintenance = JsonConvert.DeserializeObject<Maintenance>(reader.ReadToEnd());
                            DateTime dateTime = UnixTimeStampToDateTime(maintenance.TimeEnd);
                            Util.WriteLineAndLogWhithTime(string.Format("\n\t\t Тех. работы до {0} \n", dateTime));
                            throw new Exception(string.Format("Тех. работы до {0}", dateTime));
                        }
                    }
                    catch (NullReferenceException)
                    {
                        //Thread.Sleep(1000);
                    }
                    //catch (Exception e)
                    //{
                    //    //File.WriteAllText(pathLogFile, DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                    //    throw new Exception(url, e);
                    //}
                }
                //catch (Exception e)
                //{
                //    //File.WriteAllText(pathLogFile, DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                //    throw e;
                //}
            }
            return response;
        }

        static public void WaitEndMaintenance()
        {
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://theunderminejournal.com/api/item.php?house=147&item=152576");
                    httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                    httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                    httpWebRequest.Timeout = TimeOut;
                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        Thread.Sleep(1000);
                        break;
                    }
                }
                catch (WebException ex)
                {
                    try
                    {
                        using (var stream = ex.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            Maintenance maintenance = JsonConvert.DeserializeObject<Maintenance>(reader.ReadToEnd());
                            DateTime dateTime = UnixTimeStampToDateTime(maintenance.TimeEnd);
                            Util.WriteLineAndLogWhithTime(String.Format("\n\t\t Тех. работы до {0} \n", dateTime));
                            if (dateTime.CompareTo(DateTime.Now) != -1)
                            {
                                while (dateTime.CompareTo(DateTime.Now) != -1)
                                {
                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                Thread.Sleep(60000);
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Thread.Sleep(1000);
                    }
                    //catch (Exception e)
                    //{
                    //    //File.WriteAllText(pathLogFile, DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                    //    throw e;
                    //}

                }
                //catch (Exception e)
                //{
                //    //File.WriteAllText(pathLogFile, DateTime.Now.ToString() + "\n" + e.ToString() + "\n");
                //    throw e;
                //}
            }
        }
    }
}
