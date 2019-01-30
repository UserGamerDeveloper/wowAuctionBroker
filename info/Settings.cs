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
        public int TARGET_PROFIT;
        public int count_IN_STACK;
        public int TARGET_INCOME_IN_HOUR;

        public Settings() { }
    }
}
