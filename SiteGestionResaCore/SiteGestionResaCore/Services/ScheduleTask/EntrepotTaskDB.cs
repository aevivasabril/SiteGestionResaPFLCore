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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EntrepotTaskDB: IEntrepotTaskDB
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly GestionResaContext resaDb;
        private readonly ILogger<EntrepotTaskDB> logger;

        public EntrepotTaskDB(
            UserManager<utilisateur> userManager,
            GestionResaContext resaDb,
            ILogger<EntrepotTaskDB> logger)
        {
            this.userManager = userManager;
            this.resaDb = resaDb;
            this.logger = logger;
        }
        /// <summary>
        /// Obtenir la liste des projets dont la date de création est supérieure ou égal à 2 ans 11 mois de la date actuelle et inferieur à 3 ans pour notification
        /// </summary>
        /// <returns></returns>
        public List<projet> GetProjetsXNotification()
        {
            // 1 an et 11 mois = 700 jours environ
            // 2 ans            = 730 jours environ
            List<projet> list = new List<projet>();
            int LimitInf = 700;
            int LimitSup = 730;
            var listProj = resaDb.projet.Where(p => p.entrepot_supprime == null && p.date_creation_entrepot != null);

            foreach (var p in listProj)
            {
                TimeSpan diff = DateTime.Now - p.date_creation_entrepot.Value;
                // Si le projet rentre dans la condition alors on envoie un mail
                if(diff.Days > LimitInf & diff.Days < LimitSup)
                {
                    list.Add(p);
                }         
            }
            return list;
        }

        public List<projet> GetProjetsXSuppression()
        {
            List<projet> list = new List<projet>();
            int LimitSup = 730;

            var listP = resaDb.projet.Where(p => p.entrepot_supprime == null && p.date_creation_entrepot != null);

            foreach(var p in listP)
            {
                TimeSpan diff = DateTime.Now - p.date_creation_entrepot.Value;
                if(diff.Days >= LimitSup)
                {
                    list.Add(p);
                }
            }

            return list;
        }

        public bool SupprimerEntrepotXProj(projet pr)
        {
            bool isOk = false;

            var essais = resaDb.essai.Where(e => e.projetID == pr.id && e.entrepot_cree == true).ToList();           
            foreach(var ess in essais)
            {
                var docList = resaDb.doc_essai_pgd.Where(d => d.essaiID == ess.id).ToList();
                foreach (var dc in docList)
                {
                    try
                    {
                        resaDb.doc_essai_pgd.Remove(dc);
                        resaDb.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.ToString(), "Problème lors de la suppression du document automatique (task)");
                        return false;
                    }
                }
                
                try
                {
                    // Signaler que l'entrepot des données a été supprimé pour cet essai
                    var proj = resaDb.projet.First(p => p.id == pr.id);
                    proj.entrepot_supprime = true;
                    resaDb.SaveChanges();
                    isOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString(), "Problème lors de la mise à jour pour indiquer qu'un entrepôt a été supprimé pour ce projet (task)");
                    return false;
                }
            }

            return isOk;
        }
    }
}
