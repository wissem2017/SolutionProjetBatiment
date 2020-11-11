using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBatiment.API.Resources
{
    public class ProjectItemResource
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
