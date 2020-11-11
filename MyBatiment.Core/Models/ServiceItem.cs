using System;
using System.Collections.Generic;
using System.Text;

namespace MyBatiment.Core.Models
{
    public class ServiceItem
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
