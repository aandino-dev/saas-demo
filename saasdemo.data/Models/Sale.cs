using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saasdemo.data.Models
{
    public class Sale
    {
        [Key]
        public Guid Transaction_ID { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Product { get; set; }
        public float Price { get; set; }
        public string Payment_Type { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime Account_Created { get; set; }
        public DateTime Last_Login { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
