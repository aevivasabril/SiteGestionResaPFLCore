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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class AjoutEquipementsViewModel
    {
        public string OuvrirEquipSansZone { get; set; }
        /// <summary>
        /// Description contenant une référence à l'équipement et le problème retrouvé
        /// </summary>
        [Display(Name = "Décrivez l'équipement + le problème: ")]
        public string DescriptionProbleme { get; set; }

        /// <summary>
        /// String contenant la description des zones affectées
        /// </summary>
        [Display(Name = "Saissisez les zones affectées. (Ex: PFL + Salles alimentaires)")]
        public string ZoneImpacte { get; set; }

        /// <summary>
        /// Date debut pour créneau maintenance
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Debut' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Debut : ")]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Fin' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Fin : ")]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (Maintenance)
        /// </summary>
        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date début est requis")]
        [Display(Name = "Créneau début")]
        public string DatePickerDebut_Matin { get; set; }

        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }

        //public EquipementSansZoneVM EquipementSansZoneVM { get; set; }

        public List<EquipementSansZoneVM> ListEquipsSansZone { get; set; }

        public int IdPourSuppressionSansZone { get; set; }

        public int IdPoursuppressionDansZone { get; set; }

        /// <summary>
        /// Liste des équipements présents dans les zones concernés par une intervention
        /// </summary>
        public List<EquipementDansZone> ListEquipsDansZones { get; set; }

    }
}
