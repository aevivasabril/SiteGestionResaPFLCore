/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

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
