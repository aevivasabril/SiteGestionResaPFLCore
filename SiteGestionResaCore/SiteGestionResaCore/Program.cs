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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Opts;

namespace SiteGestionResaCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostReturn = CreateHostBuilder(args).Build(); // appel à configure services 
            using(var scope = hostReturn.Services.CreateScope())
            {
                using (var db = scope.ServiceProvider.GetRequiredService<GestionResaContext>())
                {
                    await db.Database.MigrateAsync();// Vérifie les changements sur la BDD prod et dev et le mettre à jour 
                
                    // Me rajouter dans les rôles Admin et MainAdmin par défaut
                    IOptions<AdminOptions> options = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>();
                    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<utilisateur>>();
                    if(await UserManager.FindByEmailAsync(options.Value.email) == null)
                    {
                        var usr = new utilisateur { Email = options.Value.email, UserName = options.Value.email, nom = options.Value.nom, prenom = options.Value.prenom, EmailConfirmed = true, organismeID = 1, equipeID = 6 };
                        await UserManager.CreateAsync(usr, options.Value.mdp);
                        await UserManager.AddToRoleAsync(usr, "MainAdmin");
                        await UserManager.AddToRoleAsync(usr, "Admin");
                        await UserManager.AddToRoleAsync(usr, "LogisticMaint");
                    }
                }
            }
            hostReturn.Run(); // appel à configure

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
