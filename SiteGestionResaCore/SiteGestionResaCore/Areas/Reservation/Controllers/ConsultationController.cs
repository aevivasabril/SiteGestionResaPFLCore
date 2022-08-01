using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Consultation;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
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
                //InfosProjet = new InfosProjet(), 
                //InfosEssai = new ConsultInfosEssaiChildVM(),
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
            /*ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                InfosProjet = resaAValider.ObtenirInfosProjet(id),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                Reservations = new List<InfosReservation>()
            };
            ViewBag.modalProj = "show";*/
            InfosProjet vm = resaAValider.ObtenirInfosProjet(id);
            return PartialView("~/Views/Shared/_DisplayInfosProjet.cshtml", vm);
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
            EquipementsReservesVM vm = new EquipementsReservesVM()
            {
                Reservations = resaAValider.InfosReservations(id)
            };
            return PartialView("~/Views/Shared/_DisplayEquipsReserves.cshtml", vm);
        }

        public IActionResult ResasRefusees()
        {
            ConsultResasNonValidViewModel model = new ConsultResasNonValidViewModel()
            {
                ResasNonValid = consultDB.ObtInfosEssaisRefusees()
            };
            return View("ResasRefusees", model);
        }

        public IActionResult ResasSupprimees()
        {
            ConsultResasNonValidViewModel model = new ConsultResasNonValidViewModel()
            {
                ResasNonValid = consultDB.ObtInfosEssaisSupprimees()
            };
            return View("ResasSupprimees", model);
        }

        public IActionResult SupprimerEssaiAdm(int? id)
        {
            ConsultationViewModel model = new ConsultationViewModel()
            {
                ResasValid = consultDB.ObtInfEssaiValidees(),
                IdEssai = id.Value,
                TitreEssai = consultDB.ObtenirEssai(id.Value).titreEssai,
                Reservations = new List<InfosReservation>()
            };
            // Sauvegarder la session data consultation
            this.HttpContext.AddToSession("ConsultationViewModel", model);

            ViewBag.modalAnnul = "show";
            return View("ResasValidees", model);
        }

        [HttpPost]
        public IActionResult ConfSuppEssaiAdm(int id, ConsultationViewModel model)
        {
            // Récupérer la session "ConsultationViewModel"
            ConsultationViewModel vm = HttpContext.GetFromSession<ConsultationViewModel>("ConsultationViewModel");

            if (ModelState.IsValid)
            {
                bool IsAnnulationOk = consultDB.AnnulerEssaiAdm(id, model.RaisonAnnulation);
                if (IsAnnulationOk)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Essai supprimé avec succès! ";
                    vm.ResasValid = consultDB.ObtInfEssaiValidees();
                }
                else
                {
                    ModelState.AddModelError("", "Problème pour annuler la réservation. Veuillez essayer ultérieurement");
                }
            }
            else
            {

                ViewBag.modalAnnul = "show";
            }
            return View("ResasValidees", vm);
        }
    }
}