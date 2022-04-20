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
