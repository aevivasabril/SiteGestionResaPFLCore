using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.DonneesPGD.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Areas.DonneesPGD.Controllers
{
    [Area("DonneesPGD")]
    public class AccesEntrepotController : Controller
    {
        private readonly IEssaisXEntrepotDB entrepotDB;
        private readonly UserManager<utilisateur> userManager;

        public AccesEntrepotController(
            IEssaisXEntrepotDB entrepotDB,
            UserManager<utilisateur> userManager)
        {
            this.entrepotDB = entrepotDB;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CreationEntrepotAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            EssaisXEntrepotVM vm = new EssaisXEntrepotVM();
            vm.InfosResasSansEntrepot = entrepotDB.ObtenirResasSansEntrepotUsr(user);

            return View("ListeEssaisXEntrepot", vm);
        }

        /// <summary>
        /// Action pour afficher les infos sur le projet d'un essai sur la liste
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public IActionResult VoirInfosProj(int id)
        {
            InfosProjet vm = entrepotDB.ObtenirInfosProjet(id);
            return PartialView("~/Views/Shared/_DisplayInfosProjet.cshtml", vm);
        }

        /// <summary>
        /// Action pour afficher les infos sur un essai de la liste 
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM model = entrepotDB.ObtenirInfosEssai(id);
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", model);
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
                Reservations = entrepotDB.InfosReservations(id)
            };
            return PartialView("~/Views/Shared/_DisplayEquipsReserves.cshtml", vm);
        }

        public IActionResult CreationEntrepotEssai(int? id)
        {
            // initialisation de session pour l'essai X
            CreationEntrepotVM vm = new CreationEntrepotVM()
            {
                ListReservationsXEssai = entrepotDB.ListeReservationsXEssai(id.Value),
                idEssai = id.Value,
                ListeTypeDoc = entrepotDB.ListeTypeDocuments()
            };

            this.HttpContext.AddToSession("CreationEntrepotVM", vm);

            return View("CreationEntrepotXEssai",vm );
        }

        public IActionResult OuvrirUploadDoc(int? id)
        {
            // Récupérer la session "EquipementZone"
            CreationEntrepotVM vm = HttpContext.GetFromSession<CreationEntrepotVM>("CreationEntrepotVM");
            //vm.OpenUploadPartUn = "";
            // Création d'une liste dropdownlist pour le type produit entrée
            vm.TypeDocumentItem = entrepotDB.ListeTypeDocuments().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_document+": " + f.identificateur });
            vm.NomActivite = entrepotDB.ObtenirNomActivite(id.Value);

            ViewBag.ModalDocOne = "show";
            return View("CreationEntrepotXEssai", vm);
        }

        [HttpPost]
        public IActionResult AjouterDocument(IFormFile file, string description, CreationEntrepotVM vm)
        {
            return View();
        }
    }
}
