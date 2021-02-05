using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public class DonneesUsrDB: IDonneesUsrDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<DonneesUsrDB> logger;

        public DonneesUsrDB(
            GestionResaContext resaDB,
            ILogger<DonneesUsrDB> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        public List<InfosResa> ObtenirResasUser(int IdUsr)
        {
            List<InfosResa> List = new List<InfosResa>();
            InfosResa infos = new InfosResa();
            bool IsEquipUnderPcVue = false;

            var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr).ToList();

            foreach (var i in essaiUsr)
            {
                // Récupérer toutes les réservations pour cet essai
                var resasEssai = resaDB.reservation_projet.Where(r => r.essaiID == i.id).ToList();

                // Determiner si au moins un des équipements est sous supervision de données
                foreach (var resa in resasEssai)
                {
                    IsEquipUnderPcVue = (resaDB.equipement.First(e => e.id == resa.equipementID).nomTabPcVue != null);
                    if (IsEquipUnderPcVue == true)
                        break;
                }

                infos = new InfosResa
                {
                    NomProjet = resaDB.projet.First(p => p.id == i.projetID).titre_projet,
                    NumProjet = resaDB.projet.First(p => p.id == i.projetID).num_projet,
                    IdEssai = i.id,
                    EquipementSousPcVue = IsEquipUnderPcVue

                };
                //IsEquipUnderPcVue = false;   
                List.Add(infos);
            }
            return List;
        }

        public ConsultInfosEssaiChilVM ObtenirInfosEssai(int IdEssai)
        {
            var essai = resaDB.essai.First(e => e.id == IdEssai);
            ConsultInfosEssaiChilVM vm = new ConsultInfosEssaiChilVM
            {
                id = essai.id,
                Commentaire = essai.commentaire,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = resaDB.Users.First(u => u.Id == essai.compte_userID).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return vm;
        }
    }
}
