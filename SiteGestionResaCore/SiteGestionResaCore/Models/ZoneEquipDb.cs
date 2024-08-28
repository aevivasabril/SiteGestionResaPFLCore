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


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class ZoneEquipDb: IZoneEquipDb
    {

        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        private readonly ILogger<ZoneEquipDb> logger;

        /// <summary>
        /// Constructeur initializant la connexion vers les 2 base de données
        /// </summary>
        public ZoneEquipDb(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager,
            ILogger<ZoneEquipDb> logger)
        {
            this.context = resaDB;
            this.userManager = userManager;
            this.logger = logger;
        }
        /// <summary>
        /// Obtenir liste de toutes les zones PFL
        /// </summary>
        /// <returns>Liste "zone"</returns>
        public List<zone> ListeZones()
        {
            return context.zone.ToList();
        }

        /// <summary>
        /// Méthode pour obtenir une liste des équipement pour une zone déterminée
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>Liste des équipements + boolean si équipement réservé ou pas</returns>
        public List<equipement> ListeEquipements(int idZone)
        {
            List<equipement> List = new List<equipement>();

            var query = (from equip in context.equipement
                         where equip.zoneID == idZone && equip.equip_delete != true
                         select equip).ToArray();

            foreach (var y in query)
            {
                List.Add(y);
            }

            return List;
        }

        public equipement GetEquipement(int IdEquip)
        {
            return (context.equipement.FirstOrDefault(u => u.id == IdEquip));
        }


        /// <summary>
        /// Méthode pour obtenir le nom d'une zone
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>nom de la zone</returns>
        public string GetNomZone(int idZone)
        {
            return context.zone.First(x => x.id == idZone).nom_zone;
        }





       
    }
}
