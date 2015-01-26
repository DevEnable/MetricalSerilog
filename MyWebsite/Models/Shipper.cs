using System;

namespace MyWebsite.Models
{
    [Serializable]
    public class Shipper
    {
        public int ShipperId { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}