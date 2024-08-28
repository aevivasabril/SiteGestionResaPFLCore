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

using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public class ModifMaintenanceVM
    {
        public string NumMaintenance { get; set; }

        public string OpenModifInter { get; set; }

        public MaintenanceInfos InfosMaint { get; set; }

        public List<EquipCommunXInterv> ListEquipsCommuns { get; set; }

        public List<EquipPflXIntervention> ListeEquipsPfl { get; set; }

        /// <summary>
        /// Id intervention équipement adjacent
        /// </summary>
        public int IdIntervCom { get; set; }

        /// <summary>
        /// Id intervention équipement PFL
        /// </summary>
        public int IdIntervPfl { get; set; }

        /// <summary>
        /// Raison de la suppression
        /// </summary>
        public string RaisonSuppression { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Fin' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Nouvelle date fin : ")]
        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }

        [Required(ErrorMessage = "Veuillez décrire les actions réalisées")]
        public string ActionsMaintenance { get; set; }

        #region Ouvrir/ Fermer la vue suite à la recherche d'une opération

        public string Ouvert = "";
        public string Ferme = "none";

        #endregion
    }
}
