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
        /// Id d'un item selectionné pour un équipement (rapport interne)
        /// </summary>
        [Display(Name = "Equipement")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un capteur")]
        public int SelectecEquipementIntId { get; set; }

        public IEnumerable<SelectListItem> EquipementInterneItem { get; set; }


        /// <summary>
        /// Id d'un item selectionné pour un équipement (rapport externe)
        /// </summary>
        [Display(Name = "Equipement")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un capteur")]
        public int SelectecEquipementExtId { get; set; }

        public IEnumerable<SelectListItem> EquipementExterneItem { get; set; }


    }
}
