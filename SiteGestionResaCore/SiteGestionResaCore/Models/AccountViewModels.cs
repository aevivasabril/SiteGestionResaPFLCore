using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteGestionResaCore.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Se souvenir de moi?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        /// <summary>
        /// Email de l'utilisateur pour l'ouverture de compte
        /// </summary>
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z][^%$*!&?()\\^#,=\/""\s]*@[a-zA-Z\[-]*]*\.[a-zA-Z]{2,}", ErrorMessage ="Votre adresse mail semble erroné!")] // mettre 2 "" dans l'expression quand on souhaite filtrer le "
        [Display(Name = "Email*")]
        public string Email { get; set; }

        /// <summary>
        /// Mot de passe défini par l'utilisateur
        /// </summary>
        [Required]
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
        [Required]
        [MaxLength(25)]
        [Display(Name = "Nom*")]
        [RegularExpression(@"[\p{L}- ]+$", ErrorMessage = "Format invalide!")]
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        [Required]
        [MaxLength(25)]
        [Display(Name = "Prenom*")]
        [RegularExpression(@"[\p{L}- ]+$", ErrorMessage = "Format invalide!")]
        public string Prenom { get; set; }

        private List<organisme> _listeOrganismes;    
        /// <summary>
        /// Obtenir la liste des organismes à partir de la base de données
        /// </summary>
        public List<organisme> ListeOrganismes
        {
            get 
            {
                // TODO: c'est le contrôleur qui doit générer cette liste
                /*using (IResaDB resaBdd = new ResaDB())
                {
                    _listeOrganismes = new List<organisme>(resaBdd.ObtenirListOrg());
                }*/
                return _listeOrganismes; 
            }
            set { _listeOrganismes = value; }
        }

        /// <summary>
        /// Id de l'item selectionné pour l'organisme d'appartenance de l'utilisateur
        /// </summary>
        [Required]
        [Display(Name = "Organisme*")]
        [Range(1, 100, ErrorMessage = "Selectionnez un organisme")]
        public int SelectedOrganId { get; set; }
       
        /// <summary>
        /// Création du dropdownlist pour lister les organismes d'appartenance
        /// </summary>
        public IEnumerable<SelectListItem> OrganItem
        {
            get
            {
                var allOrgs = ListeOrganismes.Select(f => new SelectListItem
                {
                    Value = f.id.ToString(),
                    Text = f.nom_organisme
                });
                return DefaultOrgItem.Concat(allOrgs);
            }
        }

        /// <summary>
        /// entete de la liste des organismes d'appartenance 
        /// </summary>
        public IEnumerable<SelectListItem> DefaultOrgItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Selectionner un organisme -"
                }, count: 1);
            }
        }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
