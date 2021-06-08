using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data
{
    public class EnqueteDb: IEnqueteDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly ILogger<EnqueteDb> logger;

        public EnqueteDb(
            GestionResaContext resaDb,
            ILogger<EnqueteDb> logger)
        {
            this.context = resaDb;
            this.logger = logger;
        }

        public essai ObtenirEssai(int essaiID)
        {
            return context.essai.First(e=>e.id == essaiID);
        }

        public projet ObtenirProjet(essai essai)
        {
            return context.projet.First(p => p.id == essai.projetID);
        }

        public void UpdateEnqueteWithResponse(string reponse, enquete enquete)
        {
            //var enquete = context.enquete.First(e => e.id == e);
            enquete.fichierReponse = reponse;
            enquete.reponduEnquete = true;
            enquete.date_reponse = DateTime.Now;

            context.SaveChanges();
        }

        public enquete ObtenirEnqueteFromEssai(int IdEssai)
        {
            return context.enquete.First(e => e.essaiId == IdEssai);
        }
    }
}
