using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Data.PcVue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.RecupData
{
    public class DataAdminDB: IDataAdminDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<DataAdminDB> logger;
        private readonly PcVueContext pcVueDb;

        public DataAdminDB(
            GestionResaContext resaDB,
            ILogger<DataAdminDB> logger,
            PcVueContext pcVueDb)
        {
            this.resaDB = resaDB;
            this.logger = logger;
            this.pcVueDb = pcVueDb;
        }

        public List<InfosResasAdm> ObtInfosResasAdms()
        {
            List<InfosResasAdm> List = new List<InfosResasAdm>();
            InfosResasAdm infos = new InfosResasAdm();
            bool IsEquipUnderPcVue = false;
            DateTime date = DateTime.Now;
            date = date.AddYears(-1);
            try
            {
                // Récuperer tous les essais dont la date de création est égal ou inferieure à un an pour éviter au futur de surcharger la table
                var essais = resaDB.essai.Where(e => e.date_creation >= date && e.resa_refuse == null && e.resa_supprime == null).ToList().Distinct(); // list de tous les essais 
                foreach (var ess in essais)
                {
                    // Récupérer toutes les réservations pour cet essai
                    var resasEssai = resaDB.reservation_projet.Where(r => r.essaiID == ess.id).ToList();

                    // Determiner si au moins un des équipements est sous supervision de données
                    foreach (var resa in resasEssai)
                    {
                        IsEquipUnderPcVue = (resaDB.equipement.First(e => e.id == resa.equipementID).nomTabPcVue != null);
                        if (IsEquipUnderPcVue == true)
                            break;
                    }

                    infos = new InfosResasAdm
                    {
                        NomProjet = resaDB.projet.First(p => p.id == ess.projetID).titre_projet,
                        NumProjet = resaDB.projet.First(p => p.id == ess.projetID).num_projet,
                        IdEssai = ess.id,
                        TitreEssai = ess.titreEssai,
                        EquipementSousPcVue = IsEquipUnderPcVue,
                        DateCreationEssai = ess.date_creation,
                        MailPropietaireEssai = resaDB.Users.Find(ess.compte_userID).Email
                    };
                    List.Add(infos);
                }
                return List.OrderByDescending(x=>x.DateCreationEssai).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return null;
            }    
        }
    }
}
