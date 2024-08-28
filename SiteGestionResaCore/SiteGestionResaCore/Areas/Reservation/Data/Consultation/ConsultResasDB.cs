/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
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
                           dateValidation = m.date_validation,
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
                    Confidentialit = x.Confi,
                    DateValidation = x.dateValidation.GetValueOrDefault()
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
                           DateValid = m.date_validation,
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
                    Confidentialit = x.Confi,
                    DateValidation = x.DateValid.GetValueOrDefault()
                };

                list.Add(infos);
            }
            return list.OrderByDescending(x => x.DateSaisieEssai).ToList(); // Ordonner la liste par ordre chronologique descendant
        }

        public essai ObtenirEssai(int id)
        {
            return resaDB.essai.First(e => e.id == id);
        }

        public bool AnnulerEssaiAdm(int id, string raisonAnnulation)
        {
            bool IsChangeOk = false;

            int retry = 0;

            var essai = resaDB.essai.First(e => e.id == id);
            essai.status_essai = EnumStatusEssai.Canceled.ToString();
            essai.date_validation = DateTime.Now;
            essai.resa_supprime = true;
            essai.raison_suppression = "Supprimé par un Admin: " + raisonAnnulation;
            while (retry < 3 && IsChangeOk != true)
            {
                try
                {
                    resaDB.SaveChanges();
                    IsChangeOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Erreur lors de la validation de l'essai N° :" + id);
                    retry++;
                }
            }

            return IsChangeOk;
        }
    }
}
