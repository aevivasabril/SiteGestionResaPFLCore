using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data.DocQualite;
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
            string cheminDoc = docsQualiDB.GetCheminDocQualite(id.Value);
            string nomDoc = docsQualiDB.GetNomDoc(cheminDoc);
            byte[] fileBytes = System.IO.File.ReadAllBytes(cheminDoc);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nomDoc);
        }
    }
}
