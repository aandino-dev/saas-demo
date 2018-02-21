using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data
{
    public class Tenant : ITenant
    {
        [Key]
        public Guid TenantID { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Organization { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Server { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Database { get; set; }
    }
}
