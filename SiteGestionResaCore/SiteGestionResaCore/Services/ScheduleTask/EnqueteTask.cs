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

        public EnqueteTask(IServiceScopeFactory serviceScopeFactory/*, 
            IEnqueteTaskDB enqueteTaskDB,
            IEmailSender emailSender*/): base(serviceScopeFactory)
        {
            /*this.enqueteTaskDB = enqueteTaskDB;
            this.emailSender = emailSender;*/
            // lien solution: https://www.thecodebuzz.com/cannot-consume-scoped-service-from-singleton-ihostedservice/
            emailSender = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            enqueteTaskDB = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEnqueteTaskDB>();
        }
        protected override string Schedule => "*/1 * * * *";

        public override async Task<Task> ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            //var Ok = SendingEmailAsync();
            // Retry pour envoi mail
            string message;

            IList<utilisateur> UsersAdmin = new List<utilisateur>();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification

            message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> TEST pour vérifier l'envoi de mail par tâche planifié!! : <b> </b> (Essai N°: ) " +
                             " c'est tout"
                            + "</p><p>L'équipe PFL! </p> </body></html>";

            UsersAdmin = await enqueteTaskDB.GetUtilisateurSuperAdminAsync();

            for (int i = 0; i < UsersAdmin.Count(); i++)
            {

                await emailSender.SendEmailAsync(UsersAdmin[i].Email, "Test task ENQUETE", message);
            }

            Console.WriteLine("ENQUETE 1 : " + DateTime.Now.ToString()); // Fonctionne!
            return Task.CompletedTask;
        }
      
    }
}
