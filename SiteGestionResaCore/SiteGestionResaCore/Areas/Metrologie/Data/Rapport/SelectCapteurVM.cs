using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    [Serializable]
    public class SelectCapteurVM
    {
        /// <summary>
        /// Id d'un item selectionné pour un équipement
        /// </summary>
        [Display(Name = "Equipement")]
        public int SelectecEquipementId { get; set; }

        public IEnumerable<SelectListItem> EquipementItem { get; set; }

        /// <summary>
        /// Id d'un item selectionné pour un capteur appartenant à un équipement
        /// </summary>
        [Display(Name = "Capteur équipement")]
        public int SelectecCapteurId { get; set; }

        public IEnumerable<SelectListItem> CapteurItem { get; set; }

    }
}
