using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace info
{
    [Serializable]
    public class Settings
    {
        public double TargetProfitInRUB = 10;
        public double TargetIncomeInHourInRub = 50;
        public string ClientId = "2822529a2c004a43896c9a67395969dd";
        public string ClientSecret = "8wBVDXSb3owSWimdkioCAHjVwAMo8Hq9";

        public Settings() { }
    }
}
