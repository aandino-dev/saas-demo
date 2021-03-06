﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using saasdemo.data;

namespace saasdemo.web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, ITenant
    {
        public Guid TenantID { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Organization { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Server { get; set; }
        [MinLength(4), MaxLength(50)]
        public string Database { get; set; }
    }
}
