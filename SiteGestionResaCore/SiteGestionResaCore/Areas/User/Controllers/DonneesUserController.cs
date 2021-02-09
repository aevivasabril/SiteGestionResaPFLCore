using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.User.Data.DonneesUser;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class DonneesUserController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IDonneesUsrDB donneesUsrDB;

        public DonneesUserController(
            UserManager<utilisateur> userManager,
            IDonneesUsrDB donneesUsrDB)
        {
            this.userManager = userManager;
            this.donneesUsrDB = donneesUsrDB;
        }

        public async Task<IActionResult> ListEssaisDonneesAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            ListResasDonneesVM vm = new ListResasDonneesVM()
            {
                ResasUser = donneesUsrDB.ObtenirResasUser(user.Id),
                EquipVsDonnees = new EquipVsDonneesVM(),
                ConsultInfosEssai = new ConsultInfosEssaiChilVM()
            };
            return View(vm);
        }

        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChilVM vm = donneesUsrDB.ObtenirInfosEssai(id);
            vm.ActionName = "ListEssaisDonnees";
            return PartialView("_DisplayInfosEssai", vm);
        }
        

        public IActionResult ListEquipVsDonnees(int id) 
        {
            // id essai
            EquipVsDonneesVM vm = new EquipVsDonneesVM();
            List<InfosResasEquipement> ListResa = donneesUsrDB.ListEquipVsDonnees(id);
            vm.EquipementsReserves = ListResa;
            vm.IdEssai = id;
            return PartialView("_EquipVsDonnees", vm);
        }
    }
}
