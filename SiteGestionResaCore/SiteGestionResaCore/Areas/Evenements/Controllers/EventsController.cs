using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Controllers
{
    [Area("Evenements")]
    public class EventsController : Controller
    {
        public IActionResult AccueilEvenements()
        {
            return View("GestionEvenements");
        }
    }
}
