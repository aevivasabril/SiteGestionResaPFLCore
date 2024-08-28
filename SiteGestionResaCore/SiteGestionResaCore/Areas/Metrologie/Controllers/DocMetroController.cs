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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Metrologie.Data;
using SiteGestionResaCore.Areas.Metrologie.Data.DocMetro;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Controllers
{
    [Area("Metrologie")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class DocMetroController : Controller
    {
        private readonly IDocMetroDB docMetroDB;

        public DocMetroController(
            IDocMetroDB docMetroDB)
        {
            this.docMetroDB = docMetroDB;
        }

        public IActionResult AccueilMetrologie()
        {
            AccueilVM vm = new AccueilVM();
            vm.ListCapteurXVerif = docMetroDB.ListProchainesVerifs();

            return View("AccueilMetrologie", vm);
        }

        public IActionResult DocMetrologie()
        {
            DocMetroVM vm = new DocMetroVM();
            vm.docs = docMetroDB.GetListDocuments();
            this.HttpContext.AddToSession("DocMetroVM", vm);

            return View("DocMetrologie", vm);
        }

        [HttpPost]
        public IActionResult ChargerDocument()
        {
            ViewBag.ModalAddDocMetro = "show";
            DocMetroVM vm = HttpContext.GetFromSession<DocMetroVM>("DocMetroVM");
            return View("DocMetrologie", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnvoiChargeDocAsync(IFormFile file, string description)
        {
            DocumentMetrologie doc = new DocumentMetrologie();

            DocMetroVM vm = HttpContext.GetFromSession<DocMetroVM>("DocMetroVM");
            // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
            using (var datastream = new MemoryStream())
            {
                await file.CopyToAsync(datastream);
                doc.ContenuDoc = datastream.ToArray();               
            }
            doc.DescriptionDoc = description;
            doc.NomDocument = file.FileName.ToString();
            doc.dateAjout = DateTime.Now;

            // ajouter document dans la base des données
            bool isOk = docMetroDB.AddingDocMetrologie(doc);
            if (!isOk)
            {
                ModelState.AddModelError("", "Problème pour enregistrer le nouveau document métrologie dans la BDD");
                //ViewBag.ModalAddingDoc = "show";
                return View("DocMetrologie",vm);
            }
            vm.docs = docMetroDB.GetListDocuments();
            this.HttpContext.AddToSession("DocMetroVM", vm);

            return View("DocMetrologie", vm);
        }

        public IActionResult OuvrirDoc(int? id)
        {
            doc_metrologie doc = docMetroDB.ObtenirDocMetro(id.Value);
            return File(doc.contenu_doc, System.Net.Mime.MediaTypeNames.Application.Octet, doc.nom_document);
        }

        public IActionResult SupprimerDoc(int? id)
        {
            // Supprimer document métrologie
            bool isOk = docMetroDB.SupprimerDoc(id.Value);
            DocMetroVM vm = HttpContext.GetFromSession<DocMetroVM>("DocMetroVM");

            if (!isOk)
            {
                ModelState.AddModelError("", "Problème pour enregistrer le nouveau document métrologie dans la BDD");
                return View("DocMetrologie", vm);
            }

            vm.docs = docMetroDB.GetListDocuments();
            this.HttpContext.AddToSession("DocMetroVM", vm);

            return View("DocMetrologie", vm);
        }
    }
}
