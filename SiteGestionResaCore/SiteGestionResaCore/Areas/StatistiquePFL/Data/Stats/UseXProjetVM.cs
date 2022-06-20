using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public class UseXProjetVM
    {
        /// <summary>
        /// Id d'un item selectionnée pour le type de financement
        /// </summary>
        [Display(Name = "Projet")]
        public int SelectProjetId { get; set; }

        public IEnumerable<SelectListItem> ProjetItem { get; set; }
    }
}
