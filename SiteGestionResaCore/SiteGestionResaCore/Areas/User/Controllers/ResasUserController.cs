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
        private readonly IFormulaireResaDb formDb;
        private readonly IProjetEssaiResaDb projetEssaiDb;
        private readonly IResaAValiderDb resaAValiderDb;

        public ResasUserController(
            UserManager<utilisateur> userManager, 
            IResasUserDB resasUserDB,
            IFormulaireResaDb formDb,
            IProjetEssaiResaDb projetEssaiDb,
            IResaAValiderDb resaAValiderDb)
        {
            this.userManager = userManager;
            this.resasUserDB = resasUserDB;
            this.formDb = formDb;
            this.projetEssaiDb = projetEssaiDb;
            this.resaAValiderDb = resaAValiderDb;
        }

        public async Task<IActionResult> MesReservationsAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, 0),
                ChildVmModifEssai = new ResEssaiChildViewModel()
            };
            //vm.ChildVmModifEssai.infosEssai = new InfosEssai();
            return View(vm);
        }

        public async Task<IActionResult> OuvrirEssaiXModifAsync(int id)
        {
            essai ess = new essai(); // Variable pour récupération d'essai
            InfosEssai infos;
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());           

            #region Récupérer les infos projet pour affichage dans le formulaire de modif

            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usersManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste dropdownlist pour le type produit entrée
            var prodEntree = formDb.ObtenirList_TypeProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in });
            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProd = formDb.ObtenirList_ProvenanceProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit });
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProd = formDb.ObtenirList_DestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });

            #endregion

            ess = resasUserDB.ObtenirEssaiPourModif(id);

            var isModif = resasUserDB.IsEssaiModifiable(id);
            if (isModif)
            {
                infos = new InfosEssai();
            }
            else
            {                
                infos = resaAValiderDb.ObtenirInfosEssai(id);
            }

            // initialiser le childmodel utilisé dans la vue partielle (affichage des infos essai)
            ResEssaiChildViewModel childVM = new ResEssaiChildViewModel()
            {
                ManipProjItem = usersManip,
                ProductItem = prodEntree,
                ProvenanceProduitItem = provProd,
                DestProduitItem = destProd,
                ConfidentialiteEssai = ess.confidentialite,
                PrecisionProduitIn = ess.precision_produit,
                QuantiteProduit = ess.quantite_produit,
                CommentaireEssai = ess.commentaire,
                TransportSTLO = ess.transport_stlo.ToString(),
                SelectedManipulateurID = ess.manipulateurID,
                SelectedProveProduitId = projetEssaiDb.IdProvProduitPourCopie(ess.id),
                SelectedDestProduit = projetEssaiDb.IdDestProduitPourCopie(ess.id),
                SelectedProductId = projetEssaiDb.IdProduitInPourCopie(ess.id),
                IsEssaiModifiable = isModif,
                InfosEssai = infos
            };

            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                // afficher les infos essai selectionné propiète style=display:none ou "" (none pas d'affichage et "" affichage)
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, "", id),
                ChildVmModifEssai = childVM
            };
            
            return View("MesReservations", vm);
        }
    }
}