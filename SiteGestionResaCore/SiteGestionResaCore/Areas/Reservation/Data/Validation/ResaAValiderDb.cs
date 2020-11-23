using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    /// <summary>
    /// Classe d'accès à la base de données pour la partie validation des réservations
    /// </summary>
    public class ResaAValiderDb : IResaAValiderDb
    {
        private readonly GestionResaContext resaDB;

        public ResaAValiderDb(
            GestionResaContext resaDB)
        {
            this.resaDB = resaDB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Liste des réservations avec leurs infos pour affichage</returns>
        public List<AffichageResasAValider> ResasAValider()
        {
            AffichageResasAValider infos = new AffichageResasAValider();

            List<AffichageResasAValider> list = new List<AffichageResasAValider>();

            // Récuperer tous les "essai" en attente de validation
            var essais = resaDB.essai.Where(e => e.status_essai == EnumStatusEssai.WaitingValidation.ToString()).Distinct().ToList();
            
            foreach(var essai in essais)
            {

            }

            return list;
        }
    }
}
