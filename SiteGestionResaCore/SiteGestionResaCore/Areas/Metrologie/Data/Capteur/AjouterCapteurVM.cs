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

        [Required (ErrorMessage ="Saisir la date de la prochaine métrologie Interne")]
        [DataType(DataType.Date)]
        [Display(Name = "Date prochaine métrologie Interne (*): ")]
        public DateTime? DateProchaineVerifInt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date dernière métrologie Interne: ")]
        public DateTime? DateDernierVerifInt { get; set; }

        [Required(ErrorMessage = "Saisir la date de la prochaine métrologie Externe")]
        [DataType(DataType.Date)]
        [Display(Name = "Date prochaine métrologie externe (*): ")]
        public DateTime? DateProchaineVerifExt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date dernière métrologie externe: ")]
        public DateTime? DateDernierVerifExt { get; set; }

        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required (ErrorMessage ="Sélectionnez la periodicité métrologie interne")]
        [Display(Name = "Périodicité métrologie interne (*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez l'option dans la liste")]
        public int SelectPeriodIDint { get; set; }

        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required(ErrorMessage = "Sélectionnez la periodicité métrologie externe")]
        [Display(Name = "Périodicité métrologie externe (*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez l'option dans la liste")]
        public int SelectPeriodIDExt { get; set; }

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
