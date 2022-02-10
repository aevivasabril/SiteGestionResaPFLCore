using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    public class AccesEntrepotController : Controller
    {
        public IActionResult CreationEntrepot()
        {
            return View();
        }

        public IActionResult ConsulterEntrepots()
        {
            return View();
        }
    }
}
