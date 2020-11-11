using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBatimentMVC.ViewModels
{
    public class RegisterViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Prénom")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Nom Utilisateur")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmez le mot de passe")]
        [Compare(nameof(Password), ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit comporter au moins {2} et au maximum {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
