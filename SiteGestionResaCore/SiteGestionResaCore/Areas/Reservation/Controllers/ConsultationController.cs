using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Consultation;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.Reservation.Controllers
{
    [Area("Reservation")]
    public class ConsultationController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IConsultResasDB consultDB;
        private readonly IResaAValiderDb resaAValider;

        public ConsultationController(
            UserManager<utilisateur> userManager,
            IConsultResasDB consultDB,
            IResaAValiderDb resaAValiderDB)
        {
            this.userManager = userManager;
            this.consultDB = consultDB;
            this.resaAValider = resaAValiderDB;
        }

        /// <summary>
        /// Affichage des réservations validées
        /// </summary>
        /// <returns></returns>
        public IActionResult ResasValidees()
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = new InfosProjet(), 
                InfosEssai = new ConsultInfosEssaiChildVM(),
                Reservations = new List<InfosReservation>()
            };
            return View(model);
        }

        /// <summary>
        /// Action pour afficher les infos sur le projet d'un essai sur la liste
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public IActionResult VoirInfosProj(int id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = resaAValider.ObtenirInfosProjet(id),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                Reservations = new List<InfosReservation>()
            };
            ViewBag.modalProj = "show";
            return View("ResasValidees", model);
        }

        /// <summary>
        /// Action pour afficher les infos sur un essai de la liste 
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM model = resaAValider.ObtenirInfosEssai(id);
            return PartialView ("~/Views/Shared/_DisplayInfosEssai.cshtml", model);
        }

        /// <summary>
        /// Action pour afficher les réservations d'un essai sur la liste
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirReservations(int id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = new InfosProjet(),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                Reservations = resaAValider.InfosReservations(id)
            };
            ViewBag.modalResas = "show";
            return View("ResasValidees", model);
        }
    }
}