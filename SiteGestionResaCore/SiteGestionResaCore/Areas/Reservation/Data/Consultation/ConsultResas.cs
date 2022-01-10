using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Consultation
{
    /// <summary>
    /// Classe d'accès aux infos d'une réservation validée
    /// </summary>
    public class ConsultResasDB : IConsultResasDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<ConsultResasDB> logger;

        public ConsultResasDB(
            GestionResaContext resaDB,
            ILogger<ConsultResasDB> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        /// <summary>
        /// méthode pour récupérer les infos sur les essais validées par les admins
        /// </summary>
        /// <returns>liste des infos à afficher pour les réservations validées</returns>
        public IList<InfosResasValid> ObtInfEssaiValidees()
        {
            List<InfosResasValid> list = new List<InfosResasValid>();
            // Obtenir tous les essais "VALIDEES"
            var Reg = (from proj in resaDB.projet
                        join ess in resaDB.essai on proj.id equals ess.projetID into t1
                        from m in t1.DefaultIfEmpty()
                        where m.status_essai == EnumStatusEssai.Validate.ToString()                    
                        select new
                        {
                            idEssai = m.id,
                            DateValidation = m.date_validation.Value,
                            MailRespProj = proj.mailRespProjet,
                            titreEssai = m.titreEssai,
                            NomProjet = proj.titre_projet,
                            NumProjet = proj.num_projet,
                            DateSaisie = m.date_creation,
                            IdProj = proj.id,
                            Confi = m.confidentialite,
                        }).Distinct();

            foreach (var x in Reg)
            {
                InfosResasValid infos = new InfosResasValid {
                    TitreEssai = x.titreEssai, DateSaisieEssai = x.DateSaisie, 
                    DateValidation = x.DateValidation, idEssai = x.idEssai, MailRespProj = x.MailRespProj,
                    NomProjet = x.NomProjet, NumProjet = x.NumProjet, idProj = x.IdProj, Confidentialit = x.Confi};

                list.Add(infos);
            }
            return list.OrderByDescending(x=>x.DateValidation).ToList(); // Ordonner la liste par ordre chronologique descendant
        }

        /// <summary>
        /// Méthode pour récupérer la liste des essais refusés par les administrateurs
        /// </summary>
        /// <returns></returns>
        public IList<InfosResaNonValid> ObtInfosEssaisRefusees()
        {
            List<InfosResaNonValid> list = new List<InfosResaNonValid>();
            // Obtenir tous les essais "REFUSES"
            var Reg = (from proj in resaDB.projet
                       join ess in resaDB.essai on proj.id equals ess.projetID into t1
                       from m in t1.DefaultIfEmpty()
                       where m.status_essai == EnumStatusEssai.Refuse.ToString()
                       select new
                       {
                           idEssai = m.id,
                           RaisonRefuse = m.raison_refus,
                           MailRespProj = proj.mailRespProjet,
                           titreEssai = m.titreEssai,
                           NomProjet = proj.titre_projet,
                           NumProjet = proj.num_projet,
                           DateSaisie = m.date_creation,
                           IdProj = proj.id,
                           Confi = m.confidentialite,
                       }).Distinct();

            foreach (var x in Reg)
            {
                InfosResaNonValid infos = new InfosResaNonValid
                {
                    TitreEssai = x.titreEssai,
                    DateSaisieEssai = x.DateSaisie,
                    RaisonRefus = x.RaisonRefuse,
                    idEssai = x.idEssai,
                    MailRespProj = x.MailRespProj,
                    NomProjet = x.NomProjet,
                    NumProjet = x.NumProjet,
                    idProj = x.IdProj,
                    Confidentialit = x.Confi
                };

                list.Add(infos);
            }
            return list.OrderByDescending(x => x.DateSaisieEssai).ToList(); // Ordonner la liste par ordre chronologique descendant
        }

        /// <summary>
        /// Méthode pour obtenir la list des essais supprimés par les utilisateurs
        /// </summary>
        /// <returns></returns>
        public IList<InfosResaNonValid> ObtInfosEssaisSupprimees()
        {
            List<InfosResaNonValid> list = new List<InfosResaNonValid>();
            // Obtenir tous les essais "SUPPRIMES"
            var Reg = (from proj in resaDB.projet
                       join ess in resaDB.essai on proj.id equals ess.projetID into t1
                       from m in t1.DefaultIfEmpty()
                       where m.status_essai == EnumStatusEssai.Canceled.ToString()
                       select new
                       {
                           idEssai = m.id,
                           RaisonSupp = m.raison_suppression,
                           MailRespProj = proj.mailRespProjet,
                           titreEssai = m.titreEssai,
                           NomProjet = proj.titre_projet,
                           NumProjet = proj.num_projet,
                           DateSaisie = m.date_creation,
                           IdProj = proj.id,
                           Confi = m.confidentialite,
                       }).Distinct();

            foreach (var x in Reg)
            {
                InfosResaNonValid infos = new InfosResaNonValid
                {
                    TitreEssai = x.titreEssai,
                    DateSaisieEssai = x.DateSaisie,
                    RaisonSuppression = x.RaisonSupp,
                    idEssai = x.idEssai,
                    MailRespProj = x.MailRespProj,
                    NomProjet = x.NomProjet,
                    NumProjet = x.NumProjet,
                    idProj = x.IdProj,
                    Confidentialit = x.Confi
                };

                list.Add(infos);
            }
            return list.OrderByDescending(x => x.DateSaisieEssai).ToList(); // Ordonner la liste par ordre chronologique descendant
        }    
    }
}
