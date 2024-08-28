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
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    /// <summary>
    /// Classe qui permet d'intégrer toutes les données sur les réservations d'un jour specifique (destination vers le calendrier)
    /// </summary>
    public class ReservationsJour
    {
        /// <summary>
        /// Constructeur classe initialisant les Listes
        /// </summary>
        public  ReservationsJour ()
        {
            InfosResaMatin = new List<ReservationInfos>();
            InfosResaAprem = new List<ReservationInfos>();
            InfosIntervAprem = new List<InfosAffichageMaint>();
            InfosIntervMatin = new List<InfosAffichageMaint>();
        }

        /// <summary>
        /// Jour du calendrier à  interroger
        /// </summary>
        public DateTime JourResa { get; set; }

        /// <summary>
        /// Nom du jour en français
        /// </summary>
        public string NomJour { get; set; }

        /// <summary>
        /// Reservations pour le jour en question (Matin) 
        /// </summary>
        public List<ReservationInfos> InfosResaMatin { get; set; }

        /// <summary>
        /// Reservations pour le jour en question (Aprèm) 
        /// </summary>
        public List<ReservationInfos> InfosResaAprem { get; set; }

        public List<InfosAffichageMaint> InfosIntervMatin { get; set; }
        public List<InfosAffichageMaint> InfosIntervAprem { get; set; }

        // TODO: Créer le tableau !!!! métrologie pour le jour en question
        // metrologie [] InfosMetrologie { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Matin)
        /// </summary>
        public string CouleurFondMatin { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Aprem)
        /// </summary>
        public string CouleurFondAprem { get; set; }

    }
}