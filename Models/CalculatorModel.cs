using info;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public class CalculatorModel
    {
        public Dictionary<int, long> Items { get; set; } = new Dictionary<int, long>();
        public int SelectedItemID { get; set; } = int.MinValue;
        public string SelectedRealmName { get; set; }
        public List<SelectListItem> ReamlsNameSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RecipeTreeNameSelectList { get; internal set; } = new List<SelectListItem>();
        public HashSet<ItemData> ItemList { get; internal set; } = new HashSet<ItemData>();
        public Dictionary<string, string> Result { get; internal set; } = new Dictionary<string, string>();
    }
}
