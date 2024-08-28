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

using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite
{
    public class DocsQualiDB : IDocsQualiDB
    {
        private readonly GestionResaContext context;

        public DocsQualiDB(
            GestionResaContext context)
        {
            this.context = context;
        }
        public List<DocumentQualite> ListDocs()
        {
            List<DocumentQualite> listXVue = new List<DocumentQualite>();
            var list = context.doc_qualite.ToList();
            foreach (var doc in list)
            {
                listXVue.Add(new DocumentQualite
                {
                    IdDocument = doc.id,
                    NomRubriqueDoc = doc.nom_rubrique_doc,
                    DescriptionDoc = doc.description_doc_qualite
                });
            }
            return listXVue;
        }

        public string GetNomDoc(string cheminDoc)
        {
            // Appliquer une regex pour extraire uniquement le nom
            string regexPatt = @"([^\\s]+)$";

            Regex Rg = new Regex(regexPatt);
            MatchCollection match = Rg.Matches(cheminDoc);

            return match[0].Groups[1].Value;
        }
        public doc_qualite ObtenirDocAQ(int IdDoc)
        {
            return context.doc_qualite.First(d => d.id == IdDoc);
        }

        public List<CapteurXRapport> ListRapports()
        {
            CapteurXRapport captXrapp = new CapteurXRapport();
            List<CapteurXRapport> list = new List<CapteurXRapport>();

            var rapports = context.rapport_metrologie.Distinct().ToList();
            foreach (var x in rapports)
            {
                var capt = context.capteur.First(c => c.id == x.capteurID);
                var equip = context.equipement.First(e => e.id == capt.equipementID);

                captXrapp = new CapteurXRapport
                {
                    idCapteur = capt.id,
                    nomCapteur = capt.nom_capteur,
                    nomEquipement = equip.nom,
                    numGmao = equip.numGmao,
                    dateVerif = x.date_verif_metrologie,
                    nom_document = x.nom_document,
                    idRapport = x.id,
                    TypeRapport = x.type_rapport_metrologie,
                    Commentaire = x.commentaire,
                    CodeCapteur = capt.code_capteur
                };
                list.Add(captXrapp);
            }
            return list;
        }
    }
}
