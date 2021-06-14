using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Areas.User.Data;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;

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
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, null, 0)
            };
            //vm.ChildVmModifEssai.infosEssai = new InfosEssai();
            return View(vm);
        }

        public async Task<IActionResult> OuvrirEssaiXModifAsync(int id)
        {
            essai ess = new essai(); // Variable pour récupération d'essai
            ConsultInfosEssaiChildVM infos;
            //ResEssaiChildViewModel childVM;

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());           

            #region Récupérer les infos projet pour affichage dans le formulaire de modif

            List<utilisateur> listUsersAcces = userManager.Users.Where(e => e.EmailConfirmed == true).OrderBy(u=>u.nom).ToList();
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
                infos = new ConsultInfosEssaiChildVM();
            }
            else
            {                
                infos = resasUserDB.ObtenirInfosEssai(id);
                ViewBag.modalState = "show";
            }
            
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                // afficher les infos essai selectionné propiète style=display:none ou "" (none pas d'affichage et "" affichage)
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, "", null, id),
                IdEss = id,
                ManiProjItem = usrsManip,
                ProdItem = prodIn,
                ProvProduitItem = provProduit,
                DestProdItem = destProduit,
                ConfidentialiteEss = ess.confidentialite,
                PrecisionProdIn = ess.precision_produit,
                QuantProduit = ess.quantite_produit,
                TitreEssai = ess.titreEssai,
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
        public async Task<IActionResult> EnregistrerInfosEssaiAsync(MyReservationsViewModel reVM, int id)
        {
            bool isChangeOk = false;
            // Voir selon les infos saisies s'il y a une différence avec les infos actuelles
            var essa = resasUserDB.ObtenirEssaiPourModif(id);

            #region vérifier si le manipulateur essai a changé

            if (reVM.SelecManipulateurID != -1 && reVM.SelecManipulateurID != 0)
            {         
                if (essa.manipulateurID != reVM.SelecManipulateurID)
                {
                    isChangeOk = resasUserDB.UpdateManipID(essa, reVM.SelecManipulateurID);
                    if (!isChangeOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj 'manipulateur ID', reesayez ultérieurement.");
                        goto ENDT;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vous devez avoir un manipulateur essai, Modifications essai N° " + id +  " non pris en compte. Repetez l'opération.");
                goto ENDT;
            }
            
            #endregion

            #region Vérifier le produit entrant
            if (reVM.SelectProductId != -1 && reVM.SelectProductId != 0)
            {
                if(essa.type_produit_entrant == null) //  valeur null n'existe pas dans la bdd donc on met à jour la valeur de l'essai
                {
                    isChangeOk = resasUserDB.UpdateProdEntree(essa, reVM.SelectProductId);
                    if (!isChangeOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj 'type produit entrant', reesayez ultérieurement.");
                        goto ENDT;
                    }
                }
                else
                {
                    // vérifier si le produit entrée a changé
                    if (!resasUserDB.compareTypeProdEntree(essa.type_produit_entrant, reVM.SelectProductId)) 
                    {

                        isChangeOk = resasUserDB.UpdateProdEntree(essa, reVM.SelectProductId);
                        if (!isChangeOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj 'type produit entrant', reesayez ultérieurement.");
                            goto ENDT;
                        }

                    }
                }
            }
            #endregion

            #region Vérifier la précision produit
            // TODO: vérifier cette méthode / vérifier si la précision du produit peut-être incluse
            if( (essa.precision_produit == null && reVM.PrecisionProdIn != null) || (essa.precision_produit != null && reVM.PrecisionProdIn == null) ||
                (essa.precision_produit != reVM.PrecisionProdIn) ) // valeur dans essai à null et l'utilisateur souhaite mettre quelque chose 
            {
                isChangeOk = resasUserDB.UpdatePrecisionProd(essa, reVM.PrecisionProdIn);
                if (!isChangeOk)
                {
                    ModelState.AddModelError("", "Problème lors de la maj 'precision produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            } 
            
            #endregion

            #region Vérifir la quantité de produit
            // vérifier si la quantité de produit a changée
            if((essa.quantite_produit == 0 && reVM.QuantProduit != 0) || (essa.quantite_produit != 0 && reVM.QuantProduit == 0) ||
                (essa.quantite_produit != reVM.QuantProduit) )
            {
                isChangeOk = resasUserDB.UpdateQuantiteProd(essa, reVM.QuantProduit);
                if (!isChangeOk)
                {
                    ModelState.AddModelError("", "Problème lors de la maj 'quantité produit', reesayez ultérieurement.");
                    goto ENDT;
                }
            }
            
            #endregion

            #region Vérifier la provenance produit
            // vérifier la provenance produit
            if (reVM.SelectProvProduitId != -1 && reVM.SelectProvProduitId != 0)
            {
                if(essa.provenance_produit == null) // la valeur null n'existe pas dans la liste de provenance produit
                {
                    isChangeOk = resasUserDB.UpdateProvProd(essa, reVM.SelectProvProduitId);
                    if (!isChangeOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj 'provenance produit', reesayez ultérieurement.");
                        goto ENDT;
                    }
                }
                else // si la valeur n'est pas null alors comparer avec la nouvelle valeur choisie
                {
                    if (!resasUserDB.compareProvProd(essa.provenance_produit, reVM.SelectProvProduitId)) //option par défaut
                    {
                        isChangeOk = resasUserDB.UpdateProvProd(essa, reVM.SelectProvProduitId);
                        if (!isChangeOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj 'provenance produit', reesayez ultérieurement.");
                            goto ENDT;
                        }
                    }
                }
                
            }
            #endregion

            #region Verifier la destination produit

            if (reVM.SelectDestProduit != -1 && reVM.SelectDestProduit != 0)
            {
                if(essa.destination_produit == null)
                {
                    isChangeOk = resasUserDB.UpdateDestProd(essa, reVM.SelectDestProduit);
                    if (!isChangeOk)
                    {
                        ModelState.AddModelError("", "Problème lors de la maj 'destination produit', reesayez ultérieurement.");
                        goto ENDT;
                    }
                }
                else
                {
                    // vérifier la Destination produit
                    if (!resasUserDB.compareDestProd(essa.destination_produit, reVM.SelectDestProduit)) //option par défaut
                    {
                        isChangeOk = resasUserDB.UpdateDestProd(essa, reVM.SelectDestProduit);
                        if (!isChangeOk)
                        {
                            ModelState.AddModelError("", "Problème lors de la maj 'destination produit', reesayez ultérieurement.");
                            goto ENDT;
                        }
                    }
                }
            }
            #endregion

            #region Vérifier le transport désiré

            // Vérifier si le mode de transport a changé
            if (essa.transport_stlo != Convert.ToBoolean(reVM.TranspSTLO))
            {
                isChangeOk = resasUserDB.UpdateTransport(essa, reVM.TranspSTLO);
                if (!isChangeOk)
                {
                    ModelState.AddModelError("", "Problème lors de la maj 'transport', reesayez ultérieurement.");
                    goto ENDT;
                }
            }
            #endregion

            #region Vérifier le commentaire essai
            // Vérifier si le Commentaire essai a changé
            if ((essa.titreEssai == null && reVM.TitreEssai != null) || (essa.titreEssai != null && reVM.TitreEssai == null) ||
                (essa.titreEssai != reVM.TitreEssai))
            {
                isChangeOk = resasUserDB.UpdateTitre(essa, reVM.TitreEssai);
                if (!isChangeOk)
                {
                    ModelState.AddModelError("", "Problème lors de la maj 'Commentaire essai', reesayez ultérieurement.");
                    goto ENDT;
                }
            }
                
            #endregion

            ViewBag.AfficherMessage = true;
            ViewBag.Message = "L'essai N° " + id + " a été mis à jour! ";           

        ENDT:
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            reVM.ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, null, 0);

            return View("MesReservations", reVM);
        }

        public async Task<IActionResult> RecapEquipementsAsync(int id)
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // vérifier si cet essai est modifiable 
            var isModif = resasUserDB.IsEssaiModifiable(id);

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, "", id),
                EquipementsReserves = resasUserDB.ResasEssai(id),
                ConsultInfosEssai = new ConsultInfosEssaiChildVM(), 
                IsEssaiModifiable = isModif
            };
            return View("MesReservations", vm);
        }

        public IActionResult ModifierEquipResa(int id)
        {
            ModifierEquipVM vm = new ModifierEquipVM()
            {
                ListResas = resasUserDB.ResasEssai(id),
                IdEssai = id
            };

            return View(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id réservation</param>
        /// <returns></returns>
        public IActionResult PreSupprimerResaAsync(int id)
        {
            // obtenir certaines informations sur la réservations 
            reservation_projet resa = resasUserDB.ObtenirResa(id);
            //Affichage des réservations 
            ModifierEquipVM vm = new ModifierEquipVM()
            {
                ListResas = resasUserDB.ResasEssai(resa.essaiID),
                IdEssai = resa.essaiID,
                IdResa = id
            };
            ViewBag.modalConfirm = "show";
            return View("ModifierEquipResa", vm);
        }

        [HttpPost]
        public IActionResult SupprimerResa(ModifierEquipVM vm, int id)
        {
            bool isChangeOk = false;
            isChangeOk = resasUserDB.SupprimerResa(id);
            if (!isChangeOk)
            {
                ModelState.AddModelError("", "Sorry! Problème lors de la suppression réservation. Essayez ulterieurement");
                goto ENDT;
            }
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Réservation supprimée! ";

        ENDT:
            vm.ListResas = resasUserDB.ResasEssai(vm.IdEssai);

            return View("ModifierEquipResa", vm);
        }

        public async Task<IActionResult> AnnulerEssaiAsync(int id) // id essai
        {

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, null, 0), 
                IdEss = id,
                TitreEssai = resasUserDB.ObtenirEssaiPourModif(id).titreEssai
            };
            ViewBag.modalAnnul = "show";
            return View("MesReservations", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AnnulerEssaiAsync(MyReservationsViewModel vm, int id) // id essai
        {
            // REMOVE les champs non concernés pour cette partie ModelState
            ModelState.Remove("TitreEssai");
            ModelState.Remove("TranspSTLO");
            ModelState.Remove("SelecManipulateurID");

            if (ModelState.IsValid)
            {
                bool IsAnnulationOk = resasUserDB.AnnulerEssai(id, vm.RaisonAnnulation);
                if (IsAnnulationOk)
                {
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Essai supprimé avec succès! ";
                }
                else
                {
                    ModelState.AddModelError("", "Problème pour annuler la réservation. Veuillez essayer ultérieurement");
                }
            }
            else
            {              

                ViewBag.modalAnnul = "show";
            }
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            vm.ResasUser = resasUserDB.ObtenirResasUser(user.Id, null, null, 0);
            vm.IdEss = id;
            vm.TitreEssai = resasUserDB.ObtenirEssaiPourModif(id).titreEssai;

            return View("MesReservations", vm);
        }
    }
}