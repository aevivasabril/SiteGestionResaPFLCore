using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.StatistiquePFL.Data;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Controllers
{
    [Area("StatistiquePFL")]
    public class StatsController : Controller
    {
        public IActionResult AccueilStats()
        {
            AccueilStatsVM vm = new AccueilStatsVM();
            return View("AccueilStats", vm);
        }

        [HttpPost]
        public IActionResult GenererExcelResas(AccueilStatsVM vm)
        {
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;

            if (vm.DateAu != null && vm.DateDu != null) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (vm.DateDu.Value <= vm.DateAu.Value)
                {

                }
                else
                {
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                }
            }
            else
            {
                ModelState.AddModelError("", "Oups! Vous avez oublié de saisir les dates! ");
            }

            return View("AccueilStats", vm);
        }
    }
}
