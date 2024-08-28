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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EnqueteTask : ScheduledProcessor
    {
        private readonly IEnqueteTaskDB enqueteTaskDB;
        private readonly ILogger<EnqueteTask> logger;
        private readonly IEmailSender emailSender;

        public EnqueteTask(IServiceScopeFactory serviceScopeFactory,
            ILogger<EnqueteTask> logger): base(serviceScopeFactory)
        {
            // lien solution: https://www.thecodebuzz.com/cannot-consume-scoped-service-from-singleton-ihostedservice/
            emailSender = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            enqueteTaskDB = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEnqueteTaskDB>();
            this.logger = logger;
        }
        //protected override string Schedule => "*/2 * * * *";
        protected override string Schedule => "50 22 * * *"; // tous les jours à 22:50

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            List<enquete> ListEnquetesFirstTime = new List<enquete>();
            List<enquete> ListEnquetesXRelance = new List<enquete>();

            string message;

            // Ajouter un log pour tracer l'execution de la tâche planifié
            logger.LogWarning("Execution de la tâche automatique envoie d'enquetes le:  " + DateTime.Now);

            #region Rajouter les enquetes pour les essais dont elle a pas été crée automatiquement (Environnement Prod) TODO: A effacer une fois la MAJ est faite en Prod

            bool isOK = enqueteTaskDB.AreEnquetesCreated();

            #endregion

            #region Reperer les enquetes dont l'envoi se fait pour la première fois

            ListEnquetesFirstTime = enqueteTaskDB.GetEnquetesXFirstTime();

            #region Envoi mail pour remplissage enquete

            foreach(var enque in ListEnquetesFirstTime)
            {
                var essai = enqueteTaskDB.GetEssaiParEnquete(enque.essaiId);
                var proj = enqueteTaskDB.GetProjetParEnquete(essai.projetID);
                var email = enqueteTaskDB.GetEmailCreatorEssai(essai.compte_userID);

                string callbackUrl = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                //string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>"+ essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": "+ proj.titre_projet+ "). Pour répondre à cette enquete: "+ "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";
                try
                {
                    await emailSender.SendEmailAsync(email, "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                }
                catch(Exception e)
                {
                    logger.LogError("Problème d'envoi de mail enquete pour l'essai Id: " + essai.id + " .Message: " + e.Message);
                }
               
                //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));

                // Mettre à jour la date-envoi_enquete
                enqueteTaskDB.UpdateDateEnvoiEnquete(enque);
            }

            #endregion
            // TODO: effacer!! c'est juste pour tester l'envoi des mails tous les 2 minutes
            await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "TEST tâche côté serveur", DateTime.Now.ToString());

            #endregion


            #region Relancer l'enquête de satisfaction si non répondu après 7 jours 

            ListEnquetesXRelance = enqueteTaskDB.GetEnquetesPourRelance();

            #region Envoi mail pour remplissage enquete

            foreach (var enque in ListEnquetesXRelance)
            {
                var essai = enqueteTaskDB.GetEssaiParEnquete(enque.essaiId);
                var proj = enqueteTaskDB.GetProjetParEnquete(essai.projetID);
                var email = enqueteTaskDB.GetEmailCreatorEssai(essai.compte_userID);

                string callbackUrl = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                //string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>" + essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": " + proj.titre_projet + "). Pour répondre à cette enquete: " + "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

                try
                {
                    await emailSender.SendEmailAsync(email, "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                }
                catch (Exception e)
                {
                    logger.LogError("Problème d'envoi de mail enquete (RELANCE) pour l'essai Id: " + essai.id + " .Message: " + e.Message);
                }
                
                //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                // Mettre à jour la date-envoi_enquete
                enqueteTaskDB.UpdateDateEnvoiEnquete(enque);
            }

            #endregion

            #endregion

            return Task.CompletedTask;
        }
      
    }
}
