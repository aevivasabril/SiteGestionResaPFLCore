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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EntrepotTask : ScheduledProcessor
    {
        private readonly IEmailSender emailSender;
        private readonly IEntrepotTaskDB entrepotTaskDB;

        public EntrepotTask(IServiceScopeFactory serviceScopeFactory): base(serviceScopeFactory)
        {
            emailSender = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            entrepotTaskDB = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEntrepotTaskDB>();
        }
        protected override string Schedule => "30 21 * * 1"; // tous les lundis à 21h30

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            string message = "";
            // Reperer les entrepots qui ont 1 an et 11 mois dans la BDD (notification avant suppression)
            var listProjs = entrepotTaskDB.GetProjetsXNotification();

            #region Envoi mail pour le propiètaire projet, invitiation à récupérer son entrepot des données avant suppression définitive

            foreach (var proj in listProjs)
            {
                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette notification pour vous inviter à récupérer le fichier compressé pour votre entrepôt des données pour le projet : 
                            <b> </b>  N°: " + proj.num_projet + ": <strong> \"" + proj.titre_projet + "\" </strong>. En effet, dans les prochains jours " +
                            "il sera supprimé DEFINITIVEMENT de notre système car il arrive au bout de la date limite de stockage.<br/> " +
                            "</p><p>Cordialement, </p><p>L'équipe PFL! </p> </body></html>";

                await emailSender.SendEmailAsync(proj.mailRespProjet, "Notification avant suppression automatique entrepôt des données", message);
                await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "Notification avant suppression automatique entrepôt des données", message);// Ajouté uniquement pour vérifier!
            }

            #endregion

            #region Suppression des entrepôts dont la création date de 2 ans ou plus

            var ListProjXSupp = entrepotTaskDB.GetProjetsXSuppression();

            foreach(var proj in ListProjXSupp)
            {
                // Supprimer l'entrepôt "projet"
                bool isOk = entrepotTaskDB.SupprimerEntrepotXProj(proj);

                if (isOk)
                {
                    message = @"<html><body><p> Bonjour, <br><br> Après 2 ans de stockage dans notre système, votre entrêpot des données pour le projet: 
                            <b> </b>  N°: " + proj.num_projet + ": <strong> \"" + proj.titre_projet + "\" </strong> vient d'être supprimé définitivement.<br/> " +
                            "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

                    await emailSender.SendEmailAsync(proj.mailRespProjet, "Suppression automatique entrepôt des données", message);
                    await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "Notification avant suppression automatique entrepôt des données", message);
                }
                else
                {
                    // Les enterpôt avec un pb de suppression se feront lors de la prochaine execution de task
                }
            }

            #endregion

            return Task.CompletedTask;
        }
    }
}
