using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data
{
    public interface ITenant
    {
        Guid TenantID { get; set; }
        string Organization { get; set; }
        string Server { get; set; }
        string Database { get; set; }
    }
}
