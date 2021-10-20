using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data
{
    [Area("Maintenance")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class MaintenanceController : Controller
    {
        private readonly IFormulaireIntervDB intervDb;
        private readonly IEmailSender emailSender;

        public MaintenanceController(
            IFormulaireIntervDB intervDb,
            IEmailSender emailSender)
        {
            this.intervDb = intervDb;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> SaisirInterventionAsync()
        {
            IList<utilisateur> logistMaint = await intervDb.List_utilisateurs_logistiqueAsync();
            // Obtenir la liste des valeurs à pre-remplir pour charger le formulaire
            var Intervenants = logistMaint.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            var ListMaints = intervDb.List_Type_Maintenances().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_maintenance });
            var code = intervDb.CodeOperation();
            MaintenanceViewModel vm = new MaintenanceViewModel
            {
                IntervenantItem = Intervenants,
                InterventionItem = ListMaints,
                CodeMaintenance = code
            };
            this.HttpContext.AddToSession("FormulaireOperation", vm);

            return View("SaisirIntervention",vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SoumettreInfosMaint(MaintenanceViewModel vm)
        {
            if (vm.IntervenantExterne == "true" && vm.NomSociete == null) // Si l'utilisateur coche "oui" alors il est obligé de taper le nom de la sociète
                ModelState.AddModelError("NomSociete", "Veuillez indiquer le nom de la sociète intervenante");

            MaintenanceViewModel model = HttpContext.GetFromSession<MaintenanceViewModel>("FormulaireOperation");
            vm.CodeMaintenance = model.CodeMaintenance;
            vm.IntervenantItem = model.IntervenantItem;
            vm.InterventionItem = model.InterventionItem;

            if (ModelState.IsValid)
            {
                // Sauvegarder la session data du formulaire intervention
                this.HttpContext.AddToSession("FormulaireOperation", vm);
                return RedirectToAction("AjoutEquipements");
            }
            else
            {
                goto ERR;
            }

        ERR:

            return View("SaisirIntervention", vm);
        }

        /// <summary>
        /// Action qui traite le retour à partir de l'action "ValiderResa" du controleur "Reservation" 
        /// </summary>
        /// <returns></returns>
        public IActionResult AjoutEquipementsExterne()
        {
            // Récupérer la session AjoutEquipementsViewModel 
            AjoutEquipementsViewModel MaintenanceEquipements = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");
            MaintenanceEquipements.OuvrirEquipSansZone = "none";
            return View("AjoutEquipements", MaintenanceEquipements);
        }

        public IActionResult AjoutEquipements()
        {
            AjoutEquipementsViewModel vm = new AjoutEquipementsViewModel()
            {
                //EquipementSansZoneVM = new EquipementSansZoneVM(),
                ListEquipsSansZone = new List<EquipementSansZoneVM>(),
                ListEquipsDansZones = new List<EquipementDansZone>(),
                OuvrirEquipSansZone = "none"
            };
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);
            return View("AjoutEquipements", vm);
        }

        public IActionResult AjouterEquipsSansZone()
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            vm.OuvrirEquipSansZone = "";
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            return View("AjoutEquipements", vm);
        }

        [HttpPost]
        public IActionResult AjouterEquipsSansZone(AjoutEquipementsViewModel model)
        {
            DateTime debutToSave = new DateTime();
            DateTime finToSave = new DateTime();
            EquipementSansZoneVM equipementSansZoneVM = new EquipementSansZoneVM();
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            if(ModelState.IsValid)
            {
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
                    #region Définition des dates réservation avec l'heure selon le créneau choisi
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
                    // complèter le model avec les informations de l'intervention 
                    equipementSansZoneVM.DateDebut = debutToSave;
                    equipementSansZoneVM.DateFin = finToSave;
                    equipementSansZoneVM.DescriptionProbleme = model.DescriptionProbleme;
                    equipementSansZoneVM.ZoneImpacte = model.ZoneImpacte;

                    vm.ListEquipsSansZone.Add(equipementSansZoneVM);
                    // Ajouter dans le model pour afficher uniquement la liste
                    model.ListEquipsSansZone = new List<EquipementSansZoneVM>();
                    model.ListEquipsSansZone.Add(equipementSansZoneVM);

                    // Ouvrir une session pour le viewmodel de la vue AjoutEquipements
                    // Sauvegarder la session avec les données à mettre à jour pour la vue EquipementsVsZones
                    vm.OuvrirEquipSansZone = "none";
                    this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);
                    return View("AjoutEquipements", vm);
                }
                else
                {
                    ModelState.AddModelError("", "La date fin de réservation ne peut pas être inférieure à la date début");
                }
            }
            else
            {
                goto ENDT;
            }
               
            // TODO: Voir si c'est nécessaire de mettre des limites pour le choix 
            /*TimeSpan diff = debutToSave - DateTime.Now;
            if (diff.Hours < 8 && diff.Days == 0) // pour permettre à une personne d'ajouter un équipement avant 10h du matin la veille de la manip
            {
                ModelState.AddModelError("", "Vous ne pouvez pas ajouter un équipement à votre réservation à moins 8 heures avant l'intervention!");
                goto ENDT;
            }
            if (diff.Days < 0)
            {
                ModelState.AddModelError("", "Vous ne pouvez pas ajouter un équipement à votre réservation à une date antérieur!");
                goto ENDT;
            }*/
            ENDT:
            
            return View("AjoutEquipements", vm);
        }


        public IActionResult AjouterEquipsReserv()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AjouterEquipsReserv(AjoutEquipementsViewModel vm)
        {
            return View();
        }

        public IActionResult FermerAjoutSansZone()
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");
            vm.OuvrirEquipSansZone = "none";
            return View("AjoutEquipements", vm);
        }

        /// <summary>
        /// Liberer toutes les sessions de stockage de data lors de l'annulation de l'intervention
        /// </summary>
        /// <returns></returns>
        public ActionResult AnnulerDemandeInter()
        {
            //Libérer toutes les sessions ouvertes avant de quitter la vue formulaire, libérer uniquement les sessions concernées 
            HttpContext.Session.Remove("FormulaireOperation");

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult SupprimerIntervSans(int? i)
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");
            vm.IdPourSuppressionSansZone = i.Value;

            // Variable pour "afficher" le modal pop up de confirmation de suppression
            ViewBag.modalSuppSans = "show";

            // Enregistrer la mise à jour dans la session
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            return View("AjoutEquipements", vm);
        }

        [HttpPost]
        public IActionResult SupprimerIntervSans(AjoutEquipementsViewModel model)
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            vm.ListEquipsSansZone.RemoveAt(model.IdPourSuppressionSansZone);

            // Enregistrer la mise à jour dans la session
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            return View("AjoutEquipements", vm);
        }


        public IActionResult SupprimerIntervDans(int? i)
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");
            vm.IdPourSuppressionSansZone = i.Value;

            // Variable pour "afficher" le modal pop up de confirmation de suppression
            ViewBag.modalSuppDans = "show";

            // Enregistrer la mise à jour dans la session
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            return View("AjoutEquipements", vm);
        }

        [HttpPost]
        public IActionResult SupprimerIntervDans(AjoutEquipementsViewModel model)
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            vm.ListEquipsDansZones.RemoveAt(model.IdPoursuppressionDansZone);

            // Enregistrer la mise à jour dans la session
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            return View("AjoutEquipements", vm);
        }

        /// <summary>
        /// Action pour annuler les créneaux des interventions saisis pour revenir vers la vue formulaire intervention
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AnnulerIntervAsync()
        {
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            // Vider toutes les reservations pour tous les CalendrierChildVM
            vm.ListEquipsSansZone.Clear();

            // Enregistrer la mise à jour dans la session
            this.HttpContext.AddToSession("AjoutEquipementsViewModel", vm);

            MaintenanceViewModel formulaire = HttpContext.GetFromSession<MaintenanceViewModel>("FormulaireOperation");
            //Obtenir la liste des utilisateurs logistique
            IList<utilisateur> logistMaint = await intervDb.List_utilisateurs_logistiqueAsync();
            // Obtenir la liste des valeurs à pre-remplir pour charger le formulaire
            var Intervenants = logistMaint.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
            var ListMaints = intervDb.List_Type_Maintenances().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_maintenance });
            var code = intervDb.CodeOperation();

            formulaire.IntervenantItem = Intervenants;
            formulaire.InterventionItem = ListMaints;
            
            return View("SaisirIntervention", formulaire);
        }

        public async Task<IActionResult> ValiderInterventionsAsync(AjoutEquipementsViewModel model)
        {
            string MsgUser = "";
            bool IsOk = false;
            // Retry pour envoi mail
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;
            string MsgLogist = "";
            IList<utilisateur> UsersLogistic = await intervDb.List_utilisateurs_logistiqueAsync();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification
            essai essXres = new essai();

            #region Validation des interventions sur des péripheriques à la PFL
            // Récuperer ma session avec les créneaux d'intervention
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            // Créer l'opération de maintenance à partir de ma session
            MaintenanceViewModel formulaire = HttpContext.GetFromSession<MaintenanceViewModel>("FormulaireOperation");

            // Traiter chaque créneau d'intervention saisie pour les Equipements sans zone
            #region Enregistrer en base de données et envoyer le mail pour informer les utilisateurs (EQUIPEMENTS SANS ZONE)

            // Afficher message pour indiquer qu'il faut saisir au moins un équipement
            if (vm.ListEquipsDansZones.Count() == 0 && vm.ListEquipsSansZone.Count() == 0)
            {
                ModelState.AddModelError("", "Problème pour enregistrer les créneaux d'intervention.");
                ViewBag.AfficherMessage = true;
                ViewBag.Message = "Vous devez choisir au moins un équipement";
                return View("AjoutEquipements", vm);
            }

            // Création de l'opération de maintenance
            maintenance maintenance = intervDb.AjoutMaintenance(formulaire);

            #region Liste des équipements sans zone
            // Pas de notion pour le type de maintenance pour les équipements sans zone
            if (vm.ListEquipsSansZone.Count() != 0)
            {
                foreach (var inter in vm.ListEquipsSansZone)
                {
                    IsOk = intervDb.EnregistrerIntervSansZone(inter, maintenance);
                    if (!IsOk)
                    {
                        ModelState.AddModelError("DescriptionProbleme", "Problème pour ajouter une nouvelle intervention sans zone, essayez à nouveau");
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème pour ajouter le(s) interventions sans zone, essayez à nouveau";
                        return View("AjoutEquipements", vm);
                    }

                    // si enregistrement OK alors envoyer le mail
                    MsgUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> L'équipe PFL vous informe qu'une intervention du type  : <b> " + maintenance.type_maintenance + "</b> aura lieu dans la ou les zone(s):<b> "
                                   + inter.ZoneImpacte + "</b>. Descriptif du problème: <b>" + inter.DescriptionProbleme + "</b>" +
                                   ". " + "Cet intervention aura lieu à partir du: <b> " + inter.DateDebut + "</b> au <b> et sera finie provisoirement le " + inter.DateFin +
                                   "</b>.</p> <p> Cette intervention pénalise vos manips en cours ou à venir." +
                                   "Vous recevrez un mail pour indiquer la fin de l'opération. En attendant, Merci d'annuler vos essais. </p> <p>L'équipe PFL, " +
                                   "</p>" +
                                   "</body>" +
                                   "</html>";

                    // Obtenir la liste des utilisateurs
                    List<utilisateur> users = intervDb.ObtenirListUtilisateursSite();

                    // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                    foreach (var usr in users)
                    {
                        success = false;
                        while (!success && retryCount > 0)
                        {
                            try
                            {
                                await emailSender.SendEmailAsync(usr.Email, "Dépannage materiel commun et utilités PFL", MsgUser);
                                success = true;
                            }
                            catch (Exception e)
                            {
                                retryCount--;
                                if (retryCount == 0)
                                {
                                    ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                    ViewBag.AfficherMessage = true;
                                    ViewBag.Message = "Problème de connexion pour l'envoie du mail: " + e.Message + ".";
                                    return View("AjoutEquipements", vm);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Liste des équipements dans zone

            if (vm.ListEquipsDansZones.Count() != 0)
            {
                #region Traiter les équipements pour intervention qui sont dans une zone (EQUIPEMENTS DANS UNE ZONE)

                switch (maintenance.type_maintenance)
                {
                    case "Maintenance curative (Dépannage)":
                    case "Equipement en panne":
                    case "Maintenance curative (Dépannage sans blocage zone)":
                        // J'annule les essais dans la liste et je leur envoie un mail de notification d'annulation
                        // Obtenir la liste des essais à supprimer
                        #region Annulation des essais et envoie de mail selon le cas d'intervention

                        foreach (var equipementDansZone in vm.ListEquipsDansZones)
                        {
                            foreach (var IDreser in equipementDansZone.ResasXAnnulation)
                            {
                                essXres = intervDb.ObtenirEssai(IDreser);
                                bool isOk = intervDb.SupprimerReservation(IDreser);
                                if (!isOk)
                                {
                                    ModelState.AddModelError("", "Problème pour supprimer le créneau de réservation");
                                    ViewBag.AfficherMessage = true;
                                    ViewBag.Message = "Problème pour supprimer le créneau de réservation";
                                    return View("AjoutEquipements", vm);
                                }
                                // Envoyer le mail au propiètaire de l'essai
                                string mail = intervDb.ObtenirMailUser(essXres.compte_userID);
                                if (maintenance.type_maintenance.Equals("Equipement en panne")) // Envoie de mail selon le type d'intervention
                                {
                                    #region Mails à envoyer
                                    MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'un des équipements réservés sur votre essai N° " + essXres.id + " est en panne." +
                                        "Titre essai: <b>" + essXres.titreEssai + "</b> Le créneaux réservé pour cet équipement vient d'être supprimé automatiquement." +
                                        "<br><br>Descriptif du problème: <b>" + formulaire.DescriptionInter + "</b>" +
                                        ".<br>Code Intervention: <b>" + formulaire.CodeMaintenance + ".</b> <br> Equipement concerné :<b> " + equipementDansZone.NomEquipement +
                                        "</b>.<br> <p>Nous nous excusons du dérangement.</p> </p> <p>L'équipe PFL, " +
                                        "</p>" +
                                        "</body>" +
                                        "</html>";

                                    MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipement <b>" + equipementDansZone.NomEquipement + "</b> a été déclaré en PANNE provocant l'annulation automatique d'une réservation " +
                                        "de l'essai N°: " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail +
                                        ")</b>. <br><br>Descriptif du problème: <b>" + formulaire.DescriptionInter + "</b>" +
                                        ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.<br>" +
                                        "<br><br> </p> <p>L'équipe PFL, " +
                                        "</p>" +
                                        "</body>" +
                                        "</html>";
                                    #endregion
                                }
                                else if (maintenance.type_maintenance.Equals("Maintenance curative (Dépannage)"))
                                {
                                    #region Mails à envoyer
                                    MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'une des réservations sur votre essai N° " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai +
                                            "</b> vient d'être supprimée automatiquement." + "<br>Une maintenance curative (Dépannage) sera appliquée les mêmes dates, sur un " +
                                            "des équipements réservés de votre essai ou dans la même zone.<br><br>Descriptif du problème: <b>" + formulaire.DescriptionInter + "</b>" +
                                            ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.<br> Equipement concerné: <b>" + equipementDansZone.NomEquipement +                                          
                                            "</b><br><br> <p>Nous nous excusons du dérangement.</p> </p> <p>L'équipe PFL, </p>" +
                                            "</body>" +
                                            "</html>";

                                    MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> Une maintenance curative (Dépannage) sera appliquée les mêmes dates sur une des zones ou un des équipements réservés sur l'essai N°:"
                                        + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail + ")</b>. <br><br>Descriptif du problème: <b>" +
                                        formulaire.DescriptionInter + "</b>" + ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.<br>" + 
                                        "Suppression de la réservation sur l'équipement: <b>" + equipementDansZone.NomEquipement + "</b><br><br> </p> <p>L'équipe PFL, </p>" +
                                        "</body>" +
                                        "</html>";
                                    #endregion
                                }
                                else // cas if(maintenance.type_maintenance.Equals("Maintenance curative (Dépannage sans blocage zone)"))
                                {
                                    #region Mails à envoyer
                                    MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'une des réservations sur votre essai N° " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai +
                                        "</b> vient d'être supprimée automatiquement." + "<br>Une maintenance curative (Dépannage sans blocage de zone) sera appliquée " +
                                        "les mêmes dates, sur un des équipements réservés de votre essai." +
                                        "<br><br>Descriptif du problème: <b>" + formulaire.DescriptionInter + "</b>" +
                                        ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.Equipement concerné: <b>" + equipementDansZone.NomEquipement +
                                        "</b><br><br> <p>Nous nous excusons du dérangement.</p> </p> <p>L'équipe PFL, </p>" +
                                        "</body>" +
                                        "</html>";

                                    MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> Une maintenance curative (Dépannage sans blocage de zone) sera appliquée les mêmes dates sur un des équipements réservés sur l'essai N°:"
                                        + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail + ")</b>. <br><br>Descriptif du problème: <b>" +
                                        formulaire.DescriptionInter + "</b>" + ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.<br>" + "Equipement: <b>" +
                                        equipementDansZone.NomEquipement + "</b><br><br> </p> <p>L'équipe PFL, </p>" +
                                        "</body>" +
                                        "</html>";
                                    #endregion
                                }

                                success = false;
                                retryCount = 5;
                                while (!success && retryCount > 0)
                                {
                                    try
                                    {
                                        await emailSender.SendEmailAsync(mail, "Mise à jour de votre essai", MsgUser);
                                        success = true;
                                    }
                                    catch (Exception e)
                                    {
                                        retryCount--;
                                        if (retryCount == 0)
                                        {
                                            ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                            ViewBag.AfficherMessage = true;
                                            ViewBag.Message = "Problème de connexion pour l'envoie du mail: " + e.Message + ".";
                                            return View("AjoutEquipements", vm);
                                        }
                                    }
                                }

                                #region Envoyer un mail aux utilisateurs logistique

                                for (int index = 0; index < UsersLogistic.Count(); index++)
                                {
                                    NumberOfRetries = 5;
                                    retryCount = NumberOfRetries;
                                    success = false;

                                    while (!success && retryCount > 0)
                                    {
                                        try
                                        {
                                            await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Opération de maintenance annulant un essai", MsgLogist);
                                            success = true;
                                        }
                                        catch (Exception e)
                                        {
                                            retryCount--;

                                            if (retryCount == 0)
                                            {
                                                ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                                ViewBag.AfficherMessage = true;
                                                ViewBag.Message = "Problème de connexion pour l'envoie du mail: " + e.Message + ".";
                                                return View("AjoutEquipements", vm);
                                            }
                                        }
                                    }
                                }
                                #endregion                               
                            }
                        }
                        IsOk = intervDb.EnregistrerIntervsDansZone(vm.ListEquipsDansZones, maintenance);
                        if (!IsOk)
                        {
                            ModelState.AddModelError("", "Problème pour enregistrer les créneaux d'intervention.");
                            ViewBag.AfficherMessage = true;
                            ViewBag.Message = "Problème pour enregistrer les créneaux d'intervention.";
                            return View("AjoutEquipements", vm);
                        }
                        #endregion

                        break;
                    default:// Tous les autres cas on envoie pas de mail de notification, on enregistre uniquement les interventions        
                        // Dans tous les autres cas, normalement la personne qui saisie l'intervention veille à ne pas faire de conflit (avec un essai "Restreint")
                        IsOk = intervDb.EnregistrerIntervsDansZone(vm.ListEquipsDansZones, maintenance);
                        if (!IsOk)
                        {
                            ModelState.AddModelError("", "Problème pour enregistrer les créneaux d'intervention.");
                            ViewBag.AfficherMessage = true;
                            ViewBag.Message = "Problème pour enregistrer les créneaux d'intervention.";
                            return View("AjoutEquipements", vm);
                        }
                        break;
                }
                #endregion
            }

            #endregion


            #endregion


            #endregion

            return View("Confirmation");
        }
    }
}
