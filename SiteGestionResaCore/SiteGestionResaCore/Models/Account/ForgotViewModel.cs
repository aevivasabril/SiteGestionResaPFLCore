using System.ComponentModel.DataAnnotations;

namespace SiteGestionResaCore.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
