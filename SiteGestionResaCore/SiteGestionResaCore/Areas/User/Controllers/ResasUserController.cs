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
            bool isChangeOk = false;
            // Voir selon les infos saisies s'il y a une différence avec les infos actuelles
            // vérifier si la confidentialité a changé
            var essa = resasUserDB.ObtenirEssaiPourModif(id);
            if (essa.confidentialite != reVM.ConfidentialiteEss)
            {
                isChangeOk = resasUserDB.UpdateConfidentialiteEss(essa, reVM.ConfidentialiteEss);
                if(!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'confidentialité', reesayez ultérieurement.");
                    goto ENDT;
                }
            }
            // vérifier si le manipulateur essai a changé
            if(essa.manipulateurID!=reVM.SelecManipulateurID && reVM.SelecManipulateurID != -1 && reVM.SelecManipulateurID != 0)
            {
                isChangeOk = resasUserDB.UpdateManipID(essa, reVM.SelecManipulateurID);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'manipulateur ID', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // vérifier si le produit entrée a changé
            if (!resasUserDB.compareTypeProdEntree(essa.type_produit_entrant, reVM.SelectProductId) 
                && reVM.SelectProductId != -1 && reVM.SelecManipulateurID != 0) //option par défaut
            {
                isChangeOk = resasUserDB.UpdateProdEntree(essa, reVM.SelectProductId);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'type produit entrant', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // TODO: vérifier cette méthode / vérifier si la précision du produit peut-être incluse
            if(essa.precision_produit != reVM.PrecisionProdIn && reVM.PrecisionProdIn != null) // car si on change pas il sera null
            {
                isChangeOk = resasUserDB.UpdatePrecisionProd(essa, reVM.PrecisionProdIn);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'precision produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // vérifier si la quantité de produit a changée
            if (essa.quantite_produit != reVM.QuantProduit && reVM.QuantProduit != null)
            {
                isChangeOk = resasUserDB.UpdateQuantiteProd(essa, reVM.QuantProduit);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'quantité produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // vérifier la provenance produit
            if (!resasUserDB.compareProvProd(essa.provenance_produit, reVM.SelectProvProduitId) && reVM.SelectProvProduitId != -1 && reVM.SelectProvProduitId != 0) //option par défaut
            {
                isChangeOk = resasUserDB.UpdateProvProd(essa, reVM.SelectProvProduitId);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'provenance produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // vérifier la Destination produit
            if (!resasUserDB.compareDestProd(essa.destination_produit, reVM.SelectDestProduit) && reVM.SelectDestProduit != -1 && reVM.SelectDestProduit != 0) //option par défaut
            {
                isChangeOk = resasUserDB.UpdateDestProd(essa, reVM.SelectDestProduit);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'destination produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // Vérifier si le mode de transport a changé
            if(essa.transport_stlo.ToString() != reVM.TranspSTLO)
            {
                isChangeOk = resasUserDB.UpdateTransport(essa, reVM.TranspSTLO);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'transport', reesayez ultérieurement.");
                    goto ENDT;
                }
            }

            // Vérifier si le Commentaire essai a changé
            if (essa.commentaire != reVM.CommentEssai && reVM.CommentEssai != null)
            {
                isChangeOk = resasUserDB.UpdateComment(essa, reVM.CommentEssai);
                if (!isChangeOk)
                {
                    // TODO: tester!
                    ModelState.AddModelError("", "Problème lors de la maj 'Commentaire essai', reesayez ultérieurement.");
                    goto ENDT;
                }
            }
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "L'essai N° " + id + " a été mis à jour! ";
        ENDT:
            return View("MesReservations");
        }
    }
}