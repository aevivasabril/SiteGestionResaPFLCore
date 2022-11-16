using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Metrologie.Data;
using System;
using System.Collections.Generic;
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
            return View("AccueilMetrologie");
        }
    }
}
