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

using SiteGestionResaCore.Areas.Reservation.Data.Reservation;
using SiteGestionResaCore.Data;
using System.Collections.Generic;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    #region Model view pour la vue "PlanZonesReservation"

    #endregion

    #region Model View pour la vue "EquipementsVsZone"
    /// <summary>
    /// View model qui répresente les équipements contenus dans une zone specifique
    /// </summary>
    public class EquipementsParZoneViewModel
    {
        /// <summary>
        /// Nom de la zone à reserver
        /// </summary>
        public string NomZone { get; set; }

        /// <summary>
        /// Id de la zone à réserver
        /// </summary>
        public int IdZone { get; set; }

        /// <summary>
        /// création du model child pour le passer à la vue partielle _CalendrierEquipement (tous les réservations par équipement)
        /// Liste des équipements pour sauvegarder les infos à afficher sur les calendriers
        /// </summary>
        public List<CalendrierEquipChildViewModel> PreCalendrierChildVM { get; set; }

        /// <summary>
        /// Liste contenant uniquement les équipements sélectionnées
        /// </summary>
        public List<CalendrierEquipChildViewModel> CalendEquipSelectionnes { get; set; }

        public int IndiceChildModel { get; set; }

        public int IndiceResaEquipXChild { get; set; }

        public string OpenCalendEtCreneau { get; set; }

        public CalendrierEquipChildViewModel CalendVM { get; set; }

        /// <summary>
        /// Mois à sauvegarder pour synchroniser tous les datepicker de la page
        /// </summary>
        public int MoisDatePick { get; set; }

        /// <summary>
        /// Année à sauvegarder pour synchroniser tous les datepicker de la page
        /// </summary>
        public int AnneeDatePick { get; set; }

        /// <summary>
        /// Jour à sauvegarder pour synchroniser tous les datepicker de la page
        /// </summary>
        public int DayDatePick { get; set; }

        public List<SousListeEquipements> SousListeEquipements { get; set; }
    }
    #endregion

}