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

        public List<enquete> GetReponsesEnquetes(DateTime datedu, DateTime dateau)
        {
            return resaDb.enquete.Where(e => e.date_premier_envoi.Value.Date >= datedu.Date && e.date_premier_envoi.Value.Date <= dateau.Date).ToList();
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
