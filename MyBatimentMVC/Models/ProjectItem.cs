using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBatimentMVC.Models
{
    public class ProjectItem
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
