using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MyBatimentMVC.Models;

namespace MyBatimentMVC.ViewModels
{
    public class ListBatimentViewModel
    {
        public Owner Owner;
        public List<ProjectItem> ListProject;
        public List<ServiceItem> ListeService;

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
