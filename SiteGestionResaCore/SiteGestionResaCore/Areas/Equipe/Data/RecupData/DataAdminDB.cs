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
