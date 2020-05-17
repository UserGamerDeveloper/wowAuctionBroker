using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
