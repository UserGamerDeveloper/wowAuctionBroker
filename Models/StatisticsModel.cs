using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public class StatisticsModel
    {
        public string RealmName { get; set; }
        public List<StatisticFaction> statisticFactions { get; set; } = new List<StatisticFaction>();
        public string LastUpdate { get; set; }
        public bool FarmMode { get; set; }
    }
    public class StatisticFaction
    {
        public string FractionName;
        public string Money { get; set; }
        public string WaitMoney { get; set; }
    }
}
