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

namespace SiteGestionResaCore.Areas.Equipe.Data.ModifDocAq
{
    public class ModifAqDB : IModifAqDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<ModifAqDB> logger;

        public ModifAqDB(
            GestionResaContext contextDb,
            ILogger<ModifAqDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        public List<DocQualiteToModif> ObtenirDocsAQXModif()
        {
            List<DocQualiteToModif> list = new List<DocQualiteToModif>();
            var sublist = contextDb.doc_qualite.ToList();
            foreach(var d in sublist)
            {
                DocQualiteToModif doc = new DocQualiteToModif
                {
                    IdDoc = d.id,
                    NomDocument = d.nom_document,
                    DernierDateModif = d.date_modif_doc.Value,
                    DescripDoc = d.description_doc_qualite,
                    NomRubriqDoc = d.nom_rubrique_doc
                };
                list.Add(doc);
            }
            return list;
        }

        public bool AjouterDocToBDD(DocQualiteToModif doc)
        {
            bool isOk = false;
            try
            {
                doc_qualite docQ = new doc_qualite
                {
                    contenu_doc_qualite = doc.ContenuDoc,
                    date_modif_doc = doc.DernierDateModif,
                    description_doc_qualite = doc.DescripDoc,
                    nom_document = doc.NomDocument,
                    nom_rubrique_doc = doc.NomRubriqDoc
                };
                contextDb.doc_qualite.Add(docQ);
                contextDb.SaveChanges();
                isOk = true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Erreur d'écriture du document dans la table doc_essai_pgd: " + e.ToString());
                isOk = false;
            }

            return isOk;
        }

        public doc_qualite ObtenirDocQualite(int id)
        {
            return contextDb.doc_qualite.First(d => d.id == id);
        }

        public bool SupprimerDocAQ(int IdDoc)
        {
            bool IsOk = false;
            try
            {
                doc_qualite doc = contextDb.doc_qualite.First(d => d.id == IdDoc);
                contextDb.doc_qualite.Remove(doc);
                contextDb.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "Erreur lors de la suppression du document qualité: "+ e.ToString());
                IsOk = false;
            }
            return IsOk;
        }
    }
}
