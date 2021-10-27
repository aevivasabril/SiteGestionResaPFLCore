using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Maintenance.Data.Consultation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Controllers
{
    // Autoriser les utilisateurs pour accès
    [Area("Maintenance")]
    public class ConsultMaintController : Controller
    {
        private readonly IConsultMaintDb consultDb;
        //private readonly IEmailSender emailSender;

        public ConsultMaintController(
            IConsultMaintDb consultDb)
        {
            this.consultDb = consultDb;
            //this.emailSender = emailSender;
        }
        public IActionResult ConsultInterventions()
        {
            ConsultInterventionVM vm = new ConsultInterventionVM()
            {
                ListIntervDansPFL = consultDb.ListIntervPFL(),
                ListIntervSansZone = consultDb.ListIntervSansZones()
            };
            return View("ConsultationInterventions", vm);
        }
    }
}
