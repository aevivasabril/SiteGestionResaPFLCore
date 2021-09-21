﻿using Microsoft.AspNetCore.Authorization;
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
            if (vm.IntervenantExterne == "true") // Si l'utilisateur coche "oui" alors il est obligé de taper le nom de la sociète
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
            // Retry pour envoi mail
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;

            #region Validation des interventions sur des péripheriques à la PFL
            // Récuperer ma session avec les créneaux d'intervention
            AjoutEquipementsViewModel vm = HttpContext.GetFromSession<AjoutEquipementsViewModel>("AjoutEquipementsViewModel");

            // Créer l'opération de maintenance à partir de ma session
            MaintenanceViewModel formulaire = HttpContext.GetFromSession<MaintenanceViewModel>("FormulaireOperation");

            // Traiter chaque créneau d'intervention saisie pour les Equipements sans zone
            #region Enregistrer en base de données et envoyer le mail pour informer les utilisateurs (EQUIPEMENTS SANS ZONE)

            // Création de l'opération de maintenance
            maintenance maintenance = intervDb.AjoutMaintenance(formulaire);

            // Pas de notion pour le type de maintenance pour les équipements sans zone
            foreach (var inter in vm.ListEquipsSansZone)
            {
                bool IsOk = intervDb.EnregistrerIntervSansZone(inter, maintenance);
                if(!IsOk)
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
                               ". " + "Cet intervention aura lieu du: <b> " + inter.DateDebut + "</b> au <b> " + inter.DateFin + "</b>.</p> <p> Cette intervention pénalise vos manips en cours ou à venir." +
                               " Merci d'annuler vos manips et attendre jusqu'à la réception du mail de fin d'intervention. </p> <p>L'équipe PFL, " +
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
            #endregion

            #region Traiter les équipements pour intervention qui sont dans une zone (EQUIPEMENTS DANS UNE ZONE)

            if (maintenance.type_maintenance.Equals("Maintenance curative (Panne)"))
            {
                // J'annule les essais dans la liste et je leur envoie un mail de notification d'annulation
                // Obtenir la liste des essais à supprimer
                foreach (var equipementDansZone in vm.ListEquipsDansZones)
                {
                    foreach (var ess in equipementDansZone.EssaisXAnnulation)
                    {
                        if (intervDb.UpdateEssai(ess.id).resa_refuse == null) // essai pas encore annulé
                        {
                            intervDb.AnnulerEssai(ess, formulaire.CodeMaintenance);
                            // Envoyer le mail au propiètaire de l'essai
                            // obtenir le mail du propietaire
                            string mail = intervDb.ObtenirMailUser(ess.compte_userID);
                            MsgUser = @"<html>
                                            <body> 
                                            <p> Bonjour, <br><br> L'équipe PFL vous informe que votre essai N° " + ess.id + ".Titre essai: <b>" + ess.titreEssai +
                                            "</b> vient d'être annulé automatiquement." + "<br>Une maintenance curative (Panne) sera appliquée les mêmes dates sur un des équipements réservés de votre essai. " +
                                            "<br><br>Descriptif du problème: <b>" + formulaire.DescriptionInter + "</b>" +
                                            ". <br>Code Intervention: <b>" + formulaire.CodeMaintenance + "</b>.<br> Prenez contact avec l'équipe pour reprogrammer votre essai" +
                                            "<br><br> Nous nous excusons du dérangement. </p> <p>L'équipe PFL, " +
                                            "</p>" +
                                            "</body>" +
                                            "</html>";

                            success = false;
                            retryCount = 5;
                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(mail, "Votre essai est annulé", MsgUser);
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
                bool IsOk = intervDb.EnregistrerIntervsDansZone(vm.ListEquipsDansZones, maintenance);
                if (!IsOk)
                {
                    ModelState.AddModelError("", "Problème pour enregistrer les créneaux d'intervention.");
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème pour enregistrer les créneaux d'intervention.";
                    return View("AjoutEquipements", vm);
                }
            }
            else
            { // Dans tous les autres cas, normalement la personne qui saisie l'intervention veille à ne pas faire de conflit (avec un essai "Restreint")

                bool IsOk = intervDb.EnregistrerIntervsDansZone(vm.ListEquipsDansZones, maintenance);
                if (!IsOk)
                {
                    ModelState.AddModelError("", "Problème pour enregistrer les créneaux d'intervention.");
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème pour enregistrer les créneaux d'intervention.";
                    return View("AjoutEquipements", vm);
                }
            }
            // Ajouter les interventions de maintenance dans la BDD
            //intervDb.AjoutInterventions(vm.ListEquipsDansZones, vm.ListEquipsSansZone, maintenance.id);

            #endregion

            #endregion

            return View("Confirmation");
        }
    }
}
