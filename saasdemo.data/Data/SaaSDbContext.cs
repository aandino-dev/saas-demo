using saasdemo.data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data.Data
{
    public class SaaSDbContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }

        public SaaSDbContext() : base(nameof(SaaSDbContext))
        {

        }

        public SaaSDbContext(string nameOrConnectionString = nameof(SaaSDbContext)) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SaaSDbContext, Configuration>());
        }
    }
}
