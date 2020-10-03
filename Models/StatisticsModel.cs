using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public class StatisticsModel
    {
        public string Name { get; set; }
        public string Money { get; set; }
        public string WaitMoney { get; set; }
        public string LastUpdate { get; set; }
        public bool FarmMode { get; set; }
    }
}
