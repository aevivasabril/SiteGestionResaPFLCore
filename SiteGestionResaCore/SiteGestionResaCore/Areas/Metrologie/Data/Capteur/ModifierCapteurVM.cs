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

        [Display(Name = "Commentaire : ")]
        public string Commentaire { get; set; }

        [Display(Name = "Unité de mesure du capteur: ")]
        public string Unite { get; set; }
    }
}
