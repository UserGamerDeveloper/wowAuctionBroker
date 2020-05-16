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
            return getProfitInGold(profit) / getTimeInSeconds(timeNeed) * 3600f;
        }

        public static double getProfitInGold(long profit_all)
        {
            return profit_all / 10000f;
        }

        public static double getTimeInSeconds(DateTime tempTimeNeed)
        {
            return tempTimeNeed.Millisecond / 1000f + tempTimeNeed.Second * 1f + tempTimeNeed.Minute * 60f + tempTimeNeed.Hour * 3600f;
        }

        public static long convertToSilver(long cost)
        {
            return (long)Math.Floor(cost / 100f);
        }

        public static double getTimeInMinuts(DateTime timeNeed)
        {
            return getTimeInSeconds(timeNeed) / 60f;
        }
    }
}
