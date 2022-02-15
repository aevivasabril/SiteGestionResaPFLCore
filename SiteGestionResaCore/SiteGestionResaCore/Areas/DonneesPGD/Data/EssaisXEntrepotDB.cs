using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class EssaisXEntrepotDB: IEssaisXEntrepotDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private readonly GestionResaContext contextDB;
        private readonly ILogger<EssaisXEntrepotDB> logger;

        public EssaisXEntrepotDB(
            GestionResaContext contextDB,
            ILogger<EssaisXEntrepotDB> logger)
        {
            this.contextDB = contextDB;
            this.logger = logger;
        }
        public List<InfosResasSansEntrepot> ObtenirResasSansEntrepotUsr(utilisateur usr)
        {
            List<InfosResasSansEntrepot> infos = new List<InfosResasSansEntrepot>();
            //var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr).ToList();
            var list = contextDB.essai.Where(e => e.entrepot_cree == null && e.compte_userID == usr.Id).ToList();
            foreach(var ess in list)
            {
                // Obtenir le projet pour l'essai X
                var proj = contextDB.projet.First(p => p.id == ess.projetID);
                InfosResasSansEntrepot info = new InfosResasSansEntrepot { idEssai = ess.id, DateSaisieEssai = ess.date_creation,
                                                                            idProj = proj.id, MailRespProj = proj.mailRespProjet, 
                                                                            NomProjet = proj.titre_projet, NumProjet = proj.num_projet, TitreEssai = ess.titreEssai};
                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// Obtenir des infos sur un projet à partir son Id
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public InfosProjet ObtenirInfosProjet(int id)
        {
            var proj = contextDB.projet.First(p => p.id == id);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation,
                Description = proj.description_projet,
                Financement = proj.financement,
                MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = contextDB.Users.First(p => p.Id == proj.compte_userID).Email,
                NumProjet = proj.num_projet,
                Organisme = contextDB.organisme.First(o => o.id == proj.organismeID).nom_organisme,
                Provenance = proj.provenance,
                TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;
        }

        /// <summary>
        /// Obtenir les infos affichage essai à partir d'un id essai
        /// </summary>
        /// <param name="idEssai"></param>
        /// <returns></returns>
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai)
        {
            var essai = contextDB.essai.First(e => e.id == idEssai);

            ConsultInfosEssaiChildVM Infos = new ConsultInfosEssaiChildVM
            {
                id = essai.id,
                TitreEssai = essai.titreEssai,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = contextDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = contextDB.Users.First(u => u.Id == essai.compte_userID).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return Infos;
        }

        /// <summary>
        /// Obtenir toutes les réservations pour un essai
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>liste des réservations</returns>
        public List<InfosReservation> InfosReservations(int idEssai)
        {
            List<InfosReservation> ListResas = new List<InfosReservation>();
            // récupérer toutes les réservations du projet 
            var reservations = contextDB.reservation_projet.Where(r => r.essaiID == idEssai).Distinct().ToList();

            foreach (var x in reservations)
            {
                InfosReservation resa = new InfosReservation()
                {
                    DateDebut = x.date_debut,
                    DateFin = x.date_fin,
                    NomEquipement = contextDB.equipement.First(e => e.id == x.equipementID).nom,
                    ZoneEquipement = (from zon in contextDB.zone
                                      from equi in contextDB.equipement
                                      where zon.id == equi.zoneID && equi.id == x.equipementID
                                      select zon.nom_zone).First()
                };
                ListResas.Add(resa);
            }
            return ListResas;
        }
    }
}
