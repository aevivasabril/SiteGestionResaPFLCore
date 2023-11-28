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
using SiteGestionResaCore.Services;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class ResasUserController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IResasUserDB resasUserDB;
        private readonly IReservationDb reservationDb;
        private readonly IEmailSender emailSender;

        public ResasUserController(
            UserManager<utilisateur> userManager, 
            IResasUserDB resasUserDB,
            IReservationDb reservationDb,
            IEmailSender emailSender
            )
        {
            this.userManager = userManager;
            this.resasUserDB = resasUserDB;
            this.reservationDb = reservationDb;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> MesReservationsAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = await resasUserDB.ObtenirResasUserAsync(user.Id, null, null, 0)
            };
            //vm.ChildVmModifEssai.infosEssai = new InfosEssai();
            return View(vm);
        }

        public async Task<IActionResult> OuvrirEssaiXModifAsync(int id)
        {
            essai ess = new essai(); // Variable pour récupération d'essai
            ConsultInfosEssaiChildVM infos;
            bool isModif = false;
            //ResEssaiChildViewModel childVM;       

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

            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            // Obtenir les roles et vérifier s'il est "Logistic"
            var allUserRoles = await userManager.GetRolesAsync(user);
            bool IsLogistic = false;
            // Vérifier si la personne est dans le groupe des "Logistic"
            if (allUserRoles.Contains("Logistic"))
            {
                IsLogistic = true;
            }

            if (IsLogistic)
                isModif = true;
            else // vérifier si cet essai est modifiable           
                isModif = resasUserDB.IsEssaiModifiableOuSupp(id);

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
                ResasUser = await resasUserDB.ObtenirResasUserAsync(user.Id, "", null, id),
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
            reVM.ResasUser = await resasUserDB.ObtenirResasUserAsync(user.Id, null, null, 0);

            return View("MesReservations", reVM);
        }

        public async Task<IActionResult> RecapEquipementsAsync(int id)
        {
            bool isModif = false;
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());
            // Obtenir les roles et vérifier s'il est "Logistic"
            var allUserRoles = await userManager.GetRolesAsync(user);
            bool IsLogistic = false;
            // Vérifier si la personne est dans le groupe des "Logistic"
            if (allUserRoles.Contains("Logistic"))
            {
                IsLogistic = true;
            }

            if (IsLogistic)
                isModif = true;
            else // vérifier si cet essai est modifiable           
                isModif = resasUserDB.IsEssaiModifiableOuSupp(id);

            // on envoie null et zéro pour les paramètres car pas d'ouverture des infos essai
            MyReservationsViewModel vm = new MyReservationsViewModel()
            {
                ResasUser = await resasUserDB.ObtenirResasUserAsync(user.Id, null, "", id),
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
                ResasUser = await resasUserDB .ObtenirResasUserAsync(user.Id, null, null, 0), 
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
            vm.ResasUser = await resasUserDB.ObtenirResasUserAsync(user.Id, null, null, 0);
            vm.IdEss = id;
            vm.TitreEssai = resasUserDB.ObtenirEssaiPourModif(id).titreEssai;

            return View("MesReservations", vm);
        }

        public IActionResult ModifierResa(int? id)
        {
            ModifDatesVM vm = new ModifDatesVM();
            var resa = resasUserDB.ObtenirResa(id.Value);
            var equip = resasUserDB.ObtenirEquipement(resa.equipementID);
            vm.NomEquipement = equip.nom;
            vm.IdEquipement = equip.id;
            vm.IdEssai = resa.essaiID;
            vm.IdReservation = resa.id;

            vm.ListResas = resasUserDB.DonneesCalendrierEquipement(true, equip.id , resa.date_debut, resa.date_fin);
            vm.DateDebutCreneau = resa.date_debut;
            vm.DateFinCreneau = resa.date_fin;

            this.HttpContext.AddToSession("ModifDatesVM", vm);
            return View("ModificationDates", vm);
        }

        public IActionResult AfficherPlanningEquip(ModifDatesVM vm, int id)
        {
            // Récupérer la session "ModifDatesVM"
            ModifDatesVM ModifDatesVM = HttpContext.GetFromSession<ModifDatesVM>("ModifDatesVM");
            // Il faut supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DateDebutCreneau");
            ModelState.Remove("DateFinCreneau");
            ModelState.Remove("DatePickerDebut_Matin");
            ModelState.Remove("DatePickerFin_Matin");

            if (ModelState.IsValid) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (vm.DateDebutCalend <= vm.DateFinCalend)
                {
                    ModifDatesVM.ListResas = resasUserDB.DonneesCalendrierEquipement(false, id, vm.DateDebutCalend, vm.DateFinCalend);
                    ModifDatesVM.DateDebutCalend = vm.DateDebutCalend;
                    ModifDatesVM.DateFinCalend = vm.DateFinCalend;
                    this.HttpContext.AddToSession("ModifDatesVM", ModifDatesVM);
                    return View("ModificationDates", ModifDatesVM);
                }
                else
                {
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                    return PartialView("ModifierEquipResa", ModifDatesVM);
                }
            }
            else
            {
                return View("ModificationDates", ModifDatesVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangerDateResaAsync(ModifDatesVM vm, int id)
        {
            DateTime debutToSave = new DateTime();
            DateTime finToSave = new DateTime();
            string mssLogis = "";
            var user = await userManager.FindByIdAsync(User.GetUserId());           // Utilisateur qui modifie la date
                                                                                    // Retry pour envoi mail
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;

            // Récupérer la session "ModifDatesVM"
            ModifDatesVM ModifDatesVM = HttpContext.GetFromSession<ModifDatesVM>("ModifDatesVM");
            // Il faut supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DateDebutCalend");
            ModelState.Remove("DateFinCalend");

            if (ModelState.IsValid)
            {
                if (ModifDatesVM.DateDebutCreneau.Value.Date == ModifDatesVM.DateFinCreneau.Value.Date)
                {
                    if ((Convert.ToBoolean(vm.DatePickerDebut_Matin) == false) && (Convert.ToBoolean(vm.DatePickerFin_Matin) == true))
                    {
                        ModelState.AddModelError("", "Si la date début et la date fin sont égales, la réservation ne peut pas commencer l'après-midi et finir le matin");
                        return View("ModificationDates", ModifDatesVM);
                    }
                    else
                    {
                        goto ADD;
                    }
                }

            ADD:
                if (vm.DateDebutCreneau.Value.Date <= vm.DateFinCreneau.Value.Date) // si la date debut est inférieure à la date fin alors OK
                {
                    // Etablir l'heure de début et de fin selon les créneaux choisis (Matin ou après-midi)
                    #region Définition des dates réservation avec l'heure selon le créneau choisi (réservations et maintenance)
                    // Definition date debut

                    if (Convert.ToBoolean(vm.DatePickerDebut_Matin) == true) // definir l'heure de début à 7h
                    {
                        debutToSave = new DateTime(vm.DateDebutCreneau.Value.Year,
                            vm.DateDebutCreneau.Value.Month, vm.DateDebutCreneau.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    }
                    else // Début de manip l'après-midi à 13h
                    {
                        debutToSave = new DateTime(vm.DateDebutCreneau.Value.Year,
                            vm.DateDebutCreneau.Value.Month, vm.DateDebutCreneau.Value.Day, 13, 0, 0, DateTimeKind.Local);
                    }
                    // Definition date fin
                    if (Convert.ToBoolean(vm.DatePickerFin_Matin) == true)
                    {
                        finToSave = new DateTime(vm.DateFinCreneau.Value.Year,
                            vm.DateFinCreneau.Value.Month, vm.DateFinCreneau.Value.Day, 12, 0, 0, DateTimeKind.Local);
                    }
                    else // Fin de la manip 18h
                    {
                        finToSave = new DateTime(vm.DateFinCreneau.Value.Year,
                            vm.DateFinCreneau.Value.Month, vm.DateFinCreneau.Value.Day, 18, 0, 0, DateTimeKind.Local);
                    }
                    #endregion                

                    #region Vérifier que la date début du créneaux à rajouter est inférieur ou égal à une semaine avant la réservation

                    // Autoriser les utilisateurs logistic à faire des réservations tardives
                    var userResa = await userManager.FindByIdAsync(User.GetUserId()); // Obtenir le ID d'authentification AspNet
                    var allUserRoles = await userManager.GetRolesAsync(userResa);
                    bool IsLogistic = false;
                    // Vérifier si la personne est dans le groupe des "Logistic"
                    if (allUserRoles.Contains("Logistic"))
                    {
                        IsLogistic = true;
                    }

                    TimeSpan diff = debutToSave - DateTime.Now;
                    // Vérifier que la personne ne depasse pas la limite de changement de date (avant 10h du matin la veille de la manipe) 
                    if (IsLogistic == false)
                    {
                        if (diff.Hours < 21 && diff.Days == 0) // pour permettre à une personne d'ajouter un équipement avant 10h du matin la veille de la manip
                        {
                            ModelState.AddModelError("", "Vous ne pouvez pas modifier les dates réservation équipement à moins d'un jour du début d'utilisation!");
                            return View("ModificationDates", ModifDatesVM);
                        }
                        /*if (diff.Days < 0)
                        {
                            ModelState.AddModelError("", "Vous ne pouvez pas ajouter un équipement à votre réservation à une date antérieur!");
                            return View("ModificationDates", ModifDatesVM);
                        }*/
                    }
                    
                    #endregion

                    #region Vérification de disponibilité pour les dates saisies avant de le stocker dans le model

                    bool isResaOkToAdd = false;
                    // selon le type de confidentialité, vérifier la disponibilité des équipements
                    // obtenir l'essai où les réservations devront être ajoutées
                    essai Essai = resasUserDB.ObtenirEssaiPourModif(ModifDatesVM.IdEssai);
                    switch (Essai.confidentialite)
                    {
                        case "Ouvert":
                            // Car il s'agit d'une modification on utilise une methode differente 
                            isResaOkToAdd = reservationDb.DispoEssaiOuvertPourAjout(debutToSave, finToSave, ModifDatesVM.IdEquipement, Essai.id);
                            if (isResaOkToAdd == false)
                            {
                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                return View("ModificationDates", ModifDatesVM);
                            }                            
                            break;
                        case "Restreint":
                            isResaOkToAdd = reservationDb.DispoEssaiRestreintPourAjout(debutToSave, finToSave, ModifDatesVM.IdEquipement, Essai.id);
                            if (isResaOkToAdd == false)
                            {
                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                return View("ModificationDates", ModifDatesVM);
                            }                         
                            break;
                        case "Confidentiel":
                            isResaOkToAdd = reservationDb.DispoEssaiConfidentielPourAjout(debutToSave, finToSave, ModifDatesVM.IdEquipement, Essai.id);
                            if (isResaOkToAdd == false)
                            {
                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                return View("ModificationDates", ModifDatesVM);
                            }                           
                            break;
                    }
                    #endregion

                    // Changer les dates de la réservation
                    if (isResaOkToAdd)
                    {
                        bool IsOk = resasUserDB.ChangerDatesResa(debutToSave, finToSave, ModifDatesVM.IdReservation);
                        if(!IsOk)
                        {
                            ModelState.AddModelError("", "Problème lors du changement de date debut et date fin de la réservation. Essayez ulterieurement");
                            return View("ModificationDates", ModifDatesVM);
                        }
                        // Obtenir reservation pour envoie de mail et pour remettre le essai en attente de validation
                        reservation_projet reser = resasUserDB.ObtenirResa(ModifDatesVM.IdReservation);
                        // Obtenir projet
                        projet pr = resasUserDB.ObtenirProjet(Essai.projetID);
                        equipement eq = resasUserDB.ObtenirEquipement(reser.equipementID);

                        // envoyer mail notification uniquement aux admins
                        // Remplir le message à envoyer aux admins pour notifier la réservation
                        mssLogis = @"<html>
                            <body> 
                            <p> Bonjour, <br> " + "L'utilisateur: " + user.Email + " " + " vient de modifier des créneaux sur sa réservation pour le projet : <b> " + pr.num_projet + "</b> (Essai N°: " + Essai.id + " ) " +
                                    " " + ". Equipement concerné par cette modification: <br> "
                                    + "</p>";

                        mssLogis += @"<table>
                                <tr>
                                    <th> Equipement </th>
                                    <th> Date début </th>
	                                <th> Date fin </th>
                                </tr>";                     

                        mssLogis += @" <tr>"+ "<td>" + eq.nom
                                       + "   </td>" + "<td>   " + reser.date_debut.ToString()
                                       + "   </td>" + "<td>   " + reser.date_fin.ToString()
                                       + "   </td> </tr>";

                        // Envoyer le mail récapitulatif utilisateur
                        mssLogis += @"</table>                               
                                <p>
                                <br>L'équipe PFL,
                                </p>
                                </body>
                                </html>";

                        // Changer le status de l'essai à Waiting4Valid
                        resasUserDB.UpdateStatusEssai(Essai);
                       
                        // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                        #region Envoi de mail notifications aux "Admin"/"Logistic"

                        // récupérer les "Administrateurs" dont ils ont un rôle supplementaire égal à "Logistic"
                        IList<utilisateur> UsersLogistic = await resasUserDB.ObtenirUsersLogisticAsync();
                       
                        for (int index = 0; index < UsersLogistic.Count(); index++)
                        {
                            NumberOfRetries = 5;
                            retryCount = NumberOfRetries;
                            success = false;

                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Modification réservation à valider", mssLogis);
                                    success = true;
                                }
                                catch (Exception e)
                                {
                                    retryCount--;

                                    if (retryCount == 0)
                                    {
                                        ModelState.AddModelError("", "Problème de connexion pour l'envoie de mail! : " + e.Message + ".");
                                        return View("ModificationDates", ModifDatesVM);  //or handle error and break/return
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    ModelState.AddModelError("", "La date fin de réservation ne peut pas être inférieure à la date début");
                    return View("ModificationDates", ModifDatesVM);
                }

                return RedirectToAction("ModifierEquipResa", "ResasUser", new { area = "User", id = ModifDatesVM.IdEssai });
            }
            else
            {
                return View("ModificationDates", ModifDatesVM);
            }
          
        }
    }
}