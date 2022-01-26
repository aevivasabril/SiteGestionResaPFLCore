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
        private readonly IEmailSender emailSender;
        private readonly ILogger<EnqueteTask> logger;

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
            bool success = false;
            int retryCount = 5;

            string message;

            #region Rajouter les enquetes pour les essais dont elle a pas été crée automatiquement (Environnement Prod) TODO: A effacer une fois la MAJ est faite en Prod
            try
            {
                enqueteTaskDB.AreEnquetesCreated();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Error lors de la création des enquetes");
            }

            #endregion

            #region Reperer les enquetes dont l'envoi se fait pour la première fois
            try
            {
                ListEnquetesFirstTime = enqueteTaskDB.GetEnquetesXFirstTime();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Error lors de l'obtention de la liste des enquetes pour un premier envoi");
            }

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
                
                success = false;
                retryCount = 5;
                while (!success && retryCount > 0)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(email, "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                        success = true;
                    }
                    catch (Exception e)
                    {
                        retryCount--;
                        if (retryCount == 0)
                        {
                            logger.LogError(e, "Error d'envoi de mail lors de l'envoi des enquetes");
                        }
                    }
                }

                //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));

                // Mettre à jour la date-envoi_enquete
                enqueteTaskDB.UpdateDateEnvoiEnquete(enque);
            }

            #endregion

            // TODO: effacer!! c'est juste pour tester l'envoi des mails tous les 2 minutes
            success = false;
            retryCount = 5;
            while (!success && retryCount > 0)
            {
                try
                {
                    await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "TEST tâche côté serveur", DateTime.Now.ToString());
                    success = true;
                }
                catch (Exception e)
                {
                    retryCount--;
                    if (retryCount == 0)
                    {
                        logger.LogError(e, "Error d'envoi de mail Test super Admin");
                    }
                }
            }

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

                success = false;
                retryCount = 5;
                while (!success && retryCount > 0)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(email, "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                        success = true;
                    }
                    catch (Exception e)
                    {
                        retryCount--;
                        if (retryCount == 0)
                        {
                            logger.LogError(e, "Error d'envoi de mail Test super Admin");
                        }
                    }
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
