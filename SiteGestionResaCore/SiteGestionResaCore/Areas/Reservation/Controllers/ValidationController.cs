using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;
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
        public IActionResult MenuConsultation()
        {
            return View();
        }

        public IActionResult ReservationsAValider()
        {
            ResasPourValidationViewModel ResaVm = new ResasPourValidationViewModel()
            {
                resasAValider = resaAValiderDb.ResasAValider()
            };
            return View(ResaVm);
        }
    }
}