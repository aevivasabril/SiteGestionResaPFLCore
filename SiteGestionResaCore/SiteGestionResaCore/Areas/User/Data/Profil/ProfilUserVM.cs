using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.Profil
{
    public class ProfilUserVM
    {
        [Display(Name = "Nom utilisateur: ")]
        public string NomUsr { get; set; }
        [Display(Name = "Prénom utilisateur: ")]
        public string PrenomUsr { get; set; }
        [Display(Name = "Mail utilisateur: ")]
        public string MailUsr { get; set; }
        [Display(Name = "Votre équipe: ")]
        public string NomEquipe { get; set; }
        [Display(Name = "Votre organisme: ")]
        public string NomOrganisme { get; set; }
        public List<EnquetesNonReponduesXUsr> ListEnquetesNonRepondues { get; set; }
    }
}
