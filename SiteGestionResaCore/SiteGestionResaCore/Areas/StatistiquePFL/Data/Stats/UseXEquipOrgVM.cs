using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats
{
    public class UseXEquipOrgVM
    {
        /// <summary>
        /// Id d'un item selectionnée pour l'équipe
        /// </summary>
        [Display(Name = "Equipe STLO")]
        public int SelectEquipeId { get; set; }

        public IEnumerable<SelectListItem> EquipeStloItem { get; set; }

        /// <summary>
        /// Date debut pour recuperation des reservations
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateDu { get; set; }

        /// <summary>
        /// Date fin pour recuperation des reservations 
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateAu { get; set; }
    }
}
