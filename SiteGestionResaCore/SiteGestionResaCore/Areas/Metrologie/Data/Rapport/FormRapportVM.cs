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
        public string nomCapteur { get; set; }

        /// <summary>
        /// Id d'un item selectionné pour un équipement
        /// </summary>
        [Display(Name = "Equipement")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un opérateur")]
        public int SelectecOperateurId { get; set; }

        public IEnumerable<SelectListItem> OperateurItem { get; set; }

        public DateTime DateVerifMetro { get; set; }

        // interne ou externe
        public string TypeMetrologie { get; set; }

        public double emtCapteur { get; set; }
        public bool capteurConforme { get; set; }
        public double facteurCorrectif { get; set; }

    }
}
