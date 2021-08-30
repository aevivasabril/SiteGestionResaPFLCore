using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
