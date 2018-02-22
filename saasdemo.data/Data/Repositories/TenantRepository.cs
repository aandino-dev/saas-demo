using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data.Data.Repositories
{
    public class TenantRepository
    {
        private ApplicationContext _db { get; set; }
        private Guid _tenantID { get; set; }

        public TenantRepository(Guid tenantID, ApplicationContext db = null)
        {
            _db = db ?? new ApplicationContext();
            _tenantID = tenantID;
        }

        public async Task<ITenant> GetAsync()
        {
            return await _db.Tenants.FindAsync(_tenantID);
        }

        public async Task<ITenant> UpdateAsync(string server, string database)
        {
            ITenant tenant =  await GetAsync();
            tenant.Server = server;
            tenant.Database = database;

            await _db.SaveChangesAsync();

            return tenant;
        }
    }
}
