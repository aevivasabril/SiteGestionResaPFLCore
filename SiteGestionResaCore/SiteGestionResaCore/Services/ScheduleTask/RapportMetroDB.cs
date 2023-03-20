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
