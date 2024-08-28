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


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class RapportMetroTask: ScheduledProcessor
    {
        private readonly IRapportMetroDB rapportMetroDB;
        private readonly ILogger<RapportMetroTask> logger;
        //private readonly IEmailSender emailSender;

        public RapportMetroTask(IServiceScopeFactory serviceScopeFactory,
            ILogger<RapportMetroTask> logger) : base(serviceScopeFactory)
        {
            // lien solution: https://www.thecodebuzz.com/cannot-consume-scoped-service-from-singleton-ihostedservice/
            //emailSender = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            rapportMetroDB = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRapportMetroDB>();
            this.logger = logger;
        }

        protected override string Schedule => "0 23 7 * *"; // tous les 7 du mois à 23h

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            // Supprimer les rapport métrologie anciens de 3 ans ou plus
            try
            {
                await rapportMetroDB.SuppressionRapportsAnciens();
                logger.LogWarning("Suppression des rapports métrologiques OK");
            }
            catch(Exception e)
            {
                logger.LogError("Problème lors de la suppression des anciens rapports de métrologie: " +e.Message);
            }

            return Task.CompletedTask;
        }
    }
}
