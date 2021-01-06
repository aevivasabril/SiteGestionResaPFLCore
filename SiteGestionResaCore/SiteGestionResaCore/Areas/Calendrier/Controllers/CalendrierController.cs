using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Calendrier.Data;

namespace SiteGestionResaCore.Areas.Calendrier.Controllers
{
    [Area("Calendrier")]
    public class CalendrierController : Controller
    {
        private readonly ICalendResaDb CalendResaDb;

        public CalendrierController(
           ICalendResaDb CalendResaDb)
        {
            this.CalendResaDb = CalendResaDb;
        }

        public IActionResult CalendrierPFL()
        {
            CalenViewModel vm = new CalenViewModel()
            {
                InfosZoneCalendrier = CalendResaDb.ObtenirZonesVsEquipements()
            };
            return View(vm);
        }
    }
}