using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public class PostEnqueteDB: IPostEnqueteDB
    {
        private readonly GestionResaContext resaDb;

        public PostEnqueteDB(
            GestionResaContext resaDb)
        {
            this.resaDb = resaDb;
        }

        /// <summary>
        /// Si l'enquete appartient à un essai finalisé entre ces 2 dates, et que l'enquete a été répondue alors on récupére la réponse
        /// </summary>
        /// <param name="datedu"></param>
        /// <param name="dateau"></param>
        /// <returns> Liste des enquetes répondues</returns>
        public List<enquete> GetReponsesEnquetes(DateTime datedu, DateTime dateau)
        {
            // TODO: vérifier
            return resaDb.enquete.Where(e => e.date_premier_envoi.Value.Date >= datedu.Date && e.date_premier_envoi.Value.Date <= dateau.Date && e.reponduEnquete == true).ToList();
        }

        public essai GetEssai(int IdEssai)
        {
            return resaDb.essai.First(e => e.id == IdEssai);
        }

        public projet GetProjet(int IdProjet)
        {
            return resaDb.projet.First(p => p.id == IdProjet);
        }
    }
}
