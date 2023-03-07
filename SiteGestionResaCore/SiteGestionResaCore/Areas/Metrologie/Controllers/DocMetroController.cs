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
