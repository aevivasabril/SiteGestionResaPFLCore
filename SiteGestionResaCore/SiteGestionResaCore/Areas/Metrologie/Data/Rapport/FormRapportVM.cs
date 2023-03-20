using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public class FormRapportVM
    {
        public int idCapteur { get; set; }
        [Display(Name ="Nom du capteur: ")]
        public string nomCapteur { get; set; }

        [Display(Name = "Equipement: ")]
        public string nomEquipement { get; set; }

        /// <summary>
        /// Id d'un item selectionné pour un équipement
        /// </summary>
        [Display(Name = "Opérateur métrologie ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un opérateur")]
        public int SelectecOperateurId { get; set; }

        public IEnumerable<SelectListItem> OperateurItem { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date vérification métrologie: ")]
        [Required(ErrorMessage ="Sélectionnez la date de vérification métrologie ")]
        public DateTime? DateVerifMetro { get; set; }

        // interne ou externe
        [Display(Name = "Type de métrologie: ")]
        public string TypeMetrologie { get; set; }

        [Display(Name ="Emt capteur: ")]
        public double emtCapteur { get; set; }

        [Display(Name = "Capteur conforme après vérification? : ")]
        [Required(ErrorMessage ="Indiquez si le capteur est conforme")]
        public bool? capteurConforme { get; set; }

        [Display(Name = "Facteur correctif capteur: ")]
        public double? facteurCorrectif { get; set; }

        public string nomDocRapport { get; set; }

        public byte[] contenuRapport { get; set; }
        public string Commentaire { get; set; }

    }
}
