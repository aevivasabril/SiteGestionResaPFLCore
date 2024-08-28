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

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class RapportMetroDB : IRapportMetroDB
    {
        private readonly GestionResaContext resaDb;
        private readonly ILogger<EnqueteTaskDB> logger;

        public RapportMetroDB(
            GestionResaContext resaDb,
            ILogger<EnqueteTaskDB> logger)
        {
            this.resaDb = resaDb;
            this.logger = logger;
        }

        public async Task SuppressionRapportsAnciens()
        {
            //Lister les rapports anciens de 3 mois ou plus
            //bool IsOk = false;
            List<rapport_metrologie> list = new List<rapport_metrologie>();

            try
            {
                list = resaDb.rapport_metrologie.Where(r => r.date_verif_metrologie <= DateTime.Today.AddYears(-3)).Distinct().ToList();

                foreach (var r in list)
                {
                    resaDb.rapport_metrologie.Remove(r);
                    await resaDb.SaveChangesAsync();
                }
                //IsOk = true;
            }
            catch(Exception e)
            {
                //IsOk = false;
                logger.LogError("", "Problème pour supprimer un des rapports métrologiques. Erreur: " + e.ToString());
            }


        }
    }
}
