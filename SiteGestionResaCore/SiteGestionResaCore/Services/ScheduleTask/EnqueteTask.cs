using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
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

        public EnqueteTask(IServiceScopeFactory serviceScopeFactory): base(serviceScopeFactory)
        {
            // lien solution: https://www.thecodebuzz.com/cannot-consume-scoped-service-from-singleton-ihostedservice/
            emailSender = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            enqueteTaskDB = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEnqueteTaskDB>();
        }
        protected override string Schedule => "*/1 * * * *";
        //protected override string Schedule => "0 23 * * *";

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            List<enquete> ListEnquetesFirstTime = new List<enquete>();
            List<enquete> ListEnquetesXRelance = new List<enquete>();

            string message;
            IList<utilisateur> UsersAdmin = new List<utilisateur>();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification

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

                //string callbackUrl = "http://147.99.161.143/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>"+ essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": "+ proj.titre_projet+ "). Pour répondre à cette enquete: "+ "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";
                    
                await emailSender.SendEmailAsync(proj.mailRespProjet, "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));

                // Mettre à jour la date-envoi_enquete
                enqueteTaskDB.UpdateDateEnvoiEnquete(enque);
            }

            #endregion

            #endregion


            #region Relancer l'enquête de satisfaction si non répondu après 7 jours 

            ListEnquetesXRelance = enqueteTaskDB.GetEnquetesPourRelance();

            #region Envoi mail pour remplissage enquete

            foreach (var enque in ListEnquetesXRelance)
            {
                var essai = enqueteTaskDB.GetEssaiParEnquete(enque.essaiId);
                var proj = enqueteTaskDB.GetProjetParEnquete(essai.projetID);

                //string callbackUrl = "http://147.99.161.143/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>" + essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": " + proj.titre_projet + "). Pour répondre à cette enquete: " + "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

                await emailSender.SendEmailAsync(proj.mailRespProjet, "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                // Mettre à jour la date-envoi_enquete
                enqueteTaskDB.UpdateDateEnvoiEnquete(enque);
            }

            #endregion

            #endregion

            return Task.CompletedTask;
        }
      
    }
}
