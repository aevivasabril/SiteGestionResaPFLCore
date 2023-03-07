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
