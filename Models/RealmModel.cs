using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models
{
    public enum FactionType
    {
        ALLIANCE,
        HORDE,
        NONE
    }
    public class RealmModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ConnectedRealmId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime TimeUpdate { get; set; }
        public virtual List<ActiveRecipe> ActiveRecipes { get; set; }
        public bool FarmMode { get; set; }
        public virtual List<FactionModel> Fractions { get; set; }
        public virtual List<Character> Characters { get; set; }

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
    public class FactionModel
    {
        public int Id { get; set; }
        public FactionType FactionType { get; set; }
        public long MoneyMax { get; set; }
        public int RealmId { get; set; }
        [Required]
        [ForeignKey("RealmId")]
        public virtual RealmModel Company { get; set; }
    }
    public class Character
    {
        public long Id { get; set; }
        public int RealmId { get; set; }
        [Required]
        [ForeignKey("RealmId")]
        public virtual RealmModel Company { get; set; }
    }
}
