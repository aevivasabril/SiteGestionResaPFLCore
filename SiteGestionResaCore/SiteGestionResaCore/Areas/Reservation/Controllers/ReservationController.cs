using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Controllers
{
    [Authorize]
    [Area("Reservation")]
    public class ReservationController : Controller
    {
        private readonly IFormulaireResaDb formDb;
        private readonly IProjetEssaiResaDb projetEssaiDb;
        private readonly IZoneEquipDb zoneEquipDb;
        private readonly IReservationDb reservationDb;
        private readonly IEquipeResaDb equipeResaDb;
        private readonly UserManager<utilisateur> userManager;
        private readonly IEmailSender emailSender;

        public ReservationController(
            IFormulaireResaDb formDb,
            IProjetEssaiResaDb projetEssaiDb,
            IZoneEquipDb zoneEquipDb,
            IReservationDb reservationDb,
            IEquipeResaDb equipeResaDb,
            UserManager<utilisateur> userManager,
            IEmailSender emailSender)
        {
            this.formDb = formDb;
            this.projetEssaiDb = projetEssaiDb;
            this.zoneEquipDb = zoneEquipDb;
            this.reservationDb = reservationDb;
            this.equipeResaDb = equipeResaDb;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        // GET: Reservation/Reservation
        public ActionResult FormulaireProjet()
        {
            List<ld_type_projet> ListeTypeProj = formDb.ObtenirList_TypeProjet();
            List<ld_financement> ListFinancProj = formDb.ObtenirList_Financement();
            List<organisme> ListeOrga = formDb.ObtenirListOrg();
            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();
            List<ld_provenance> ListeProvProj = formDb.ObtenirList_ProvenanceProjet();
            List<ld_produit_in> ListeProdEntree = formDb.ObtenirList_TypeProduitEntree();
            List<ld_provenance_produit> ListProvProd = formDb.ObtenirList_ProvenanceProduit();
            List<ld_destination> ListDestProd = formDb.ObtenirList_DestinationPro();

            FormulaireProjetViewModel vm = new FormulaireProjetViewModel()
            {
                ListeTypeProjet = ListeTypeProj,
                TypeProjetItem = formDb.ListTypeProjetItem(ListeTypeProj),
                ListeFinancement = ListFinancProj,
                TypefinancementItem = formDb.ListFinancementItem(ListFinancProj),
                ListeOrganismes = ListeOrga,
                OrganItem = formDb.ListOrgItem(ListeOrga),
                UsersWithAccess = listUsersAcces,
                RespProjItem = formDb.ListRespItem(listUsersAcces),
                ListeProvenance = ListeProvProj, 
                ProvenanceItem = formDb.ListProveItem(ListeProvProj),
                ManipProjItem = formDb.ListManipItem(listUsersAcces),
                ListeProduitsIn = ListeProdEntree,
                ProductItem = formDb.ListProdEntreeItem(ListeProdEntree),
                ListeProvenanceProduit = ListProvProd,
                ProvenanceProduitItem = formDb.ListProvProdItem(ListProvProd),
                ListeDestProduit = ListDestProd,
                DestProduitItem = formDb.ListDestProdItem(ListDestProd)

            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SoumettreFormulaireAsync(FormulaireProjetViewModel model)
        {
            // NOTE: Dans le cas où on crée un nouveau projet et un nouveau essai
            // TODO: traiter le cas où on copie les données essai et projet (pas de creation de projet! mais création d'essai) voir comment vérifier cela? 
            var user = await userManager.FindByIdAsync(User.GetUserId());
           
            // Je laisse cette ligne car elle permette de vérifier quel champ du formulaire me génère une erreur
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            ModelState.Remove("SelectedEssaiId"); // J'extrait le model error généré par SelectedEssaiId car il est pas pris en compte dans la copie projet, voir commme l'ignorer plus proprement

            if (ModelState.IsValid)
            {
                //si le projet existe alors on sait qu'il s'agit d'une copie et qu'il faut créer un nouveau essai, même si l'utilisateur ne change rien sur le formulaire
                if(projetEssaiDb.ProjetExists(model.NumProjet))
                {
                    // Si l'utilisateur est propiètaire du projet ou "Admin" ou "MainAdmin" alors autoriser la création d'un essai 
                    if(await projetEssaiDb.VerifPropieteProjetAsync(model.NumProjet, user))
                    {
                        // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
                        this.HttpContext.AddToSession("FormulaireResa", model);              
                        return RedirectToAction("PlanZonesReservation");
                    }
                    else
                    {
                        //ViewBag.Message = "Ce projet existe mais vous n'avez pas le droit de rajouter des essais";
                        ModelState.AddModelError("", "Ce projet existe mais vous n'avez pas le droit de rajouter des essais");
                        return View("FormulaireProjet", model);
                    }
                }
                else // si le projet n'existe pas alors l'ajouter dans la base de données + l'essai
                {
                    // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
                    this.HttpContext.AddToSession("FormulaireResa", model);
                    return RedirectToAction("PlanZonesReservation"); // changer pour envoyer direct à la vue du plan PFL
                }
                
            }
            else
            {
                return View("FormulaireProjet", model); // Si error alors on recharge la page pour montrer les messages
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TrouverProjetAsync(FormulaireProjetViewModel model)
        {
            // Obtenir le ID d'authentification AspNet
            var user = await userManager.FindByIdAsync(User.GetUserId());
            bool propProjetOk = false;
            // récupérer les projets pour le numéro saisie

            model.ProjetValide = projetEssaiDb.ProjetExists(model.NumProjet);
            
            if (!model.ProjetValide)
            {
                ViewBag.Message = " Ce numéro de projet n'existe pas";
            }
            else
            {
                propProjetOk = await projetEssaiDb.VerifPropieteProjetAsync(model.NumProjet, user);
                if (propProjetOk)
                {
                    List<EssaiUtilisateur> listEssaiUsr = projetEssaiDb.ObtenirList_EssaisUser(model.NumProjet);
                    ViewBag.Message = "";
                    model.EssaiUtilisateur = listEssaiUsr;
                    model.EssaiItem = projetEssaiDb.ListEssaisToSelectItem(listEssaiUsr);
                }
                else
                {
                    ViewBag.Message = "Vous n'avez pas le droit de copier les informations sur ce projet";
                }
            }         

            ViewBag.modalListe = "show";
            return View("FormulaireProjet", model);
        }

        [HttpPost]
        public ActionResult CopierProjetEssai(FormulaireProjetViewModel model, string numProjet)
        {
            projet pr = new projet();
            essai ess = new essai();

            // nouveau model au propre pour éviter les problèmes d'initialisation? TODO voir si cela marche
            //FormulaireProjetViewModel vm = new FormulaireProjetViewModel { }; // cela ne sert à rien finalement

            // Obtenir les infos sur mon Projet et mon essai pour les injecter dans mon model (affichage sur la vue)

            // Récupérer à partir de la BDD les infos sur le projet et l'essai
            // Vérifier cette ligne suite à migration ASP NET CORE
            pr = projetEssaiDb.ObtenirProjet_pourCopie(model.NumProjet); // Provenant du HiddenFor ligne 207
            ess = projetEssaiDb.ObtenirEssai_pourCopie(model.SelectedEssaiId);
            model.SelectTypeProjetId = projetEssaiDb.IdTypeProjetPourCopie(pr.id);
            model.SelectFinancementId = projetEssaiDb.IdFinancementPourCopie(pr.id);
            model.SelectedRespProjId = projetEssaiDb.IdRespoProjetPourCopie(pr.id);
            model.SelectedProvenanceId = projetEssaiDb.IdProvenancePourCopie(pr.id);
            model.SelectedProveProduitId = projetEssaiDb.IdProvProduitPourCopie(ess.id);
            model.SelectedDestProduit = projetEssaiDb.IdDestProduitPourCopie(ess.id);
            model.SelectedProductId = projetEssaiDb.IdProduitInPourCopie(ess.id);

            // Données à copier pour le projet
            model.TitreProjet = pr.titre_projet;
            model.NumProjet = pr.num_projet;
            model.SelectedOrganId = pr.organismeID.GetValueOrDefault();
            model.DescriptionProjet = pr.description_projet;

            // Données à copier pour l'essai
            model.ConfidentialiteEssai = ess.confidentialite;
            model.PrecisionProduitIn = ess.precision_produit;
            model.QuantiteProduit = ess.quantite_produit;
            model.CommentaireEssai = ess.commentaire;
            model.TransportSTLO = ess.transport_stlo.ToString();
            model.SelectedManipulateurID = ess.manipulateurID;

            ModelState.Clear();

            return View("FormulaireProjet", model);
        }

        /// <summary>
        /// Ouverture de la vue pour la selection des équipements par zone
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanZonesReservation()
        {
            ZonesReservationViewModel vm = new ZonesReservationViewModel()
            {
                Zones = zoneEquipDb.ListeZones()
            };
            // Etablir une session avec les données à mettre à jour pour la vue principale
            this.HttpContext.AddToSession("ZoneReservation", vm);
            return View(vm);
        }

        /// <summary>
        /// Réception de l'id zone pour afficher tous les équipements dans une autre vue
        /// </summary>
        /// <param name="id"> id zone</param>
        /// <returns></returns>
        public ActionResult EquipementVsZone (int ?id)
        {
            // Initialisation des variables
            List<equipement> equipements = new List<equipement>();
            List<CalendrierEquipChildViewModel> PlanningEquipements = new List<CalendrierEquipChildViewModel>();
            List<ReservationsJour> reservationsSemEquipement = new List<ReservationsJour>();
            CalendrierEquipChildViewModel calenChild;

            // 1. Obtenir la liste des equipements
            equipements = zoneEquipDb.ListeEquipements(id.Value);

            // pour chaque equipement obtenir la liste des réservations et le sauvegarder dans la liste des reservations
            for (int i = 0; i<equipements.Count(); i++)
            {
                // initialiser pour chaque équipement 
                calenChild = new CalendrierEquipChildViewModel();
                // 2. en prenant l'id de chaque equipement, obtenir la list des reservations pour la semaine en cours
                //reservationsSemEquipement = DonneesCalendrier(equipements[i].Id);
                reservationsSemEquipement = DonneesCalendrierEquipement(true, equipements[i].id, null, null);
                calenChild.ListResas = reservationsSemEquipement;
                calenChild.EquipementCalendrier = zoneEquipDb.GetEquipement(equipements[i].id);
                //calenChild.zoneID = id.Value;
                //calenChild.NomEquip = equipements[i].nom;
                // 3. Rajouter cela dans une liste contenant les données des équipements
                PlanningEquipements.Add(calenChild);
                // mettre à zéro les variables
                calenChild = null;
            }

            // Initialisation du view model pour la vue avec les valeurs obtenus ci-dessus
            EquipementsParZoneViewModel vm = new EquipementsParZoneViewModel()
            {
                Equipements = equipements,
                NomZone = zoneEquipDb.GetNomZone(id.Value),
                IdZone = id.Value,
                CalendrierChildVM = PlanningEquipements

            };

            // Etablir une session avec les données à mettre à jour pour la vue EquipementsVsZones
            this.HttpContext.AddToSession("EquipementZone",vm);

            // Récupérer le model du formulaire avec les infos saisies lors du remplissage du formulaire (Vérifié)
            //var formulaire = (FormulaireProjetViewModel)this.HttpContext.Session["FormulaireResa"];

            return View(vm);
        }


        [HttpPost]
        public ActionResult AfficherPlanning(CalendrierEquipChildViewModel model, int equipementId)
        {
            // Initialisation des variables
            List<CalendrierEquipChildViewModel> PlanningEquipements = new List<CalendrierEquipChildViewModel>();
            List<ReservationsJour> reservationsEquipement = new List<ReservationsJour>();

            // Récupérer la session "EquipementZone"
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession< EquipementsParZoneViewModel>("EquipementZone");

            // Il faut supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DateDebut");
            ModelState.Remove("DateFin");
            ModelState.Remove("DatePickerDebut_Matin");
            ModelState.Remove("DatePickerFin_Matin");

            // TODO: vérifier que cela fonctionne!
            if (ModelState.IsValid) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (model.DatePickerDu <= model.DatePickerAu)
                {
                    // pour chaque model de la vue calendrier (c'est à dire pour chaque équipement)
                    for (int i = 0; i < equipementZone.CalendrierChildVM.Count(); i++)
                    {
                        if (equipementZone.CalendrierChildVM[i].EquipementCalendrier.id == equipementId)
                        {
                            // 2. en prenant l'id de chaque equipement, obtenir la list des reservations pour la semaine en cours
                            reservationsEquipement = DonneesCalendrierEquipement(false, equipementId, model.DatePickerDu, model.DatePickerAu);
                            equipementZone.CalendrierChildVM[i].ListResas = reservationsEquipement;
                            break;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                }              
            }
            return View("EquipementVsZone", equipementZone);
        }

        /// <summary>
        /// méthode permettant d'ajouter un créneau de réservation et de revenir sur la même vue en mettant la liste à jour
        /// </summary>
        /// <param name="model">model view provenant de la vue partielle "_creneau"</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public ActionResult AjouterResa(CalendrierEquipChildViewModel model, int equipementId)
        {
            DateTime debutToSave = new DateTime();
            DateTime finToSave = new DateTime();
            // Récupérer la session "EquipementZone"
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            //EquipementsParZoneViewModel equipementZone = (EquipementsParZoneViewModel)this.HttpContext.Session["EquipementZone"];

            int indiceChild = 0; // Sauvegarder l'indice où se trouve le CalendrierEquipChildModel correspondant à l'id

            // Initialiser le calendrierChildModel de l'équipement sur le model de la vue parent avant toutes les opérations
            for (int i = 0; i < equipementZone.CalendrierChildVM.Count(); i++)
            {     
                if (equipementZone.CalendrierChildVM[i].EquipementCalendrier.id == equipementId)
                {
                    #region Compléter le model "CalendrierEquipChildViewModel" avec le model "EquipementsParZoneViewModel"

                    model.EquipementCalendrier = equipementZone.CalendrierChildVM[i].EquipementCalendrier;
                    //model.equiID = idEquip;
                    //model.NomEquip = resaBdd.GetNomEquipement(idEquip);
                    model.ListResas = equipementZone.CalendrierChildVM[i].ListResas;
                    model.ResaEquipement = equipementZone.CalendrierChildVM[i].ResaEquipement;                    
                    equipementZone.CalendrierChildVM[i] = model;
                    indiceChild = i;
                    break;

                    #endregion
                }
            }
            // DatePickerDu et DatePickerAu lançent un erreur de modelstate, il faut les supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DatePickerDu");
            ModelState.Remove("DatePickerAu");

            if (ModelState.IsValid) // Ajouter le créneau de réservation dans la liste
            {
                if (equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value == equipementZone.CalendrierChildVM[indiceChild].DateFin.Value)
                {
                    if (model.DatePickerDebut_Matin == false && model.DatePickerFin_Matin == true)
                    {
                        ModelState.AddModelError("", "Si la date début et la date fin sont égales, la réservation ne peut pas commencer l'après-midi et finir le matin");
                        goto ENDT;
                    }
                    else
                    {
                        goto ADD;
                    }
                }

                ADD:
                if (equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value <= equipementZone.CalendrierChildVM[indiceChild].DateFin.Value) // si la date debut est inférieure à la date fin alors OK
                {
                    // Etablir l'heure de début et de fin selon les créneaux choisis (Matin ou après-midi)
                    #region Définition des dates réservation avec l'heure selon le créneau choisi
                    // Definition date debut
                    if (model.DatePickerDebut_Matin == true) // definir l'heure de début à 7h
                    {
                        debutToSave = new DateTime(equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Year,
                            equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Month, equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    }
                    else // Début de manip l'après-midi à 13h
                    {
                        debutToSave = new DateTime(equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Year,
                                                equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Month, equipementZone.CalendrierChildVM[indiceChild].DateDebut.Value.Day, 13, 0, 0, DateTimeKind.Local);
                    }
                    // Definition date fin
                    if (model.DatePickerFin_Matin == true)
                    {
                        finToSave = new DateTime(equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Year,
                            equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Month, equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Day, 12, 0, 0, DateTimeKind.Local);
                    }
                    else // Début de manip l'après-midi à 13h
                    {
                        finToSave = new DateTime(equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Year,
                            equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Month, equipementZone.CalendrierChildVM[indiceChild].DateFin.Value.Day, 18, 0, 0, DateTimeKind.Local);
                    }
                    #endregion

                    #region Sauvegarde réservation dans la liste des créneaux saisies (seulement View model)

                    // ajouter dans la liste des créneaux réservation progressivement
                    equipementZone.CalendrierChildVM[indiceChild].ResaEquipement.Add(new ReservationTemp
                    {
                        date_debut = debutToSave,
                        date_fin = finToSave
                    });

                    #endregion
                }
                
                else
                {
                    ModelState.AddModelError("", "La date fin de réservation ne peut pas être inférieure à la date début");
                }
            }
            ENDT:
            return View("EquipementVsZone", equipementZone); // Si error alors on recharge la page pour montrer les messages
        }

        /// <summary>
        /// méthode pour afficher la confirmation de supression d'une réservation pour un équipement défini
        /// </summary>
        /// <param name="i">indice CalendrierChildViewModel</param>
        /// <param name="j">indice réservation équipement</param>
        /// <returns>Vue "EquipementVsZone" mise à jour </returns>
        public ActionResult SupprimerCreneauResa(int ?i, int ?j)
        {
            // Récupérer la session "EquipementZone" où se trouvent toutes les informations des réservations
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            // Sauvegarde des indices qui seront utilisés dans la action POST pour répérer le créneau à supprimer 
            equipementZone.IndiceChildModel = i.Value;
            equipementZone.IndiceResaEquipXChild = j.Value;

            // variable pour "afficher" le modal pop up de confirmation de suppression
            ViewBag.modalSupp = "show";

            return View("EquipementVsZone", equipementZone);
        }

        /// <summary>
        /// Suppression suite à confirmation d'un créneau réservation
        /// </summary>
        /// <param name="IndiceChildModel"></param>
        /// <param name="IndiceResaEquipXChild"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SupprimerCreneauResa(EquipementsParZoneViewModel model)
        {
            // Récupérer la session "EquipementZone" où se trouvent toutes les informations des réservations
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");

            equipementZone.CalendrierChildVM[model.IndiceChildModel].ResaEquipement.RemoveAt(model.IndiceResaEquipXChild);
            return View("EquipementVsZone", equipementZone);
        }

        /// <summary>
        /// Validation des créneaux réservation pour une zone X
        /// </summary>
        /// <param name="model">TODO: à effacer car on ne reçoit pas du model</param>
        /// <returns>Plan des zones de réservation contenant les réservations de la zone clique ulterieurement</returns>
        [HttpPost]
        public ActionResult ValiderResaZone(EquipementsParZoneViewModel model)
        {
            // Récupérer la session "EquipementZone" où se trouvent toutes les informations des réservations
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            // Récupérer la session "ZonesReservation" pour afficher le plan + les réservations des zones
            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");

            //ajouter les view model equipementZone contenant les créneau dans le model des zones
            zonesReservation.EquipementsParZone.Add(equipementZone);

            //rediriger vers la page contenant le plan PFL
            return View("PlanZonesReservation", zonesReservation);
        }

        /// <summary>
        /// Supprimer les réservations saisies pour la zone en cours
        /// </summary>
        /// <returns></returns>
        public ActionResult AnnulerResaZone()
        {
            // Récupérer la session "EquipementZone" où se trouvent toutes les informations des réservations
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");

            // Vider toutes les reservations pour tous les CalendrierChildVM
            for(int i = 0; i < equipementZone.CalendrierChildVM.Count(); i++)
            {
                equipementZone.CalendrierChildVM[i].ResaEquipement.Clear();
            }
            // Récupérer la session "ZonesReservation" pour afficher le plan + les réservations des zones
            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");

            //rediriger vers la page contenant le plan PFL
            return View("PlanZonesReservation", zonesReservation);
        }

        /// <summary>
        /// Supprimer les réservations des équipements pour toutes les zones
        /// </summary>
        /// <returns></returns>
        public ActionResult AnnulerResa()
        {
            // Récupérer la session "ZonesReservation" pour afficher le plan + les réservations des zones
            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");
            zonesReservation.EquipementsParZone.Clear();

            // Récupérer la session "FormulaireProjet" pour revenir au formulaire
            FormulaireProjetViewModel formulaire = HttpContext.GetFromSession< FormulaireProjetViewModel>("FormulaireResa");

            return View("FormulaireProjet", formulaire);

        }

        /// <summary>
        /// Liberer toutes les sessions de stockage de data lors de l'annulation de la réservation
        /// </summary>
        /// <returns></returns>
        public ActionResult AnnulerDemande()
        {
            //Libérer toutes les sessions ouvertes avant de quitter la vue formulaire, libérer uniquement les sessions concernées par la réservation pour éviter des pbs
            HttpContext.Session.Remove("FormulaireResa");
            HttpContext.Session.Remove("ZoneReservation");
            HttpContext.Session.Remove("EquipementZone");

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // TODO: A complèter!!! méthode final pour générer une réservation (vérifier que la personne à réservée au moins un équipement)
        [HttpPost]
        public async Task<ActionResult> ValiderResa(ZonesReservationViewModel vm)
        {
            #region Déclaration des variables

            // boolean indiquant si au moins un équipement a été réservé
            bool AtLeastOneEquipment = false;
            DateTime myDateTime = DateTime.Now;
            //string IdUsr = User.Identity.GetUserId();
            var user = await userManager.FindByIdAsync(User.GetUserId());           // Utilisateur créant le projet 
            projet Proj = new projet();                                             // Variable "projet" pour récupérer un projet existant ou un projet qui vient d'être créé
            essai Essai = new essai();                                              // Variable "essai" pour récupérer un essai qui vient d'être créé
            reservation_projet resa = new reservation_projet();                     // Réservation pour un equipement 
            List<reservation_projet> resas;                                         // Liste de toutes les réservations
            IList<utilisateur> UsersLogistic = new List<utilisateur>();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification
            string subNomEquip;
            bool IsFirstResa = true;
            DateTime dateSeuilInf = new DateTime();
            DateTime dateSeuilSup = new DateTime();

            // Retry pour envoi mail
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;

            // Pour faire un tableau bien organisé : https://docs.microsoft.com/fr-fr/dotnet/api/system.string.format?view=netcore-3.1
            var sb_user = new System.Text.StringBuilder(); // Model de mail récapitulatif pour l'utilisateur
            var sb_admin = new System.Text.StringBuilder(); // Model de mail récapitulatif pour l'administrateur

            #endregion

            #region Récupération session

            // Récupérer la session "ZonesReservation" pour récupérer les infos sur le plan + les réservations des zones
            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");

            // Récupérer les infos remplisses dans le formulaire projet/essai
            FormulaireProjetViewModel formulaire = HttpContext.GetFromSession<FormulaireProjetViewModel>("FormulaireResa");

            #endregion

            // Pour retrouver l'erreur quand j'essai de créer la list en RELEASE
            try
            {
                resas = new List<reservation_projet>();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.ToString());
                return View("PlanZonesReservation", zonesReservation);
            }

            #region Création projet 

            #region Vérifier que au moins un équipement est réservé

            // Vérifier qu'au moins un équipement a été réservé
            for (int i = 0; i < zonesReservation.EquipementsParZone.Count(); i++)
            {
                for (int j = 0; j < zonesReservation.EquipementsParZone[i].CalendrierChildVM.Count(); j++)
                {
                    if (zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement.Count() != 0)
                    {
                        AtLeastOneEquipment = true;
                        goto Creation;
                    }
                }
            }

            #endregion

            #region Créer la réservation ou diriger vers la page d'erreur

        Creation:  
            if (AtLeastOneEquipment) // si au moins un équipement a été sélectionné alors on peut créer la réservation
            {
                //si le projet existe alors on sait qu'il s'agit d'une copie et qu'il faut créer un nouveau essai, même si l'utilisateur ne change rien sur le formulaire
                if (projetEssaiDb.ProjetExists(formulaire.NumProjet))
                {
                    Proj = projetEssaiDb.ObtenirProjet_pourCopie(formulaire.NumProjet);
                }
                else // si le projet n'existe pas alors l'ajouter dans la base de données + l'essai
                {
                    Proj = projetEssaiDb.CreationProjet(formulaire.TitreProjet, formulaire.SelectTypeProjetId, formulaire.SelectFinancementId, formulaire.SelectedOrganId,
                        formulaire.SelectedRespProjId, formulaire.NumProjet, formulaire.SelectedProvenanceId, formulaire.DescriptionProjet, myDateTime, user);
                }

                // Creation d'essai (à mettre à jour si l'essai est "Confidentiel") 
                Essai = projetEssaiDb.CreationEssai(Proj, user, myDateTime, formulaire.ConfidentialiteEssai, formulaire.SelectedManipulateurID, formulaire.SelectedProductId, formulaire.PrecisionProduitIn, formulaire.QuantiteProduit,
                            formulaire.SelectedProveProduitId, formulaire.SelectedDestProduit, formulaire.TransportSTLO, formulaire.CommentaireEssai); // TODO: pas oublier de rajouter le status  (enum dans view model)     

                // Remplir le message à envoyer aux admins pour notifier la réservation
                sb_admin.Append("Bonjour,\n\nUne demande de réservation pour le projet N° : " + Proj.num_projet + " (Essai N°: " + Essai.id + " )" + " saisie par l'utilisateur: " + Proj.mailRespProjet + " " +
                                " vient d'être rajoutée. \n\nRécapitulatif des réservations par équipement: \n\n");
                sb_admin.Append(String.Format("{0,55} {1,135} {2,60}\n\n", "Equipement", "Date début", "Date Fin"));

                // Remplir le message à envoyer à l'utilisateur avec récap des équipements réservés
                // possible solution pour créer une table html et la convertir en string!! https://stackoverflow.com/questions/1524105/can-i-convert-a-dynamically-created-c-sharp-table-to-a-html-string
                sb_user.Append("Bonjour,\n\nLa demande de réservation pour le projet N° : " + formulaire.NumProjet +" (Essai N°: " + Essai.id + " )" +
                    " est pris en compte. \n\nRécapitulatif des réservations par équipement: \n\n");

                sb_user.Append(String.Format("{0,55} {1,135} {2,60}\n\n", "Equipement", "Date début", "Date Fin"));


                // Créations des réservations par équipement
                for (int i = 0; i < zonesReservation.EquipementsParZone.Count(); i++)
                {
                    for (int j = 0; j < zonesReservation.EquipementsParZone[i].CalendrierChildVM.Count(); j++)
                    {
                        //Equipement = resaBdd.GetEquipement(zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].EquipementCalendrier.Id);

                        for (int y = 0; y < zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement.Count(); y++)
                        {
                            resa = reservationDb.CreationReservation(zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].EquipementCalendrier,
                                Essai, zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_debut,
                                zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_fin);

                            // ajouter la réservation dans la liste des réservations à utiliser pour le calcul si "confidentiel"
                            resas.Add(resa);

                            #region Creation d'un string contenant le récap réservation

                            subNomEquip = zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].EquipementCalendrier.nom;
                            if (subNomEquip.Length >= 50)
                            {
                                subNomEquip = subNomEquip.Substring(0, 50);
                            }
                            else
                            {
                                // Complèter avec des espaces la longeur pour avoir un nom avec 50 caracteres
                                int diff = 50 - subNomEquip.Length;
                                for(int p= 0; p < diff; p++)
                                {
                                    subNomEquip += " ";
                                }
                            }

                            subNomEquip += " ( N°GMAO: " + zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].EquipementCalendrier.numGmao + " )";

                            sb_user.Append(String.Format("{0,0} {1,70} {2,35}\n", subNomEquip,
                                zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_debut.ToString(), 
                                zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()));

                            // Rajouter dans le mail notification pour les admins
                            sb_admin.Append(String.Format("{0,0} {1,70} {2,35}\n", subNomEquip,
                                zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_debut.ToString(),
                                zonesReservation.EquipementsParZone[i].CalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()));

                            #endregion
                        }
                    }
                }

                #region date inférieure et supérieure si essai "confidentiel"

                if(Essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                {
                    foreach (var reservation in resas)
                    {
                        if (IsFirstResa == true) // Executer que lors de la premiere réservation de la liste 
                        {
                            IsFirstResa = false;
                            dateSeuilInf = resa.date_debut;
                            dateSeuilSup = resa.date_fin;
                        }
                        else
                        {
                            // Recherche des dates superieur et inferieur sur toutes les réservations
                            if ( resa.date_debut.CompareTo(dateSeuilInf) <= 0 ) // (resa.date_debut <= dateSeuilInf)
                            {
                                dateSeuilInf = resa.date_debut;
                            }
                            if ( resa.date_fin.CompareTo(dateSeuilSup) >= 0 )  // (resa.date_fin >= dateSeuilSup)
                            {
                                dateSeuilSup = resa.date_fin;
                            }
                        }
                    }
                    //Mettre à jour l'essai avec les dates seuil
                    projetEssaiDb.UpdateEssai(Essai, dateSeuilInf, dateSeuilSup);
                }
                
                #endregion

                #region Envoi du mail récapitulatif à l'utilisateur
                // Envoyer le mail récapitulatif utilisateur
                sb_user.Append("\n\nVotre demande sera traitée dans le plus brefs delai.\n\nL'équipe PFL.");

                // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion

                while (!success && retryCount > 0)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(user.Email, "Récapitulatif Réservation", sb_user.ToString());
                        success = true;
                    }
                    catch (Exception e)
                    {
                        retryCount--;

                        if (retryCount == 0)
                        {
                            ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                            return View("PlanZonesReservation", zonesReservation);  //or handle error and break/return
                        }
                    }
                }

                #endregion

                #region Envoi de mail notifications aux "Admin"/"Logistic"

                // récupérer les "Administrateurs" dont ils ont un rôle supplementaire égal à "Logistic"
                UsersLogistic = await equipeResaDb.ObtenirUsersLogisticAsync();

                sb_admin.Append("\n\nL'équipe PFL.");

                for (int index = 0; index < UsersLogistic.Count(); index++)
                {
                    NumberOfRetries = 5;
                    retryCount = NumberOfRetries;
                    success = false;

                    while (!success && retryCount > 0)
                    {
                        try
                        {
                            await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Notification de réservation pour validation", sb_admin.ToString());
                            success = true;
                        }
                        catch (Exception e)
                        {
                            retryCount--;

                            if (retryCount == 0)
                            {
                                ModelState.AddModelError("", "Problème de connexion pour l'envoie de mail! : " + e.Message + ".");
                                return View("PlanZonesReservation", zonesReservation);  //or handle error and break/return
                            }
                        }
                    }
                }

                #endregion

                //Libérer toutes les sessions ouvertes avant de quitter la vue formulaire, libérer uniquement les sessions concernées par la réservation pour éviter des pbs
                HttpContext.Session.Remove("FormulaireResa");
                HttpContext.Session.Remove("ZoneReservation");
                HttpContext.Session.Remove("EquipementZone");

                // Diriger l'utilisateur vers la vue confirmation
                return View("Confirmation");
            }
            else
            {
                ModelState.AddModelError("", "Vous devez réserver au moins un équipement pour soumettre votre demande");
                return View("PlanZonesReservation", zonesReservation);
            }

            #endregion

            #endregion
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        #region Méthodes complémentaires 

        // Disposer la comm vers la base de données
        protected override void Dispose(bool disposing)
        {
            // Rajouter pour éviter des pbs d'accès aux ressources resaBdd
            projetEssaiDb.Dispose();
            zoneEquipDb.Dispose();
            reservationDb.Dispose();
            equipeResaDb.Dispose();
            userManager.Dispose();

            base.Dispose(disposing);
        }

        // Méthode permettant de retourner les réservations à afficher sur le calendrier
        // uniquement pour les 7 jours de la semaine! (pour le moment) TODO: voir comment faire pour un créneaux long des dates "Du" "Au"!!
        /*public List<ReservationsJour> DonneesCalendrier(int idEquipement)
        {
            // Variables 
            List<ReservationsJour> ListResaSemaine = new List<ReservationsJour>();  // Liste des réservations pour une semaine contenant les réservations par jour
            ReservationsJour ResaJour = new ReservationsJour(); // Reservation pour une journée defini 
            DateTime TodayDate = new DateTime(); // Datetime pour obtenir la date actuelle

            //Afficher toujours à partir du lundi de la semaine en cours 
            TodayDate = DateTime.Now;
            DayOfWeek dow = TodayDate.DayOfWeek;

            // Revenir toujours au lundi de la semaine en cours
            if (dow == DayOfWeek.Tuesday) TodayDate = TodayDate.AddDays(-1);
            if (dow == DayOfWeek.Wednesday) TodayDate = TodayDate.AddDays(-2);
            if (dow == DayOfWeek.Thursday) TodayDate = TodayDate.AddDays(-3);
            if (dow == DayOfWeek.Friday) TodayDate = TodayDate.AddDays(-4);
            if (dow == DayOfWeek.Saturday) TodayDate = TodayDate.AddDays(-5);
            if (dow == DayOfWeek.Sunday) TodayDate = TodayDate.AddDays(-6);

            // for pour recupérer les réservations des 7 jours de la semaine en cours à partir du lundi
            for(int i = 0; i<7; i++)
            {
                // Obtenir l'emploi du temps du jour de la semaine i pour un équipement
                ResaJour = resaBdd.ObtenirReservationsJourXEquipement(TodayDate, idEquipement); 
                // ajouter à la liste de la semaine
                ListResaSemaine.Add(ResaJour);
                // incrementer le nombre des jours à partir du lundi
                TodayDate = TodayDate.AddDays(1);
            }

            return ListResaSemaine;
        }*/

        /// <summary>
        /// Méthode permettant de retourner les réservations à afficher sur le calendrier
        /// </summary>
        /// <param name="IsForOneWeek"> boolean true ou false pour indiquer si l'affichage est de 7 jours ou pas</param>
        /// <param name="idEquipement"> id de l'équipement</param>
        /// <param name="DateDu"> date à partir de laquelle on récupére les réservations</param>
        /// <param name="DateAu"> date jusqu'à laquelle on récupére les réservations</param>
        /// <returns> liste des réservations </returns>
        public List<ReservationsJour> DonneesCalendrierEquipement(bool IsForOneWeek, int idEquipement, DateTime ?DateDu, DateTime ?DateAu)
        {
            // Variables 
            List<ReservationsJour> ListResas = new List<ReservationsJour>();  // Liste des réservations pour une semaine ou pour une durée déterminée
            ReservationsJour ResaJour = new ReservationsJour(); // Reservation pour une journée defini 
            DateTime TodayDate = new DateTime(); // Datetime pour obtenir la date actuelle
            DateTime DateRecup = new DateTime();
            int NbJours = 0;

            //Afficher toujours à partir du lundi de la semaine en cours 
            TodayDate = DateTime.Now;
            DayOfWeek dow = TodayDate.DayOfWeek;

            #region Déterminer s'il s'agit d'un calendrier pour une semaine ou pour une durée déterminée

            switch(IsForOneWeek)
            {
                case true:
                    // Revenir toujours au lundi de la semaine en cours
                    if (dow == DayOfWeek.Tuesday) TodayDate = TodayDate.AddDays(-1);
                    if (dow == DayOfWeek.Wednesday) TodayDate = TodayDate.AddDays(-2);
                    if (dow == DayOfWeek.Thursday) TodayDate = TodayDate.AddDays(-3);
                    if (dow == DayOfWeek.Friday) TodayDate = TodayDate.AddDays(-4);
                    if (dow == DayOfWeek.Saturday) TodayDate = TodayDate.AddDays(-5);
                    if (dow == DayOfWeek.Sunday) TodayDate = TodayDate.AddDays(-6);

                    NbJours = 7;
                    DateRecup = TodayDate;
                    break;
                case false:
                    // Calculer la difference des jours entre la date debut d'affichage et la date fin d'affichage
                    if (DateDu.Equals(DateAu))
                    {
                        // Si la personne souhaite afficher qu'un jour pour le calendrier, afficher la semaine pour éviter d'afficher qu'une colonne
                        NbJours = 7;
                    }
                    else
                    {
                        NbJours = (DateAu.Value - DateDu.Value).Days;    // TODO: Vérifier que le nombre des jours est correct
                        NbJours = NbJours + 1;  // Car il compte pas la bonne quantité 
                        if (NbJours < 7) // Afficher au moins une semaine 
                            NbJours = 7;

                    }
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(DateDu.Value.Year, DateDu.Value.Month, DateDu.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    //DateRecup = DateDu.Value; 
                    break;
            }

            #endregion

            #region Recueil des réservations pour la durée demandée

            // for pour recupérer les réservations des N jours à partir du lundi
            for (int i = 0; i < NbJours; i++)
            {
                // Obtenir l'emploi du temps du jour de la semaine i pour un équipement
                ResaJour = reservationDb.ObtenirReservationsJourEssai(DateRecup, idEquipement);
                // ajouter à la liste de la semaine
                ListResas.Add(ResaJour);
                // incrementer le nombre des jours à partir du lundi
                DateRecup = DateRecup.AddDays(1);
            }

            #endregion 

            return ListResas;
        }

        #endregion
    }
}