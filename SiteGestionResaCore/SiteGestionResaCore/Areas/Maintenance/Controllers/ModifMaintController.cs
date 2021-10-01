using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Maintenance.Data.Modification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Controllers
{
    [Area("Maintenance")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class ModifMaintController : Controller
    {
        public IActionResult ModificationIntervention()
        {
            ModifMaintenanceVM vm = new ModifMaintenanceVM();
            return View("ModificationIntervention", vm);
        }

        [HttpPost]
        public IActionResult TrouverIntervention(ModifMaintenanceVM vm)
        {

            return View();
        }
    }
}
