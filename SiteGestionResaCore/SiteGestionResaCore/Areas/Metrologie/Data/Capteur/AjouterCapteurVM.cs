using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    [Serializable]
    public class AjouterCapteurVM
    {
        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required]
        [Display(Name = "Pilote auquel le capteur est associé (*): ")]
        [Range(1, 1000, ErrorMessage = "Sélectionnez un pilote")]
        public int SelectedPiloteID { get; set; }

        public IEnumerable<SelectListItem> PiloteItem { get; set; }

        [Required (ErrorMessage = "Saisir le nom du capteur")]
        [Display(Name = "Nom du capteur (*): ")]       
        public string NomCapteur { get; set; }

        [Display(Name = "Code du capteur: ")]
        public string CodeCapteur { get; set; }

        [Required (ErrorMessage ="Saisir la date de la prochaine métrologie")]
        [DataType(DataType.Date)]
        [Display(Name = "Date prochaine métrologie (*): ")]
        public DateTime? DateProchaineVerif { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date dernière métrologie: ")]
        public DateTime? DateDernierVerif { get; set; }

        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required (ErrorMessage ="Sélectionnez la periodicité métrologie")]
        [Display(Name = "Périodicité métrologie (*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez l'option dans la liste")]
        public int SelectPeriodID { get; set; }

        public IEnumerable<SelectListItem> PeriodiciteItem { get; set; }

        [Required (ErrorMessage ="Indiquez si le capteur est conforme")]
        [Display(Name = "Capteur conforme? * ")]
        public bool? CapteurConforme { get; set; }

        [Required (ErrorMessage = "La valeur EMT doit être renseignée")]
        [Display(Name = "EMT capteur * ")]
        public double? EmtCapteur { get; set; }

        [Display(Name = "Si capteur non conforme alors inserez le facteur de correction (chiffre decimal: 0,0) : ")]
        public double? FacteurCorrectif { get; set; }
    }
}
