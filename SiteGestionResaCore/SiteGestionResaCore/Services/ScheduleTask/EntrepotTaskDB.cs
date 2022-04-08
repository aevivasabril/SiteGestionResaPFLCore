using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EntrepotTaskDB: IEntrepotTaskDB
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly GestionResaContext resaDb;
        private readonly ILogger<EntrepotTaskDB> logger;

        public EntrepotTaskDB(
            UserManager<utilisateur> userManager,
            GestionResaContext resaDb,
            ILogger<EntrepotTaskDB> logger)
        {
            this.userManager = userManager;
            this.resaDb = resaDb;
            this.logger = logger;
        }
        /// <summary>
        /// Obtenir la liste des projets dont la date de création est supérieure ou égal à 2 ans 11 mois de la date actuelle et inferieur à 3 ans pour notification
        /// </summary>
        /// <returns></returns>
        public List<projet> GetProjetsXNotification()
        {
            // 2 ans et 11 mois = 1065 jours environ
            // 3 ans            = 1095 jours environ
            List<projet> list = new List<projet>();

            var listProj = resaDb.projet.Where(p => p.entrepot_supprime == null && p.date_creation_entrepot != null);

            foreach (var p in listProj)
            {
                TimeSpan diff = DateTime.Now - p.date_creation_entrepot.Value;
                // Si le projet rentre dans la condition alors on envoie un mail
                if(diff.Days > 1065 & diff.Days < 1095)
                {
                    list.Add(p);
                }         
            }
            return list;
        }

        public List<projet> GetProjetsXSuppression()
        {
            List<projet> list = new List<projet>();

            var listP = resaDb.projet.Where(p => p.entrepot_supprime == null && p.date_creation_entrepot != null);

            foreach(var p in listP)
            {
                TimeSpan diff = DateTime.Now - p.date_creation_entrepot.Value;
                if(diff.Days >= 1095)
                {
                    list.Add(p);
                }
            }

            return list;
        }

        public bool SupprimerEntrepotXProj(projet pr)
        {
            bool isOk = false;

            var essais = resaDb.essai.Where(e => e.projetID == pr.id && e.entrepot_cree == true).ToList();           
            foreach(var ess in essais)
            {
                var docList = resaDb.doc_essai_pgd.Where(d => d.essaiID == ess.id).ToList();
                foreach (var dc in docList)
                {
                    try
                    {
                        resaDb.doc_essai_pgd.Remove(dc);
                        resaDb.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.ToString(), "Problème lors de la suppression du document automatique (task)");
                        return false;
                    }
                }
                
                try
                {
                    // Signaler que l'entrepot des données a été supprimé pour cet essai
                    var proj = resaDb.projet.First(p => p.id == pr.id);
                    proj.entrepot_supprime = true;
                    resaDb.SaveChanges();
                    isOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString(), "Problème lors de la mise à jour pour indiquer qu'un entrepôt a été supprimé pour ce projet (task)");
                    return false;
                }
            }

            return isOk;
        }
    }
}
