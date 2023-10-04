using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite;
using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Controllers
{
    [Area("AboutPFL")]

    public class DocQualiteController : Controller
    {
        private readonly IDocsQualiDB docsQualiDB;
        private readonly IRapportDB rapportDB;

        public DocQualiteController(
            IDocsQualiDB docsQualiDB,
            IRapportDB rapportDB)
        {
            this.docsQualiDB = docsQualiDB;
            this.rapportDB = rapportDB;
        }

        public IActionResult DocsQualiteXConsult()
        {
            DocQualiteViewModel vm = new DocQualiteViewModel()
            {
                ListDocsQualite = docsQualiDB.ListDocs()
            };

            return View("DocQualitePfl", vm);
        }

        public IActionResult TelechargerDocxId(int? id)
        {
            doc_qualite doc = docsQualiDB.ObtenirDocAQ(id.Value);
            return File(doc.contenu_doc_qualite, System.Net.Mime.MediaTypeNames.Application.Octet, doc.nom_document);
        }

        public IActionResult ListRapportsMétrologieXUsr()
        {
            RappXDownloadVM model = new RappXDownloadVM();
            model.ListCapteursXrapports = docsQualiDB.ListRapports();
            return View("ListRapportXDownloadUsr", model);
        }

        public IActionResult TelechargementRapportXUsr(int? id)
        {
            rapport_metrologie rapport = rapportDB.ObtenirRapport(id.Value);
            return File(rapport.contenu_rapport, System.Net.Mime.MediaTypeNames.Application.Octet, rapport.nom_document);
        }
    }
}
