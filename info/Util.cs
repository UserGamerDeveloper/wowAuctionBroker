using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace info
{
    static class Util
    {
        public static double getIncomeGoldInHour(long profit, DateTime timeNeed)
        {
            return convertCopperToGold(profit / getTimeInSeconds(timeNeed) * 3600f);
        }

        public static double convertCopperToGold(double profit_all)
        {
            return profit_all / 10000f;
        }

        public static double getTimeInSeconds(DateTime tempTimeNeed)
        {
            return tempTimeNeed.Millisecond / 1000f + tempTimeNeed.Second * 1f + tempTimeNeed.Minute * 60f + tempTimeNeed.Hour * 3600f;
        }

        public static long convertCopperToSilver(long value)
        {
            return (long)Math.Floor(value / 100f);
        }

        public static double getTimeInMinuts(DateTime timeNeed)
        {
            return getTimeInSeconds(timeNeed) / 60f;
        }

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

        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                return ReadFile(path);
            }
        }
    }
}
