using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Controllers
{
    [Area("Equipe")]
    public class ModifDocAqController : Controller
    {
        public IActionResult IndexModifDocsAQ()
        {
            return View();
        }
    }
}
