using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Services;

namespace SiteGestionResaCore.Areas.Reservation.Controllers
{
    [Area("Reservation")]
    public class ValidationController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IEmailSender emailSender;
        private readonly IResaAValiderDb resaAValiderDb;

        public ValidationController(
            UserManager<utilisateur> userManager,
            IEmailSender emailSender,
            IResaAValiderDb resaAValiderDb)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.resaAValiderDb = resaAValiderDb;
        }
        /// <summary>
        /// Affichage page pour sélection des sous rubriques
        /// </summary>
        /// <returns></returns>
        public IActionResult MenuConsultation()
        {
            return View();
        }

        /// <summary>
        /// Action pour afficher la page "Réservations à valider"
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ReservationsAValiderAsync(ResasPourValidationViewModel ResaVm)
        {
            ResaVm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = new InfosProjet(),
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>()                               
            };
            
            return View(ResaVm);
        }

        /// <summary>
        /// Action 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> VoirInfosEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = resaAValiderDb.ObtenirInfosEssai(id),
                InfosProj = new InfosProjet(),
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>()
            };
            ViewBag.modalEssai = "show";
            return View("ReservationsAValider", vm);
        }

        public async Task<IActionResult> VoirInfosProjetAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = resaAValiderDb.ObtenirInfosProjet(id),
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>()
            };
            ViewBag.modalProj = "show";
            return View("ReservationsAValider", vm);
        }

        public async Task<IActionResult> ConsulterResasProjetAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = new InfosProjet(),
                Reservations = resaAValiderDb.InfosReservations(id),
                InfosConflits = new List<InfosConflit>()
            };
            ViewBag.modalResas = "show";
            return View("ReservationsAValider", vm);
        }

        public async Task<IActionResult> VoirConflitEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = new InfosProjet(),
                Reservations = new List<InfosReservation>(),
                InfosConflits = resaAValiderDb.InfosConflits(id),
                IdEss = id
            };
            ViewBag.modalConflit = "show";
            return View("ReservationsAValider", vm);
        }

        public async Task<IActionResult> ValiderEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = resaAValiderDb.ObtenirInfosProjetFromEssai(id), //obtenir les infos projet à partir de l'id essai je l'utilise pour l'affichage!
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>(),
                IdEss = id
            };
            ViewBag.modalValid = "show";
            return View("ReservationsAValider", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ValiderEssaiAsync(ResasPourValidationViewModel vm)
        {
            bool isValidOk = resaAValiderDb.ValiderEssai(vm.IdEss);
            if(isValidOk)
            {
                ViewBag.SuccessMessage = "Ok";
                ViewBag.Action = "Validation";
            }
            else
            {
                ModelState.AddModelError("", "Problème lors de la validation réservation. Veuillez essayer ultérieurement");               
            }
            vm.resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync();
            vm.InfosEssai = new InfosEssai();
            vm.InfosProj = new InfosProjet();
            vm.Reservations = new List<InfosReservation>();
            vm.InfosConflits = new List<InfosConflit>();
            vm.IdEss = vm.IdEss;

            return View("ReservationsAValider", vm);
        }

        public async Task<IActionResult> RefuserEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new InfosEssai(),
                InfosProj = resaAValiderDb.ObtenirInfosProjetFromEssai(id), //obtenir les infos projet à partir de l'id essai
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>(),
                IdEss = id
            };
            ViewBag.modalRefus = "show";
            return View("ReservationsAValider", vm);
        }

        [HttpPost]
        public async Task<IActionResult> RefuserEssaiAsync(ResasPourValidationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                bool isRefusOk = resaAValiderDb.RefuserEssai(vm.IdEss, vm.RaisonRefus);
                if (isRefusOk)
                {
                    ViewBag.SuccessMessage = "Ok";
                    ViewBag.Action = "Refus";
                }
                else
                {
                    ModelState.AddModelError("", "Problème pour refuser la réservation. Veuillez essayer ultérieurement");
                }
            }
            else
            {
                ViewBag.modalRefus = "show";
            }
            vm.resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync();
            vm.InfosEssai = new InfosEssai();
            vm.InfosProj = new InfosProjet();
            vm.Reservations = new List<InfosReservation>();
            vm.InfosConflits = new List<InfosConflit>();
            vm.IdEss = vm.IdEss;

            return View("ReservationsAValider", vm);
        }
    }
}