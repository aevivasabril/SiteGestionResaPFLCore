using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteGestionResaCore.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// Email de l'utilisateur pour l'ouverture de compte
        /// </summary>
        [Required(ErrorMessage ="Le champ Email est requis")]
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z][^%$*!&?()\\^#,=\/""\s]*@[a-zA-Z\[-]*]*\.[a-zA-Z]{2,}", ErrorMessage ="Votre adresse mail semble erroné!")] // mettre 2 "" dans l'expression quand on souhaite filtrer le "
        [Display(Name = "Email*")]
        public string Email { get; set; }

        /// <summary>
        /// Mot de passe défini par l'utilisateur
        /// </summary>
        [Required (ErrorMessage = "Le champ mot de passe est requis")]
        [StringLength(100, ErrorMessage = "Le {0} doit avoir au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        /// <summary>
        /// Mot de passe pour vérification
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer mot de passe*")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        [Required (ErrorMessage = "Le champ Nom est requis") ]
        [MaxLength(25)]
        [Display(Name = "Nom*")]
        [RegularExpression(@"[\p{L}- ]+$", ErrorMessage = "Format invalide!")]
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        [Required (ErrorMessage = "Le champ Prenom est requis")]
        [MaxLength(25)]
        [Display(Name = "Prenom*")]
        [RegularExpression(@"[\p{L}- ]+$", ErrorMessage = "Format invalide!")]
        public string Prenom { get; set; }

      
        /// <summary>
        /// Id de l'item selectionné pour l'organisme d'appartenance de l'utilisateur
        /// </summary>
        [Required (ErrorMessage = "Le champ Organisme est requis")]
        [Display(Name = "Organisme*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un organisme")]
        public int SelectedOrganId { get; set; }

        public IEnumerable<SelectListItem> OrganItem { get; set; }
       
    }
}
