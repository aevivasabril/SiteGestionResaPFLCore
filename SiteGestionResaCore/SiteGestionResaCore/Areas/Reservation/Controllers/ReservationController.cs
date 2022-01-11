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
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Areas.Reservation.Data.Reservation;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;

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
            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();

            var typeProj = formDb.ObtenirList_TypeProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_projet});
            var finanItem = formDb.ObtenirList_Financement().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_financement});
            // Création d'une liste Dropdownlist contenant les types d'organismes
            var allOrgs = formDb.ObtenirListOrg().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_organisme});
            var usersList = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"});
            // Création d'une liste de provenance projet (dropdownlist)
            var provProj = formDb.ObtenirList_ProvenanceProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance});
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usersManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"});
            // Création d'une liste dropdownlist pour le type produit entrée
            var prodEntree = formDb.ObtenirList_TypeProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in});
            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProd = formDb.ObtenirList_ProvenanceProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit});
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProd = formDb.ObtenirList_DestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });
            FormulaireProjetViewModel vm = new FormulaireProjetViewModel()
            {
                TypeProjetItem = typeProj,
                TypefinancementItem = finanItem,
                OrganItem = allOrgs,
                RespProjItem = usersList,
                ProvenanceItem = provProj,
                ManipProjItem = usersManip,
                ProductItem = prodEntree,
                ProvenanceProduitItem = provProd,
                DestProduitItem = destProd

            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SoumettreFormulaireAsync(FormulaireProjetViewModel model)
        {
            // NOTE: Dans le cas où on crée un nouveau projet et un nouveau essai
            var user = await userManager.FindByIdAsync(User.GetUserId());
           
            // Je laisse cette ligne car elle permette de vérifier quel champ du formulaire me génère une erreur
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            ModelState.Remove("SelectedEssaiId"); // J'extrait le model error généré par SelectedEssaiId car il est pas pris en compte dans la copie projet, voir commme l'ignorer plus proprement
            //ModelState.Remove("NumProjetXCopie");

            // Charger le model avec les listes
            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();

            var typeProj = formDb.ObtenirList_TypeProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_projet });
            var finanItem = formDb.ObtenirList_Financement().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_financement });
            // Création d'une liste Dropdownlist contenant les types d'organismes
            var allOrgs = formDb.ObtenirListOrg().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_organisme });
            var usersList = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste de provenance projet (dropdownlist)
            var provProj = formDb.ObtenirList_ProvenanceProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance });
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usersManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste dropdownlist pour le type produit entrée
            var prodEntree = formDb.ObtenirList_TypeProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in });
            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProd = formDb.ObtenirList_ProvenanceProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit });
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProd = formDb.ObtenirList_DestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });

            model.TypeProjetItem = typeProj;
            model.TypefinancementItem = finanItem;
            model.OrganItem = allOrgs;
            model.RespProjItem = usersList;
            model.ProvenanceItem = provProj;
            model.ManipProjItem = usersManip;
            model.ProductItem = prodEntree;
            model.ProvenanceProduitItem = provProd;
            model.DestProduitItem = destProd;

            if (ModelState.IsValid)
            {
                //si le projet existe alors on sait qu'il s'agit d'une copie et qu'il faut créer un nouveau essai, même si l'utilisateur ne change rien sur le formulaire
                if(projetEssaiDb.ProjetExists(model.NumProjet))
                {
                    // Si l'utilisateur est propiètaire du projet ou "Admin" ou "MainAdmin" alors autoriser la création d'un essai 
                    if (await projetEssaiDb.VerifPropieteProjetAsync(model.NumProjet, user))
                    {                        
                        // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
                        this.HttpContext.AddToSession("FormulaireResa", model);              
                        return RedirectToAction("PlanZonesReservation");
                    }
                    else
                    {
                        // cas où le projet existe mais l'utilisateur n'a pas les droits
                        return View("FormulaireProjet", model); // Si error alors on recharge la page pour montrer les messages
                    }
                }
                else // si le projet n'existe pas alors l'ajouter dans la session
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
            bool projetValideOk = false;
            // récupérer les projets pour le numéro saisie

            projetValideOk = projetEssaiDb.ProjetExists(model.NumProjetXCopie);
            
            if (!projetValideOk)
            {
                ViewBag.Message = "Ce numéro de projet n'existe pas ou n'a pas été saisie. 'Ignorer' cette fênetre et essayez avec un numéro valide";
            }
            else
            {
                propProjetOk = await projetEssaiDb.VerifPropieteProjetAsync(model.NumProjetXCopie, user);
                if (propProjetOk)
                {
                    ViewBag.Message = "";
                    // Création d'une liste des item avec des détails d'un essai
                    model.EssaiItem = projetEssaiDb.ObtenirList_EssaisUser(model.NumProjetXCopie).Select(f => new SelectListItem
                    {
                        Value = f.CopieEssai.id.ToString(),
                        Text = "Essai crée le " + f.CopieEssai.date_creation.ToString() + " - Manipulateur Essai: " + f.user.nom +
                        ", " + f.user.prenom + " - Titre essai: " + f.CopieEssai.titreEssai +
                        " - Type produit entrant: " + f.CopieEssai.type_produit_entrant + " -" + f.CopieEssai.quantite_produit
                    });

                }
                else
                {
                    ViewBag.Message = "Vous n'avez pas le droit de copier les informations sur ce projet";
                }
            }

            #region Recharge des listes déroulantes

            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();
            var typeProj = formDb.ObtenirList_TypeProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_projet });
            var finanItem = formDb.ObtenirList_Financement().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_financement });
            // Création d'une liste Dropdownlist contenant les types d'organismes
            var allOrgs = formDb.ObtenirListOrg().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_organisme });
            var usersList = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste de provenance projet (dropdownlist)
            var provProj = formDb.ObtenirList_ProvenanceProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance });
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usersManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            // Création d'une liste dropdownlist pour le type produit entrée
            var prodEntree = formDb.ObtenirList_TypeProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in });
            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProd = formDb.ObtenirList_ProvenanceProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit });
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProd = formDb.ObtenirList_DestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });
            model.TypeProjetItem = typeProj;
            model.TypefinancementItem = finanItem;
            model.OrganItem = allOrgs;
            model.RespProjItem = usersList;
            model.ProvenanceItem = provProj;
            model.ManipProjItem = usersManip;
            model.ProductItem = prodEntree;
            model.ProvenanceProduitItem = provProd;
            model.DestProduitItem = destProd;

            #endregion

            ViewBag.modalListe = "show";
            return View("FormulaireProjet", model);
        }

        [HttpPost]
        public ActionResult CopierProjetEssai(FormulaireProjetViewModel model, string numProjet)
        {
            projet pr = new projet();
            essai ess = new essai();

            #region Initialisation du model avec les listes formulaire

            List<utilisateur> listUsersAcces = formDb.ObtenirList_UtilisateurValide();

            var typeProj = formDb.ObtenirList_TypeProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_projet });

            var finanItem = formDb.ObtenirList_Financement().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_financement });

            // Création d'une liste Dropdownlist contenant les types d'organismes
            var allOrgs = formDb.ObtenirListOrg().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_organisme });

            var usersList = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });

            // Création d'une liste de provenance projet (dropdownlist)
            var provProj = formDb.ObtenirList_ProvenanceProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance });

            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var usersManip = listUsersAcces.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });

            // Création d'une liste dropdownlist pour le type produit entrée
            var prodEntree = formDb.ObtenirList_TypeProduitEntree().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_produit_in });

            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var provProd = formDb.ObtenirList_ProvenanceProduit().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_provenance_produit });

            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var destProd = formDb.ObtenirList_DestinationPro().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_destination });

            FormulaireProjetViewModel vm = new FormulaireProjetViewModel()
            {
                TypeProjetItem = typeProj,
                TypefinancementItem = finanItem,
                OrganItem = allOrgs,
                RespProjItem = usersList,
                ProvenanceItem = provProj,
                ManipProjItem = usersManip,
                ProductItem = prodEntree,
                ProvenanceProduitItem = provProd,
                DestProduitItem = destProd

            };

            #endregion

            // Récupérer à partir de la BDD les infos sur le projet et l'essai
            // Vérifier cette ligne suite à migration ASP NET CORE
            pr = projetEssaiDb.ObtenirProjet_pourCopie(model.NumProjetXCopie); // Provenant du HiddenFor ligne 207
            ess = projetEssaiDb.ObtenirEssai_pourCopie(model.SelectedEssaiId);
            vm.SelectTypeProjetId = projetEssaiDb.IdTypeProjetPourCopie(pr.id);
            vm.SelectFinancementId = projetEssaiDb.IdFinancementPourCopie(pr.id);
            vm.SelectedRespProjId = projetEssaiDb.IdRespoProjetPourCopie(pr.id);
            vm.SelectedProvenanceId = projetEssaiDb.IdProvenancePourCopie(pr.id);
            vm.SelectedProveProduitId = projetEssaiDb.IdProvProduitPourCopie(ess.id);
            vm.SelectedDestProduit = projetEssaiDb.IdDestProduitPourCopie(ess.id);
            vm.SelectedProductId = projetEssaiDb.IdProduitInPourCopie(ess.id);

            // Données à copier pour le projet
            vm.TitreProjet = pr.titre_projet;
            vm.NumProjet = pr.num_projet;
            vm.SelectedOrganId = pr.organismeID.GetValueOrDefault();
            vm.DescriptionProjet = pr.description_projet;

            // Données à copier pour l'essai
            vm.ConfidentialiteEssai = ess.confidentialite;
            vm.PrecisionProduitIn = ess.precision_produit;
            vm.QuantiteProduit = ess.quantite_produit;
            vm.TitreEssai = ess.titreEssai;
            vm.TransportSTLO = ess.transport_stlo.ToString();
            vm.SelectedManipulateurID = ess.manipulateurID;

            ModelState.Clear();

            return View("FormulaireProjet", vm);
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
            List<SousListeEquipements> sousListeEquipements = new List<SousListeEquipements>();

            // 1. Obtenir la liste des equipements
            equipements = zoneEquipDb.ListeEquipements(id.Value);

            // pour chaque equipement obtenir la liste des réservations et le sauvegarder dans la liste des reservations
            for (int i = 0; i<equipements.Count(); i++)
            {
                // initialiser pour chaque équipement 
                calenChild = new CalendrierEquipChildViewModel();
                // 2. en prenant l'id de chaque equipement, obtenir la list des reservations pour la semaine en cours
                reservationsSemEquipement = DonneesCalendrierEquipement(true, equipements[i].id, null, null); // TODO: Pas nécessaire?? new List<ReservationsJour>()
                calenChild.ListResas = reservationsSemEquipement;
                calenChild.idEquipement = equipements[i].id;
                calenChild.nomEquipement = equipements[i].nom;
                calenChild.numGmaoEquipement = equipements[i].numGmao;
                calenChild.zoneIDEquipement = equipements[i].zoneID.Value;

                // 3. Rajouter cela dans une liste contenant les données des équipements
                PlanningEquipements.Add(calenChild);
                // mettre à zéro les variables
                calenChild = null;
                sousListeEquipements.Add(new SousListeEquipements { IdEquipement = equipements[i].id, NomEquipement = equipements[i].nom, NumGmaoEquipement = equipements[i].numGmao });
            }

            // Initialisation du view model pour la vue avec les valeurs obtenus ci-dessus
            EquipementsParZoneViewModel vm = new EquipementsParZoneViewModel()
            {
                //Equipements = equipements,
                NomZone = zoneEquipDb.GetNomZone(id.Value),
                IdZone = id.Value,
                PreCalendrierChildVM = PlanningEquipements,
                CalendEquipSelectionnes = new List<CalendrierEquipChildViewModel>(),
                CalendVM = new CalendrierEquipChildViewModel(),
                OpenCalendEtCreneau = "none",
                SousListeEquipements = sousListeEquipements
            };

            // initialiser la date des datepicker au MOIS Selectionné
            // Récupérer la session "EquipementZone" où se trouvent toutes les informations des réservations
            // Récupérer le mois et année sélectionné lors de la première recherche
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            if (equipementZone != null && equipementZone.MoisDatePick != 0 && equipementZone.AnneeDatePick != 0)
            {
                vm.MoisDatePick = equipementZone.MoisDatePick;
                vm.AnneeDatePick = equipementZone.AnneeDatePick;
            }

            // Etablir une session avec les données à mettre à jour pour la vue EquipementsVsZones
            this.HttpContext.AddToSession("EquipementZone",vm);

            // Récupérer le model du formulaire avec les infos saisies lors du remplissage du formulaire (Vérifié)
            //var formulaire = (FormulaireProjetViewModel)this.HttpContext.Session["FormulaireResa"];

            return View(vm);
        }

        public IActionResult VoirPlanningSemaine(EquipementsParZoneViewModel model)
        {
            List<ReservationsJour> reservationsEquipement = new List<ReservationsJour>();
            // Récupérer la session "EquipementZone"
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            //CalendrierEquipChildViewModel vm = equipementZone.CalendrierChildVM.Where(l => l.idEquipement == id).First();
            List<CalendrierEquipChildViewModel> CalendEquipSelectionnes = new List<CalendrierEquipChildViewModel>();
            CalendrierEquipChildViewModel SousCalend = new CalendrierEquipChildViewModel();
            bool AuMoinsUnEquipSelect = false;
            DateTime timeToday = DateTime.Today;

            equipementZone.SousListeEquipements = model.SousListeEquipements;
            
            if(equipementZone.MoisDatePick != 0 && equipementZone.AnneeDatePick != 0)
            {
                #region Vérifier si l'utilisateur a déjà demandé un affichage planning et mettre tout au même mois

                for (int i = 0; i < model.SousListeEquipements.Count(); i++)
                {
                    if(model.SousListeEquipements[i].IsEquipSelect == true)
                    {
                        SousCalend = new CalendrierEquipChildViewModel();
                        // Mettre l'affichage du calendrier semaine au mois et année souhaitée (01/MM/YY)
                        // Recuperer les réservations pour la date souhaitée 
                        reservationsEquipement = DonneesCalendrierEquipement(false, model.SousListeEquipements[i].IdEquipement,
                            new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, timeToday.Day), new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, timeToday.AddDays(7).Day));
                        SousCalend.ListResas = reservationsEquipement;
                        SousCalend.idEquipement = model.SousListeEquipements[i].IdEquipement;
                        SousCalend.nomEquipement = model.SousListeEquipements[i].NomEquipement;
                        SousCalend.numGmaoEquipement = model.SousListeEquipements[i].NumGmaoEquipement;
                        CalendEquipSelectionnes.Add(SousCalend);

                        SousCalend = null;
                        if(AuMoinsUnEquipSelect == false)
                            AuMoinsUnEquipSelect = true;
                    }            
                }               

                // initialiser la date des datepicker au MOIS Selectionné
                equipementZone.CalendVM.DatePickerDu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
                equipementZone.CalendVM.DatePickerAu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
                equipementZone.CalendVM.DateDebut = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
                equipementZone.CalendVM.DateFin = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);

                #endregion
            }
            else
            {
                #region Recupérer les calendriers pour la semaine en cours si aucune recherche ou créneau n'a pas été rajouté

                for(int j=0; j<model.SousListeEquipements.Count(); j++)
                {
                    if(model.SousListeEquipements[j].IsEquipSelect == true)
                    {
                        SousCalend = new CalendrierEquipChildViewModel();
                        SousCalend = equipementZone.PreCalendrierChildVM.Where(l => l.idEquipement == model.SousListeEquipements[j].IdEquipement).FirstOrDefault();
                        CalendEquipSelectionnes.Add(SousCalend);
                        SousCalend = null;

                        if (AuMoinsUnEquipSelect == false)
                            AuMoinsUnEquipSelect = true;
                    }
                }
                #endregion
            }

            if(AuMoinsUnEquipSelect == false)
            {
                ModelState.AddModelError("", "Sélectionnez au moins un équipement pour affichage du calendrier");
                equipementZone.OpenCalendEtCreneau = "none";
            }
            else
            { // Autoriser l'affichage des éléments du calendrier si au moins un équipement est sélectionné
                equipementZone.OpenCalendEtCreneau = null;
            }

            // Rajouter le view model calendrier dans le model "EquipementZone" 
            equipementZone.CalendEquipSelectionnes = CalendEquipSelectionnes;

            // Etablir une session avec les données à mettre à jour pour la vue EquipementsVsZones
            this.HttpContext.AddToSession("EquipementZone", equipementZone);

            return View("EquipementVsZone", equipementZone);
        }

        public IActionResult FermerCalend()
        {
            // Récupérer la session "EquipementZone"
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");
            equipementZone.OpenCalendEtCreneau = "none";

            // Etablir une session avec les données à mettre à jour pour la vue EquipementsVsZones
            this.HttpContext.AddToSession("EquipementZone", equipementZone);

            return View("EquipementVsZone", equipementZone);
        }

        /// <summary>
        /// Affichage du calendrier d'une date X à une date Y choisie par l'utilisateur 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AfficherPlanning(CalendrierEquipChildViewModel model)
        {
            // Initialisation des variables
            List<CalendrierEquipChildViewModel> PlanningEquipements = new List<CalendrierEquipChildViewModel>();
            List<ReservationsJour> reservationsEquipement = new List<ReservationsJour>();
            List<CalendrierEquipChildViewModel> CalendEquipSelectionnes = new List<CalendrierEquipChildViewModel>();
            CalendrierEquipChildViewModel SousCalend = new CalendrierEquipChildViewModel();

            // Récupérer la session "EquipementZone"
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession< EquipementsParZoneViewModel>("EquipementZone");

            // Il faut supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DateDebut");
            ModelState.Remove("DateFin");
            ModelState.Remove("DatePickerDebut_Matin");
            ModelState.Remove("DatePickerFin_Matin");

            if (ModelState.IsValid) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                #region Sauvegarder le mois sélectionné pour mettre tous les datepicker à la même date

                equipementZone.MoisDatePick = model.DatePickerDu.Value.Month;
                equipementZone.AnneeDatePick = model.DatePickerDu.Value.Year;

                #endregion

                if (model.DatePickerDu.Value <= model.DatePickerAu.Value)
                {
                    // pour chaque model de la vue calendrier (c'est à dire pour chaque équipement)
                    for (int i = 0; i < equipementZone.SousListeEquipements.Count(); i++)
                    {
                        if (equipementZone.SousListeEquipements[i].IsEquipSelect == true)
                        {
                            SousCalend = new CalendrierEquipChildViewModel();
                            // 2. en prenant l'id de chaque equipement, obtenir la list des reservations pour la semaine en cours
                            reservationsEquipement = DonneesCalendrierEquipement(false, equipementZone.SousListeEquipements[i].IdEquipement, model.DatePickerDu.Value, model.DatePickerAu.Value);
                            SousCalend.ListResas = reservationsEquipement;
                            SousCalend.idEquipement = equipementZone.SousListeEquipements[i].IdEquipement;
                            SousCalend.nomEquipement = equipementZone.SousListeEquipements[i].NomEquipement;
                            SousCalend.numGmaoEquipement = equipementZone.SousListeEquipements[i].NumGmaoEquipement;
                            CalendEquipSelectionnes.Add(SousCalend);
                            SousCalend = null;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                }
                // initialiser la date des datepicker au MOIS Selectionné
                equipementZone.CalendVM.DatePickerDu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, model.DatePickerDu.Value.Day);
                equipementZone.CalendVM.DatePickerAu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, model.DatePickerAu.Value.Day);
                equipementZone.CalendVM.DateDebut = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
                equipementZone.CalendVM.DateFin = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);

                equipementZone.OpenCalendEtCreneau = null;

                // Rajouter le view model calendrier dans le model "EquipementZone" 
                equipementZone.CalendEquipSelectionnes = CalendEquipSelectionnes;
                // Sauvegarder la session avec les données à mettre à jour pour la vue EquipementsVsZones
                this.HttpContext.AddToSession("EquipementZone", equipementZone);
            }
            return View("EquipementVsZone", equipementZone);
        }

        /// <summary>
        /// méthode permettant d'ajouter un créneau de réservation et de revenir sur la même vue en mettant la liste à jour
        /// </summary>
        /// <param name="model">model view provenant de la vue partielle "_creneau"</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<ActionResult> AjouterResaAsync(CalendrierEquipChildViewModel model) // model presque "vide" (contient les datetime)
        {
            DateTime debutToSave = new DateTime();
            DateTime finToSave = new DateTime();
            //List<essai> EssaisXAnnulation = new List<essai>();

            // Récupérer la session "EquipementZone" https://stackoverflow.com/questions/27517912/persisting-information-between-two-view-requests-in-mvc (VOIR CE LIEN)
            EquipementsParZoneViewModel equipementZone = HttpContext.GetFromSession<EquipementsParZoneViewModel>("EquipementZone");

            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");

            // DatePickerDu et DatePickerAu lançent un erreur de modelstate, il faut les supprimer de la liste d'erreur puisque ils sont pas utilisés sur cette partie
            ModelState.Remove("DatePickerDu");
            ModelState.Remove("DatePickerAu");

            if (ModelState.IsValid) // Ajouter le créneau de réservation dans la liste
            {
                #region Sauvegarder le mois sélectionné pour mettre tous les datepicker à la même date

                equipementZone.MoisDatePick = model.DateDebut.Value.Month;
                equipementZone.AnneeDatePick = model.DateDebut.Value.Year;

                #endregion

                if (model.DateDebut.Value == model.DateFin.Value)
                {
                    if ((Convert.ToBoolean(model.DatePickerDebut_Matin) == false) && (Convert.ToBoolean(model.DatePickerFin_Matin) == true))
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
                if (model.DateDebut.Value <= model.DateFin.Value) // si la date debut est inférieure à la date fin alors OK
                {
                    // Etablir l'heure de début et de fin selon les créneaux choisis (Matin ou après-midi)
                    #region Définition des dates réservation avec l'heure selon le créneau choisi (réservations et maintenance)
                    // Definition date debut

                    if (Convert.ToBoolean(model.DatePickerDebut_Matin) == true) // definir l'heure de début à 7h
                    {
                        debutToSave = new DateTime(model.DateDebut.Value.Year,
                            model.DateDebut.Value.Month, model.DateDebut.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    }
                    else // Début de manip l'après-midi à 13h
                    {
                        debutToSave = new DateTime(model.DateDebut.Value.Year,
                            model.DateDebut.Value.Month, model.DateDebut.Value.Day, 13, 0, 0, DateTimeKind.Local);
                    }
                    // Definition date fin
                    if (Convert.ToBoolean(model.DatePickerFin_Matin) == true)
                    {
                        finToSave = new DateTime(model.DateFin.Value.Year,
                            model.DateFin.Value.Month, model.DateFin.Value.Day, 12, 0, 0, DateTimeKind.Local);
                    }
                    else // Fin de la manip 18h
                    {
                        finToSave = new DateTime(model.DateFin.Value.Year,
                            model.DateFin.Value.Month, model.DateFin.Value.Day, 18, 0, 0, DateTimeKind.Local);
                    }
                    #endregion

                    #region Opérations de maintenance (vérifier la variable indicant s'il s'agit d'une intervention)

                    if (zonesReservation.ReservationXIntervention) // Si ReservationXIntervention = true alors il s'agit d'une intervention maintenance
                    {
                        // Récupérer la session de sauvegarde pour l'intervention
                        MaintenanceViewModel formulaire = HttpContext.GetFromSession<MaintenanceViewModel>("FormulaireOperation");
                        // Récupérer la session avec la liste des équipements pour rajouter les essais à annuler
                        AjoutEquipementsViewModel MaintenanceEquipements = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

                        // Obtenir le nom du type d'intervention
                        string nomTypeInterv = reservationDb.ObtenirNomTypeMaintenance(formulaire.SelectedInterventionId);
                        // Vérifier le type d'intervention pour décider l'action de vérification à exécuter
                        switch(nomTypeInterv)
                        {
                            case "Maintenance curative (Dépannage avec blocage de zone)":
                                for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                {
                                    // Vérifier qu'il n'y pas des interventions ou des essais en cours sur cette zone car la maintenance curative (dépannage) a besoin de la zone
                                    // Si false alors une intervention est déjà déclarée pour les mêmes dates sur la zone
                                    if(reservationDb.VerifDisponibilitZoneEquipSurInterventions(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement) == false )
                                    {
                                        ModelState.AddModelError("", "Zone indisponible car une intervention est déjà déclarée aux mêmes dates sur un des équipements");
                                        goto ENDT;
                                    }

                                    // Dans ce cas récupérer tous les essais se déroulant à ces dates pour cet équipement pour les annuler dans le controlleur maintenance
                                    EquipementDansZone equipementDansZone = new EquipementDansZone
                                    {
                                        ResasXAnnulation = reservationDb.ObtenirListResasXAnnulationZone(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        DateDebutInterv = debutToSave,
                                        DateFinInterv = finToSave,
                                        IdEquipementXIntervention = equipementZone.CalendEquipSelectionnes[i].idEquipement,
                                        NomEquipement = reservationDb.ObtenirNomEquipement(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        ZoneImpacte = reservationDb.ObtenirNomZoneImpacte(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        NumGMAO = reservationDb.ObtenirNumGMAOEquip(equipementZone.CalendEquipSelectionnes[i].idEquipement)
                                    };
                                    
                                    MaintenanceEquipements.ListEquipsDansZones.Add(equipementDansZone);
                                    this.HttpContext.AddToSession("AjoutEquipementsViewModel", MaintenanceEquipements);
                                }
                                break;
                            case "Maintenance curative (Dépannage sans blocage zone)": 
                            case "Equipement en panne (blocage équipement)":
                                // Vérifier qu'il n'y pas des interventions sur le même équipement à ces dates                               
                                for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                {
                                    // TODO: il faut vérifier sur les interventions blocant la zone aussi!
                                    if(reservationDb.VerifDisponibilitEquipSurInterventions(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement) == false)
                                    {
                                        ModelState.AddModelError("", "Equipement indisponible car une intervention est déjà déclarée aux mêmes dates");
                                        goto ENDT;
                                    }
                                    var resas = reservationDb.ObtenirListResasXAnnulationEquipement(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                    // Dans ce cas récupérer tous les essais se déroulant à ces dates pour cet équipement pour les annuler dans le controlleur maintenance
                                    EquipementDansZone equipementDansZone = new EquipementDansZone
                                    {
                                        ResasXAnnulation = resas,
                                        DateDebutInterv = debutToSave,
                                        DateFinInterv = finToSave,
                                        IdEquipementXIntervention = equipementZone.CalendEquipSelectionnes[i].idEquipement,
                                        NomEquipement = reservationDb.ObtenirNomEquipement(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        ZoneImpacte = reservationDb.ObtenirNomZoneImpacte(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        NumGMAO = reservationDb.ObtenirNumGMAOEquip(equipementZone.CalendEquipSelectionnes[i].idEquipement)
                                    };
                                    MaintenanceEquipements.ListEquipsDansZones.Add(equipementDansZone);
                                    this.HttpContext.AddToSession("AjoutEquipementsViewModel", MaintenanceEquipements);
                                }
                                break;                           
                            case "Maintenance préventive (Interne avec blocage de zone)": 
                            case "Maintenance préventive (Externe avec blocage de zone)":
                            case "Amélioration (avec blocage de zone)":
                                bool isResaOkToAdd = false;

                                #region Vérification de disponibilité pour les dates saisies avant de le stocker dans le model
                                for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                {
                                    // Vérifier qu'il n'y a pas une autre intervention sur la zone aux mêmes dates que l'intervention qu'on déclare
                                    isResaOkToAdd = reservationDb.ZoneDisponibleXIntervention(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                    if (isResaOkToAdd == false)
                                    {
                                        if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                        {
                                            ModelState.AddModelError("", "Equipement ou zone indisponible pour les dates choisies. Consultez le calendrier et rectifiez votre réservation");
                                            goto ENDT;
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                            goto ENDT;
                                        }
                                    }
                                    // Dans ce cas récupérer tous les essais se déroulant à ces dates pour cet équipement pour les annuler dans le controlleur maintenance
                                    //List<essai> ListEssaiXAnnulation = reservationDb.ObtenirListEssaiXAnnulation(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                    EquipementDansZone equipementDansZone = new EquipementDansZone
                                    {
                                        ResasXAnnulation = new List<int>(),
                                        DateDebutInterv = debutToSave,
                                        DateFinInterv = finToSave,
                                        IdEquipementXIntervention = equipementZone.CalendEquipSelectionnes[i].idEquipement,
                                        NomEquipement = reservationDb.ObtenirNomEquipement(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        ZoneImpacte = reservationDb.ObtenirNomZoneImpacte(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        NumGMAO = reservationDb.ObtenirNumGMAOEquip(equipementZone.CalendEquipSelectionnes[i].idEquipement)
                                    };
                                    MaintenanceEquipements.ListEquipsDansZones.Add(equipementDansZone);
                                    this.HttpContext.AddToSession("AjoutEquipementsViewModel", MaintenanceEquipements);
                                }
                                #endregion
                                break;
                            case "Maintenance préventive (Interne sans blocage de zone)":
                            case "Maintenance préventive (Externe sans blocage de zone)":
                            case "Amélioration (sans blocage de zone)":
                                isResaOkToAdd = false;                                
                                for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                {
                                    // Je fais la vérification comme pour une réservation standard considerant que la maintenance surveillera à ne pas avoir des essais aux mêmes dates
                                    isResaOkToAdd = reservationDb.EquipementDisponibleXIntervention(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                    if (isResaOkToAdd == false)
                                    {
                                        if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                        {
                                            ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre réservation");
                                            goto ENDT;
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                            goto ENDT;
                                        }
                                    }
                                }
                                #region Vérification de disponibilité pour les dates saisies avant de le stocker dans le model
                                for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                {
                                    // Dans ce cas récupérer tous les essais se déroulant à ces dates pour cet équipement pour les annuler dans le controlleur maintenance
                                    //List<essai> ListEssaiXAnnulation = reservationDb.ObtenirListEssaiXAnnulation(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                    EquipementDansZone equipementDansZone = new EquipementDansZone
                                    {
                                        ResasXAnnulation = new List<int>(),
                                        DateDebutInterv = debutToSave,
                                        DateFinInterv = finToSave,
                                        IdEquipementXIntervention = equipementZone.CalendEquipSelectionnes[i].idEquipement,
                                        NomEquipement = reservationDb.ObtenirNomEquipement(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        ZoneImpacte = reservationDb.ObtenirNomZoneImpacte(equipementZone.CalendEquipSelectionnes[i].idEquipement),
                                        NumGMAO = reservationDb.ObtenirNumGMAOEquip(equipementZone.CalendEquipSelectionnes[i].idEquipement)
                                    };
                                    MaintenanceEquipements.ListEquipsDansZones.Add(equipementDansZone);
                                    this.HttpContext.AddToSession("AjoutEquipementsViewModel", MaintenanceEquipements);
                                }
                                #endregion
                                break;                                                                               
                        }
                    }
                    else
                    { // Cas des réservations standard
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
                        if (zonesReservation.IdEssaiXAjoutEquip == 0) // Dans le cas de réservation standard vérifier le seuil de 7 jours avant 
                        {
                            if (diff.Days < 3 && IsLogistic == false) // Délai de 3 jours pour les personnes qui sont pas dans le groupe des utilisateurs "Logistic"
                            {
                                ModelState.AddModelError("", "Vous ne pouvez pas réserver un équipement à moins de 3 jours avant l'essai");
                                goto ENDT;
                            }
                        }
                        else
                        {
                            if (IsLogistic == false)
                            {
                                if (diff.Hours < 21 && diff.Days == 0) // pour permettre à une personne d'ajouter un équipement avant 10h du matin la veille de la manip
                                {
                                    ModelState.AddModelError("", "Vous ne pouvez pas ajouter un équipement à votre réservation à moins d'un jour du début de votre essai!");
                                    goto ENDT;
                                }
                                if (diff.Days < 0)
                                {
                                    ModelState.AddModelError("", "Vous ne pouvez pas ajouter un équipement à votre réservation à une date antérieur!");
                                    goto ENDT;
                                }
                            }

                        }
                        #endregion

                        #region Sauvegarde réservation dans la liste des créneaux saisies (seulement View model)

                        #region Vérification de disponibilité pour les dates saisies avant de le stocker dans le model

                        bool isResaOkToAdd = false;

                        if (zonesReservation.IdEssaiXAjoutEquip == 0) // Utiliser la vérification pour une réservation standard
                        {
                            // Récupérer la session "FormulaireProjet" pour obtenir la confidentialité de l'essai
                            FormulaireProjetViewModel formulaire = HttpContext.GetFromSession<FormulaireProjetViewModel>("FormulaireResa");
                            switch (formulaire.ConfidentialiteEssai)
                            {
                                case "Ouvert":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.VerifDisponibilitéEquipementOuvert(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                                case "Restreint":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.VerifDisponibilitéEquipementRestreint(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                                case "Confidentiel":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.VerifDisponibilitéEquipementConfidentiel(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Un des équipements de la zone est pris aux mêmes dates. Veuillez consulter le calendrier et rectifier votre réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Certains équipements de la zone sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                            }   
                        }
                        else // cas où l'on rajoute des équipements à une réservation existante
                        {
                            // selon le type de confidentialité, vérifier la disponibilité des équipements
                            // obtenir l'essai où les réservations devront être ajoutées
                            essai Essai = projetEssaiDb.ObtenirEssai_pourCopie(zonesReservation.IdEssaiXAjoutEquip);
                            switch (Essai.confidentialite)
                            {
                                case "Ouvert":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.VerifDisponibilitéEquipementOuvert(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                                case "Restreint":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.DispoEssaiRestreintPourAjout(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement, Essai.id);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                                case "Confidentiel":
                                    for (int i = 0; i < equipementZone.CalendEquipSelectionnes.Count(); i++)
                                    {
                                        isResaOkToAdd = reservationDb.DispoEssaiConfidentielPourAjout(debutToSave, finToSave, equipementZone.CalendEquipSelectionnes[i].idEquipement, Essai.id);
                                        if (isResaOkToAdd == false)
                                        {
                                            if (equipementZone.CalendEquipSelectionnes.Count() == 1)
                                            {
                                                ModelState.AddModelError("", "Equipement indisponible pour les dates choisies. Veuillez rectifier votre modification de réservation");
                                                goto ENDT;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", "Les équipements sont indisponibles pour les dates choisies. Consultez le calendrier PFL et veuillez rectifier votre réservation");
                                                goto ENDT;
                                            }
                                        }
                                    }
                                    break;
                            }

                        }

                        #endregion                       

                        #endregion
                    }

                    // Si on arrive ici ça veut dire que tous les équipements de la sélection utilisateur sont dispos! il faut les rajouter dans la liste des créneaux réservation
                    for (int j = 0; j < equipementZone.CalendEquipSelectionnes.Count(); j++)
                    {
                        // Initialiser le calendrierChildModel "model" de l'équipement sur le model de la vue parent avant toutes les opérations
                        for (int i = 0; i < equipementZone.PreCalendrierChildVM.Count(); i++)
                        {
                            if (equipementZone.PreCalendrierChildVM[i].idEquipement == equipementZone.CalendEquipSelectionnes[j].idEquipement)
                            {
                                equipementZone.PreCalendrierChildVM[i].ResaEquipement.Add(
                                    new ReservationTemp
                                    {
                                        date_debut = debutToSave,
                                        date_fin = finToSave
                                    });
                            }
                        }

                    }

                    equipementZone.OpenCalendEtCreneau = "none";

                    this.HttpContext.AddToSession("EquipementZone", equipementZone);
                    #endregion
                }
                else
                {
                    ModelState.AddModelError("", "La date fin de réservation ne peut pas être inférieure à la date début");
                }
            }
            ENDT:
            #region initialiser la date des datepicker au MOIS Selectionné
            equipementZone.CalendVM.DatePickerDu = new DateTime(model.DateDebut.Value.Year, model.DateDebut.Value.Month, model.DateDebut.Value.Day);
            equipementZone.CalendVM.DatePickerAu = new DateTime(model.DateFin.Value.Year, model.DateFin.Value.Month, model.DateFin.Value.Day);
            equipementZone.CalendVM.DateDebut = new DateTime(model.DateDebut.Value.Year, model.DateDebut.Value.Month, model.DateDebut.Value.Day);
            equipementZone.CalendVM.DateFin = new DateTime(model.DateFin.Value.Year, model.DateFin.Value.Month, model.DateFin.Value.Day);
            #endregion

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
            
            // Garder la maj dans la session
            this.HttpContext.AddToSession("EquipementZone", equipementZone);

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

            equipementZone.PreCalendrierChildVM[model.IndiceChildModel].ResaEquipement.RemoveAt(model.IndiceResaEquipXChild);

            #region initialiser la date des datepicker au MOIS Selectionné
            equipementZone.CalendVM.DatePickerDu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
            equipementZone.CalendVM.DatePickerAu = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
            equipementZone.CalendVM.DateDebut = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
            equipementZone.CalendVM.DateFin = new DateTime(equipementZone.AnneeDatePick, equipementZone.MoisDatePick, 1);
            #endregion

            // Sauvegarder la maj de la session
            this.HttpContext.AddToSession("EquipementZone", equipementZone);

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

            // Sauvegarder la mise à jour de la session
            this.HttpContext.AddToSession("ZoneReservation", zonesReservation);


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
            for(int i = 0; i < equipementZone.PreCalendrierChildVM.Count(); i++)
            {
                equipementZone.PreCalendrierChildVM[i].ResaEquipement.Clear();
            }

            // Sauvegarder la mise à jour de la session
            this.HttpContext.AddToSession("EquipementZone", equipementZone);

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

            // Sauvegarder la mise à jour de la session
            this.HttpContext.AddToSession("ZoneReservation", zonesReservation);
            if(zonesReservation.IdEssaiXAjoutEquip == 0 && zonesReservation.ReservationXIntervention == true) // Intervention maintenance
            {
                return RedirectToAction("AjoutEquipementsExterne", "Maintenance", new { area = "Maintenance" });
            }
            if(zonesReservation.IdEssaiXAjoutEquip == 0 && zonesReservation.ReservationXIntervention == false) // si il s'agit d'une réservation standard
            {
                // Récupérer la session "FormulaireProjet" pour revenir au formulaire
                FormulaireProjetViewModel formulaire = HttpContext.GetFromSession<FormulaireProjetViewModel>("FormulaireResa");
                return View("FormulaireProjet", formulaire);
            }
            if(zonesReservation.IdEssaiXAjoutEquip !=0) // Modification d'une réservation
            {
                return RedirectToAction("ModifierEquipResa", "ResasUser", new { area = "User", id= zonesReservation.IdEssaiXAjoutEquip });
            }
            // Récupérer la session "FormulaireProjet" pour revenir au formulaire
            FormulaireProjetViewModel form = HttpContext.GetFromSession<FormulaireProjetViewModel>("FormulaireResa");
            return View("FormulaireProjet", form);
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

        /// <summary>
        /// Action POST qui traite un cas de réservation normal et un cas d'ajout des équipements sur un essai existant
        /// 2 cas différentes à traiter et a differencier grâce à la variable IdEssaiXAjoutEquip dans le view model ZonesReservationViewModel
        /// </summary>
        /// <param name="vm">View model retourné de la vue</param>
        /// <returns></returns>
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

            string messUser;
            string mssLogis;

            #endregion

            #region Récupération session

            // Récupérer la session "ZonesReservation" pour récupérer les infos sur le plan + les réservations des zones
            ZonesReservationViewModel zonesReservation = HttpContext.GetFromSession<ZonesReservationViewModel>("ZoneReservation");

            // Récupérer les infos remplisses dans le formulaire projet/essai
            FormulaireProjetViewModel formulaire = HttpContext.GetFromSession<FormulaireProjetViewModel>("FormulaireResa");

            #endregion          

            // Pour retrouver l'erreur quand j'essaie de créer la list en RELEASE
            try
            {
                resas = new List<reservation_projet>();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.ToString());
                return View("PlanZonesReservation", zonesReservation);
            }

            #region Vérifier que au moins un équipement est réservé

            // Vérifier qu'au moins un équipement a été réservé
            for (int i = 0; i < zonesReservation.EquipementsParZone.Count(); i++)
            {
                for (int j = 0; j < zonesReservation.EquipementsParZone[i].PreCalendrierChildVM.Count(); j++)
                {
                    if (zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement.Count() != 0)
                    {
                        AtLeastOneEquipment = true;
                        goto Creation;
                    }
                }
            }       
            
            #endregion
         

        Creation:
            if (AtLeastOneEquipment) // si au moins un équipement est réservé!
            {
                #region Distinguer les opérations de maintenance ou réservations

                if (zonesReservation.ReservationXIntervention) // Si ReservationXIntervention = true alors il s'agit d'une intervention maintenance
                {
                    
                    return RedirectToAction("AjoutEquipementsExterne", "Maintenance", new { area = "Maintenance" });
                }
                else
                { // Réservation standard ou pour ajout des équipements

                    #region Vérifier s'il s'agit d'une réservation standard ou un ajout des équipements dans un essai existant

                    if (zonesReservation.IdEssaiXAjoutEquip != 0) // il s'agit d'un ajout des équipements dans un essai existant! (modification réservation)
                    {
                        #region Ajouter des équipements à un essai ou diriger vers la page d'erreur

                        // obtenir l'essai où les réservations devront être ajoutées
                        Essai = projetEssaiDb.ObtenirEssai_pourCopie(zonesReservation.IdEssaiXAjoutEquip);

                        // obtenir le projet 
                        Proj = projetEssaiDb.ObtenirProjXEssai(Essai.projetID);

                        // Remplir le message à envoyer aux admins pour notifier la réservation
                        mssLogis = @"<html>
                            <body> 
                            <p> Bonjour, <br> " + "L'utilisateur: " + user.Email + " " + " vient d'ajouter des équipements dans sa réservation pour le projet : <b> " + Proj.num_projet + "</b> (Essai N°: " + Essai.id + " ) " +
                                    " " + ". Récapitulatif des équipements ajoutés: <br> "
                                    + "</p>";

                        mssLogis += @"<table>
                                <tr>
                                    <th> Zone </th>
                                    <th> Equipement </th>
                                    <th> Date début </th>
	                                <th> Date fin </th>
                                </tr>";

                        // Remplir le message à envoyer à l'utilisateur avec récap des équipements réservés
                        messUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous avez ajouté des équipements sur votre essai, projet N° : <b> " + Proj.num_projet + "</b> (Essai N°: " + Essai.id + " ) " +
                                    ". " + "Voici le récapitulatif des réservations par équipement ajoutés: <br> "
                                    + "</p>";
                        messUser += @"<table>
                                <tr>
                                    <th> Zone </th>
                                    <th> Equipement </th>
                                    <th> Date début </th>
	                                <th> Date fin </th>
                                </tr>";

                        // Créations des réservations par équipement 
                        for (int i = 0; i < zonesReservation.EquipementsParZone.Count(); i++)
                        {
                            for (int j = 0; j < zonesReservation.EquipementsParZone[i].PreCalendrierChildVM.Count(); j++)
                            {
                                for (int y = 0; y < zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement.Count(); y++)
                                {
                                    resa = reservationDb.CreationReservation(zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].idEquipement,
                                        Essai, zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut,
                                        zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin);

                                    #region Creation d'un string contenant le récap réservation

                                    subNomEquip = zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].nomEquipement + " ( N°GMAO: " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].numGmaoEquipement + " )";

                                    mssLogis += @" <tr> <td>" + zonesReservation.EquipementsParZone[i].NomZone
                                        + "   </td>" + "<td>   " + subNomEquip
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut.ToString()
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()
                                        + "   </td> </tr>";


                                    messUser += @" <tr> <td>" + zonesReservation.EquipementsParZone[i].NomZone
                                        + "   </td>" + "<td>   " + subNomEquip
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut.ToString()
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()
                                        + "   </td> </tr>";

                                    #endregion
                                }
                            }
                        }


                        // Changer le status de l'essai à Waiting4Valid
                        projetEssaiDb.UpdateStatusEssai(Essai);

                        #region Envoi du mail récapitulatif à l'utilisateur
                        // Envoyer le mail récapitulatif utilisateur
                        messUser += @"</table>                               
                                <p>
                                <br>Votre demande sera traitée dans les plus brefs délais.<br><br>
	                                L'équipe PFL,
                                </p>
                                </body>
                                </html>";

                        // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion

                        while (!success && retryCount > 0)
                        {
                            try
                            {
                                await emailSender.SendEmailAsync(user.Email, "Récapitulatif Mise à jour réservation", messUser);
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

                        //sb_admin.Append("\n\nL'équipe PFL.");
                        mssLogis += @"</table>                               
                                <p>
                                <br>L'équipe PFL,
                                </p>
                                </body>
                                </html>";

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
                                        return View("PlanZonesReservation", zonesReservation);  //or handle error and break/return
                                    }
                                }
                            }
                        }

                        #endregion

                        #endregion

                        // envoyer une variable pour savoir si la confirmation doit envoyer vers l'area user (Vue mes reservations) ou vers l'accueil (réservation standard)
                        ViewBag.IsConfirOutside = true;
                        ViewBag.IdEssaiModifie = zonesReservation.IdEssaiXAjoutEquip;
                    }
                    else // réservation standard
                    {
                        #region Créer la réservation "standard" ou diriger vers la page d'erreur

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
                        Essai = projetEssaiDb.CreationEssai(Proj, user, myDateTime, formulaire.ConfidentialiteEssai, formulaire.SelectedManipulateurID, formulaire.SelectedProductId,
                            formulaire.PrecisionProduitIn, formulaire.QuantiteProduit, formulaire.SelectedProveProduitId, formulaire.SelectedDestProduit, formulaire.TransportSTLO,
                            formulaire.TitreEssai);

                        // Remplir le message à envoyer aux admins pour notifier la réservation
                        mssLogis = @"<html>
                            <body> 
                            <p> Bonjour, <br> La demande de réservation pour le projet N° : <b> " + formulaire.NumProjet + "</b> (Essai N°: " + Essai.id + " ) " +
                                    " saisie par l'utilisateur: " + user.Email + " " + " vient d'être rajoutée. Récapitulatif des réservations par équipement: <br> "
                                    + "</p>";

                        mssLogis += @"<table>
                                <tr>
                                    <th> Zone </th>
                                    <th> Equipement </th>
                                    <th> Date début </th>
	                                <th> Date fin </th>
                                </tr>";

                        // Remplir le message à envoyer à l'utilisateur avec récap des équipements réservés
                        messUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> La demande de réservation pour le projet N° : <b> " + formulaire.NumProjet + "</b> (Essai N°: " + Essai.id + " ) " +
                                    " est pris en compte. " + "Récapitulatif des réservations par équipement: <br> "
                                    + "</p>";
                        //sb_user.Append(String.Format("{0,55} {1,135} {2,60}\n\n", "Equipement", "Date début", "Date Fin"));
                        messUser += @"<table>
                                <tr>
                                    <th> Zone </th>
                                    <th> Equipement </th>
                                    <th> Date début </th>
	                                <th> Date fin </th>
                                </tr>";

                        // Créations des réservations par équipement 
                        for (int i = 0; i < zonesReservation.EquipementsParZone.Count(); i++)
                        {
                            for (int j = 0; j < zonesReservation.EquipementsParZone[i].PreCalendrierChildVM.Count(); j++)
                            {
                                for (int y = 0; y < zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement.Count(); y++)
                                {
                                    resa = reservationDb.CreationReservation(zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].idEquipement,
                                        Essai, zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut,
                                        zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin);

                                    // ajouter la réservation dans la liste des réservations à utiliser pour le calcul si "confidentiel"
                                    resas.Add(resa);

                                    #region Creation d'un string contenant le récap réservation

                                    subNomEquip = zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].nomEquipement + " ( N°GMAO: " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].numGmaoEquipement + " )";

                                    mssLogis += @" <tr> <td>" + zonesReservation.EquipementsParZone[i].NomZone
                                        + "   </td>" + "<td>   " + subNomEquip
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut.ToString()
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()
                                        + "   </td> </tr>";


                                    messUser += @" <tr> <td>" + zonesReservation.EquipementsParZone[i].NomZone
                                        + "   </td>" + "<td>   " + subNomEquip
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_debut.ToString()
                                        + "   </td>" + "<td>   " + zonesReservation.EquipementsParZone[i].PreCalendrierChildVM[j].ResaEquipement[y].date_fin.ToString()
                                        + "   </td> </tr>";

                                    #endregion
                                }
                            }
                        }

                        #region Envoi du mail récapitulatif à l'utilisateur
                        // Envoyer le mail récapitulatif utilisateur
                        messUser += @"</table>                               
                                <p>
                                <br>Votre demande sera traitée dans les plus brefs délais.<br><br>
	                                L'équipe PFL,
                                </p>
                                </body>
                                </html>";

                        // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion

                        while (!success && retryCount > 0)
                        {
                            try
                            {
                                await emailSender.SendEmailAsync(user.Email, "Récapitulatif Réservation", messUser);
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

                        //sb_admin.Append("\n\nL'équipe PFL.");
                        mssLogis += @"</table>                               
                                <p>
                                <br>L'équipe PFL,
                                </p>
                                </body>
                                </html>";

                        for (int index = 0; index < UsersLogistic.Count(); index++)
                        {
                            NumberOfRetries = 5;
                            retryCount = NumberOfRetries;
                            success = false;

                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Notification de réservation pour validation", mssLogis);
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

                        #endregion

                        // envoyer une variable pour savoir si la confirmation doit envoyer vers l'area user (Vue mes reservations) ou vers l'accueil (réservation standard)
                        ViewBag.IsConfirOutside = false;
                    }

                    #endregion
                }
                #endregion
            }
            else
            {
                ModelState.AddModelError("", "Vous devez réserver au moins un équipement pour soumettre votre demande");
                return View("PlanZonesReservation", zonesReservation);
            }

            //Libérer toutes les sessions ouvertes avant de quitter la vue formulaire, libérer uniquement les sessions concernées par la réservation pour éviter des pbs
            HttpContext.Session.Remove("FormulaireResa");
            HttpContext.Session.Remove("ZoneReservation");
            HttpContext.Session.Remove("EquipementZone");

            // Diriger l'utilisateur vers la vue confirmation
            return View("Confirmation");

        }

        public ActionResult Confirmation()
        {
            return View();
        }

        /// <summary>
        /// Action appellée de l'Area "USER" pour ajouter des équipements à une réservation existante
        /// </summary>
        /// <param name="id">id essai(area 'User')</param>
        /// <returns></returns>
        public ActionResult PlanZonesReservationXAjout(int id)
        {
            ZonesReservationViewModel vm = new ZonesReservationViewModel()
            {
                Zones = zoneEquipDb.ListeZones(),
                IdEssaiXAjoutEquip = id
            };
            // Etablir une session avec les données à mettre à jour pour la vue principale
            this.HttpContext.AddToSession("ZoneReservation", vm);
            return View("PlanZonesReservation",vm);
        }

        /// <summary>
        /// Action pour ouvrir le plan des zones pour intervention (maintenance!)
        /// </summary>
        /// <returns></returns>
        public IActionResult PlanZonesReserXIntervention()
        {
            ZonesReservationViewModel vm = new ZonesReservationViewModel()
            {
                Zones = zoneEquipDb.ListeZones(),
                ReservationXIntervention = true
            };
            // Etablir une session avec les données à mettre à jour pour la vue principale
            this.HttpContext.AddToSession("ZoneReservation", vm);
            return View("PlanZonesReservation", vm);
        }

        #region Méthodes complémentaires

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
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(TodayDate.Year, TodayDate.Month, TodayDate.Day, 7, 0, 0, DateTimeKind.Local);
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
                        NbJours = (DateAu.Value - DateDu.Value).Days;    
                        NbJours = NbJours + 1;  // Car il compte pas la bonne quantité 
                        if (NbJours < 7) // Afficher au moins une semaine 
                            NbJours = 7;

                    }
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(DateDu.Value.Year, DateDu.Value.Month, DateDu.Value.Day, 7, 0, 0, DateTimeKind.Local);
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