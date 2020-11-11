using System;
using System.Collections.Generic;
using System.Text;

namespace MyBatiment.Core.Models
{
    public class ProjectItem
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
