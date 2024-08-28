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

using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Data
{
    public class EvenementDB : IEvenementDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<EvenementDB> logger;

        public EvenementDB(
            GestionResaContext resaDB,
            ILogger<EvenementDB> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        public List<evenement> ListEvenements()
        {
            return resaDB.evenement.Distinct().OrderByDescending(i=>i.date_creation).ToList();
        }

        public bool AjoutEvent(string message)
        {
            try
            {
                evenement eve = new evenement() { message = message, date_creation = DateTime.Now };
                resaDB.evenement.Add(eve);
                resaDB.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                logger.LogError("", "problème lors de l'écriture de l'événement: " +  e.ToString());
                return false;
            }            
        }
        public bool SupprimerEvenement(int idEvent)
        {
            try
            {
                evenement eve = resaDB.evenement.First(e => e.id == idEvent);
                resaDB.evenement.Remove(eve);
                resaDB.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("", "problème lors de la suppression de l'événement: " + e.ToString());
                return false;
            }
        }
    }
}   

