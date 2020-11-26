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
        public async Task<IActionResult> ReservationsAValiderAsync()
        {
            ResasPourValidationViewModel ResaVm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.InfosAffichageAsync(),
                infosEssai = new InfosEssai()
            };
            
            return View(ResaVm);
        }

        /// <summary>
        /// Action 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> VoirInfosEssaiAsync(int? id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.InfosAffichageAsync(),
                infosEssai = resaAValiderDb.InfosEssai(id.Value)
            };
            ViewBag.modalEssai = "show";
            return View("ReservationsAValider", vm);
        }
    }
}