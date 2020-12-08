using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
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
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, 0)
            };
            //vm.ChildVmModifEssai.infosEssai = new InfosEssai();
            return View(vm);
        }

        public async Task<IActionResult> OuvrirEssaiXModifAsync(int id)
        {
            essai ess = new essai(); // Variable pour récupération d'essai
            ConsultInfosEssaiChilVM infos;
            //ResEssaiChildViewModel childVM;

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());           

            #region Récupérer les infos projet pour affichage dans le formulaire de modif

            List<utilisateur> listUsersAcces = userManager.Users.Where(e => e.EmailConfirmed == true).ToList();
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usrsManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste dropdownlist pour le type produit entrée
            var prodIn = resasUserDB.ListProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in });
            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProduit = resasUserDB.ListProveProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit });
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProduit = resasUserDB.ListDestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });

            #endregion

            ess = resasUserDB.ObtenirEssaiPourModif(id);

            var isModif = resasUserDB.IsEssaiModifiable(id);
            if (isModif)
            {
                infos = new ConsultInfosEssaiChilVM();
            }
            else
            {                
                infos = resasUserDB.ObtenirInfosEssai(id);
            }

           
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                // afficher les infos essai selectionné propiète style=display:none ou "" (none pas d'affichage et "" affichage)
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, "", id),
                IdEss = id,
                ManiProjItem = usrsManip,
                ProdItem = prodIn,
                ProvProduitItem = provProduit,
                DestProdItem = destProduit,
                ConfidentialiteEss = ess.confidentialite,
                PrecisionProdIn = ess.precision_produit,
                QuantProduit = ess.quantite_produit,
                CommentEssai = ess.commentaire,
                TranspSTLO = ess.transport_stlo.ToString(),
                SelecManipulateurID = ess.manipulateurID,
                SelectProvProduitId = resasUserDB.IdProvProduitToCopie(ess.id),
                SelectDestProduit = resasUserDB.IdDestProduitToCopie(ess.id),
                SelectProductId = resasUserDB.IdProduitInToCopie(ess.id),
                IsEssaiModifiable = isModif,
                ConsultInfosEssai = infos
            };
            
            return View("MesReservations", vm);
        }

        [HttpPost]
        public IActionResult EnregistrerInfosEssai(MyReservationsViewModel reVM, int id)
        {
            return View("MesReservations");
        }
    }
}