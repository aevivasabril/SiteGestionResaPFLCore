using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public class AccueilStatsVM
    {
        /// <summary>
        /// Date debut à afficher pour le planning PFL 
        /// </summary>
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateDu { get; set; }

        /// <summary>
        /// Date fin à afficher pour le planning PFL 
        /// </summary>
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateAu { get; set; }


        public int AnneeActuel { get; set; }


        /// <summary>
        /// Id d'un item selectionnée pour le type de financement
        /// </summary>
        [Display(Name = "Projet")]
        [Range(1, 30, ErrorMessage = "Sélectionnez un projet")]
        public int SelectProjetId { get; set; }

        public IEnumerable<SelectListItem> ProjetItem { get; set; }


        /// <summary>
        /// Id d'un item selectionnée pour l'équipe
        /// </summary>
        [Display(Name = "Equipe STLO")]
        [Range(1, 30, ErrorMessage = "Sélectionnez un equipe")]
        public int SelectEquipeId { get; set; }

        public IEnumerable<SelectListItem> EquipeStloItem { get; set; }

        /// <summary>
        /// Date debut pour recuperation des reservations
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateDuEquip { get; set; }

        /// <summary>
        /// Date fin pour recuperation des reservations 
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateAuEquip { get; set; }

        /// <summary>
        /// Id d'un item selectionnée pour l'équipe
        /// </summary>
        [Display(Name = "Organisme")]
        [Range(1, 30, ErrorMessage = "Sélectionnez un organisme")]
        public int SelectOrgId { get; set; }

        public IEnumerable<SelectListItem> OrgItem { get; set; }

        /// <summary>
        /// Date debut pour recuperation des maintenances
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateDuMaintenance { get; set; }

        /// <summary>
        /// Date fin pour recuperation des maintenances 
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateAuMaintenance { get; set; }

        public List<MaintenanceInfos> ListMaintenances { get; set; }

    }
}
