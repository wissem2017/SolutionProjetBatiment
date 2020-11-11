using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyBatimentMVC.ViewModels
{
    public class ProjectItemViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Projet")]
        [Required(ErrorMessage = "Veuillez entrer le nom de projet")]
        public string ProjectName { get; set; }

        [Display(Name = "Déscription")]
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }



    }
}
