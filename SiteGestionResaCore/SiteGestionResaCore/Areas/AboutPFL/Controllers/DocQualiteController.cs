using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite;
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

        public DocQualiteController(
            IDocsQualiDB docsQualiDB)
        {
            this.docsQualiDB = docsQualiDB;
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
    }
}
