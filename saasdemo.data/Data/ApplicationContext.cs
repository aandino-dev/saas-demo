using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public ApplicationContext() : base(nameof(ApplicationContext)) { }

        public ApplicationContext(string nameOrConnectionString = nameof(ApplicationContext)) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new NullDatabaseInitializer<ApplicationContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Tenant>().ToTable("AspNetUsers");
            base.OnModelCreating(modelBuilder);

        }

        private void FixEfProviderServicesProblem()
        {
            // The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            // for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            // Make sure the provider assembly is available to the running application. 
            // See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
