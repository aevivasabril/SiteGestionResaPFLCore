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

using SiteGestionResaCore.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Classe répresentant les infos réservation pour un équipement et pour un jour X
    /// </summary>
    public class ResasEquipParJour
    {
        public int IdEquipement { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Constructeur classe initialisant les Listes
        /// </summary>
        public ResasEquipParJour()
        {
            ListResasMatin = new List<InfosAffichageResa>();
            ListResasAprem = new List<InfosAffichageResa>();
            InfosIntervAprem = new List<InfosAffichageMaint>();
            InfosIntervMatin = new List<InfosAffichageMaint>();
            ListResasMatinAdjRest = new List<InfosAffichageResa>();
            ListResasApremAdjRest = new List<InfosAffichageResa>();
        }

        /// <summary>
        /// Informations détaillées pour chaque jour pour une semaine ou pour une durée déterminée par l'utilisateur
        /// </summary>
        /// J'ai réutilisé cette classe ReservationsJour car elle convient parfaitement
        public List<InfosAffichageResa> ListResasMatin { get; set; }

        public List<InfosAffichageResa> ListResasAprem { get; set; }

        public List<InfosAffichageMaint> InfosIntervMatin { get; set; }

        public List<InfosAffichageMaint> InfosIntervAprem { get; set; }

        public List<InfosAffichageResa> ListResasMatinAdjRest { get; set; }

        public List<InfosAffichageResa> ListResasApremAdjRest { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Matin)
        /// </summary>
        public string CouleurMatin { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Aprem)
        /// </summary>
        public string CouleurAprem { get; set; }
    }
}
