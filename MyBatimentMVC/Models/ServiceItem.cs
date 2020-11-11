using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBatimentMVC.Models
{
    public class ServiceItem
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
