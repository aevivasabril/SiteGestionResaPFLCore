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
                ListIntervDansPFL = consultDb.ListIntervPFLEnCours(),
                ListIntervSansZone = consultDb.ListIntervSansZonesEnCours()
            };
            return View("ConsultationInterventions", vm);
        }

        public IActionResult ConsultIntervFinies()
        {
            ConsultInterventionVM vm = new ConsultInterventionVM()
            {
                ListIntervDansPFL = consultDb.ListIntervPFLFinies(),
                ListIntervSansZone = consultDb.ListIntervSansZoneFinies()
            };
            return View("ConsultInterventionsFinies", vm);
        }

        public IActionResult ConsultIntervSupprimees()
        {
            ConsultInterventionVM vm = new ConsultInterventionVM()
            {
                ListIntervDansPFL = consultDb.ListIntervPFLSupp(),
                ListIntervSansZone = consultDb.ListIntervSansZoneSupp()
            };
            return View("ConsultInterventionsSupprimees", vm);
        }
    }
}
