using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    [Serializable]
    public class ModifierCapteurVM
    {
        public int idCapteur { get; set; }

        public string NomEquipement { get; set; }

        [Required(ErrorMessage = "Saisir le nom du capteur")]
        [Display(Name = "Nom du capteur (*): ")]
        public string NomCapteur { get; set; }

        [Display(Name = "Code du capteur: ")]
        public string CodeCapteur { get; set; }

        [Required(ErrorMessage = "Saisir la date de la prochaine métrologie interne")]
        [DataType(DataType.Date)]
        [Display(Name = "Date prochaine métrologie interne(*): ")]
        public DateTime? DateProchaineVerifInt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date dernière métrologie interne: ")]
        public DateTime? DateDernierVerifInt { get; set; }

        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required(ErrorMessage = "Sélectionnez la periodicité métrologie interne")]
        [Display(Name = "Périodicité métrologie interne (*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez l'option dans la liste")]
        public int SelectPeriodIDInt { get; set; }

        [Required(ErrorMessage = "Saisir la date de la prochaine métrologie externe")]
        [DataType(DataType.Date)]
        [Display(Name = "Date prochaine métrologie externe (*): ")]
        public DateTime? DateProchaineVerifExt { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date dernière métrologie externe: ")]
        public DateTime? DateDernierVerifExt { get; set; }

        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required(ErrorMessage = "Sélectionnez la periodicité métrologie externe")]
        [Display(Name = "Périodicité métrologie externe(*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez l'option dans la liste")]
        public int SelectPeriodIDExt { get; set; }

        public IEnumerable<SelectListItem> PeriodiciteItem { get; set; }

        [Required(ErrorMessage = "Indiquez si le capteur est conforme")]
        [Display(Name = "Capteur conforme? * ")]
        public bool? CapteurConforme { get; set; }

        [Required(ErrorMessage = "La valeur EMT doit être renseignée")]
        [Display(Name = "EMT capteur * ")]
        public double? EmtCapteur { get; set; }

        [Display(Name = "Si capteur non conforme alors inserez le facteur de correction (chiffre decimal: 0,0) : ")]
        public double? FacteurCorrectif { get; set; }
    }
}
