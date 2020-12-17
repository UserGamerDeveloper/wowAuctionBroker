using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public class FactionStatisticsModel
    {
        public string RealmName { get; set; }
        public string LastUpdate { get; set; }
        public bool FarmMode { get; set; }
        public string FractionName { get; set; }
        public double Gold { get; set; }
        public double WaitMoney { get; set; }
        public double MaxGold { get; set; }
    }
    public class StatisticsModel
    {
        public List<FactionStatisticsModel> Factions = new List<FactionStatisticsModel>();
        public double AllMoney { get; set; }
    }
}
