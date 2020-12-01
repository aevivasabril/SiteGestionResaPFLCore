using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Consultation;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;

namespace SiteGestionResaCore.Areas.Reservation.Controllers
{
    [Area("Reservation")]
    public class ConsultationController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IConsultResas consultDB;
        private readonly IResaAValiderDb resaAValider;

        public ConsultationController(
            UserManager<utilisateur> userManager,
            IConsultResas consultDB,
            IResaAValiderDb resaAValiderDB)
        {
            this.userManager = userManager;
            this.consultDB = consultDB;
            this.resaAValider = resaAValiderDB;
        }

        public IActionResult ResasValidees()
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = new InfosProjet(), 
                InfosEssai = new InfosEssai(),
                Reservations = new List<InfosReservation>()
            };
            return View(model);
        }

        public IActionResult VoirInfosProj(int id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = resaAValider.ObtenirInfosProjet(id),
                InfosEssai = new InfosEssai(),
                Reservations = new List<InfosReservation>()
            };
            ViewBag.modalProj = "show";
            return View("ResasValidees", model);
        }

        public IActionResult VoirInfosEssai(int id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = new InfosProjet(), 
                InfosEssai = resaAValider.ObtenirInfosEssai(id),
                Reservations = new List<InfosReservation>()
            };
            ViewBag.modalEssai = "show";
            return View("ResasValidees", model);
        }

        public IActionResult VoirReservations(int id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = new InfosProjet(),
                InfosEssai = new InfosEssai(),
                Reservations = resaAValider.InfosReservations(id)
            };
            ViewBag.modalResas = "show";
            return View("ResasValidees", model);
        }
    }
}