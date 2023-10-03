using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.User.Data.Profil;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class ProfilController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IProfilDB profilDB;

        public ProfilController(
            UserManager<utilisateur> userManager,
            IProfilDB profilDB)
        {
            this.userManager = userManager;
            this.profilDB = profilDB;
        }

        public async Task<IActionResult> ProfilUserAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            ProfilUserVM vm = new ProfilUserVM();
            vm.NomUsr = user.nom;
            vm.PrenomUsr = user.prenom;
            vm.NomEquipe = profilDB.ObtNomEquipe(user.equipeID.Value);
            vm.NomOrganisme = profilDB.ObtNomOrganisme(user.organismeID.Value);
            vm.MailUsr = user.Email;
            vm.ListEnquetesNonRepondues = profilDB.ObtListEnquetes(user);
            return View("ProfilUser", vm);
        }
    }
}
