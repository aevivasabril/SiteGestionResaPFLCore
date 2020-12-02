using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.User.Data;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class ResasUserController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IResasUserDB resasUserDB;

        public ResasUserController(
            UserManager<utilisateur> userManager, 
            IResasUserDB resasUserDB)
        {
            this.userManager = userManager;
            this.resasUserDB = resasUserDB;
        }
        public async Task<IActionResult> MesReservationsAsync()
        {
            // trouver l'utilisateur connecté
            var user = await userManager.FindByIdAsync(User.GetUserId());

            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = resasUserDB.ObtenirResasUser(user.Id)
            };
            return View(vm);
        }
    }
}