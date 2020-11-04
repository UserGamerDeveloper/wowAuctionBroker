using info;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wowCalc;

namespace Mvc.Client.Models
{
    public class CalculatorModel
    {
        public int RequiredIncomeGold { get; set; } = 0;
        public Dictionary<int, long> Items { get; set; } = new Dictionary<int, long>();
        public int SelectedItemID { get; set; } = int.MinValue;
        public FactionType SelectedFaction { get; set; } = FactionType.NONE;
        public int SelectedRealmId { get; set; } = -1;
        public List<SelectListItem> ReamlsNameSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> FactionSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RecipeTreeNameSelectList { get; internal set; } = new List<SelectListItem>();
        public HashSet<ItemData> ItemList { get; internal set; } = new HashSet<ItemData>();
        public Dictionary<string, string> Result { get; internal set; } = new Dictionary<string, string>();
        public string LastSelectedRealmName { get; internal set; }

        internal double GetTargetIncomeCopperInMillisecond()
        {
            return RequiredIncomeGold * ParseService.AmountCopperInGold / TimeSpan.FromHours(1).TotalMilliseconds;
        }
    }
}
