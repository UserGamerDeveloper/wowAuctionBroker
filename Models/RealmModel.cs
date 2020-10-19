using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public class RealmModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ConnectedRealmId { get; set; }
        [Required]
        public string Name { get; set; }
        public long CharacterId { get; set; }
        public DateTime TimeUpdate { get; set; }
        public virtual List<ActiveRecipe> ActiveRecipes { get; set; }
        public bool FarmMode { get; set; }
        public long MoneyMax { get; set; }

    }
    public class ActiveRecipe
    {
        public int Id { get; set; }
        public int IdRecipe { get; set; }
        public int RealmId { get; set; }
        [Required]
        [ForeignKey("RealmId")]
        public virtual RealmModel Company { get; set; }
    }
}
