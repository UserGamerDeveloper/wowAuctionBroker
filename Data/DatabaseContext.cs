using Microsoft.EntityFrameworkCore;
using Mvc.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<RealmModel> Realms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                    => options.UseLazyLoadingProxies().UseSqlite("Data Source=database.db");
    }
}
