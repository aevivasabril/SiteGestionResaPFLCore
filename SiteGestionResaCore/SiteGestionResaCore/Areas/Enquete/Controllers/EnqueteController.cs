using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Enquete.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Controllers
{
    [Area("Enquete")]
    public class EnqueteController : Controller
    {
        private readonly IEnqueteDb EnqueteDb;
        private readonly IEmailSender emailSender;

        public EnqueteController(
            IEnqueteDb EnqueteDb,
            IEmailSender emailSender)
        {
            this.EnqueteDb = EnqueteDb;
            this.emailSender = emailSender;
        }
        public IActionResult EnqueteSatisfaction()
        {
            //TODO: effacer j'essai de forcer le titre essai pour éviter les erreurs
            int id = 13;
            essai ess = EnqueteDb.ObtenirEssai(id);
            projet pr = EnqueteDb.ObtenirProjet(ess);

            EnqueteViewModel vm = new EnqueteViewModel
            {
                TitreEssai = ess.titreEssai, 
                IdEssai = ess.id,
                NumProjet = pr.num_projet,
                TitreProj = pr.titre_projet
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EnvoyerEnquete(EnqueteViewModel vm, int id)
        {
            return View();
        }
    }
}
