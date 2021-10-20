using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Areas.Maintenance.Data.Modification;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Controllers
{
    [Area("Maintenance")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class ModifMaintController : Controller
    {
        private readonly IModifMaintenanceDB modifMaintDb;
        private readonly IEmailSender emailSender;
        private readonly IReservationDb reservationDb;

        public ModifMaintController(
        IModifMaintenanceDB modifMaintDb,
            IEmailSender emailSender,
            IReservationDb reservationDb)
        {
            this.emailSender = emailSender;
            this.reservationDb = reservationDb;
            this.modifMaintDb = modifMaintDb;
        }
        public IActionResult ModificationIntervention()
        {
            ModifMaintenanceVM vm = new ModifMaintenanceVM();
            vm.OpenModifInter = vm.Ferme;
            vm.InfosMaint = new MaintenanceInfos();
            vm.ListEquipsCommuns = new List<EquipCommunXInterv>();
            vm.ListeEquipsPfl = new List<EquipPflXIntervention>();
            return View("ModificationIntervention", vm);
        }

        [HttpPost]
        public IActionResult TrouverIntervention(ModifMaintenanceVM vm)
        {
            // Vérifier que le numéro d'intervention existe!
            bool NumMaintExist = modifMaintDb.NumMaintExist(vm.NumMaintenance); // uniquement pour les interventions non supprimées

            if(NumMaintExist)
            {
                MaintenanceInfos m = modifMaintDb.ObtenirInfosMaint(vm.NumMaintenance);
                vm.OpenModifInter = vm.Ouvert;
                vm.InfosMaint = m;
                vm.ListeEquipsPfl = modifMaintDb.ListIntervPFL(m.IdMaint);
                vm.ListEquipsCommuns = modifMaintDb.ListMaintAdj(m.IdMaint);
                // Sauvegarder la session data du view model car il a les infos sur l'intervention complète
                this.HttpContext.AddToSession("ModifMaintVM", vm);
            }
            else
            {
                ModelState.AddModelError("NumMaintenance", "Le numéro d'intervention n'existe pas ou l'opération a été annulée");
                vm.OpenModifInter = vm.Ferme;
                vm.InfosMaint = new MaintenanceInfos();
                vm.ListEquipsCommuns = new List<EquipCommunXInterv>();
                vm.ListeEquipsPfl = new List<EquipPflXIntervention>();
            }

            return View("ModificationIntervention", vm);
        }

        public IActionResult ModifierInterCom(int id)
        {
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            Model.IdIntervCom = id;
            Model.OpenModifInter = Model.Ouvert;

            ViewBag.modalModif = "show";
            return View("ModificationIntervention", Model);
        }

        /// <summary>
        /// Action pour modifier la date fin d'un equipement commun 
        /// envoi de mail à tous les utilisateurs pour les informer de la mise à jour
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="id">id resa_maint_equip_adjacent</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ModifierInterCommunAsync(ModifMaintenanceVM vm, int id)
        {
            DateTime NewDate = new DateTime();
            bool success = false;
            var retryCount = 5;
            string MsgUser = "";

            // Obtenir les infos maintenance
            maintenance maint = modifMaintDb.ObtenirMaintenanceXInterv(id);

            // Si la personne n'a pas choisi de date ou créneau
            if (ModelState.IsValid == false )
            {
                ModelState.AddModelError("", "Sélectionnez une date et un créneau");
                // Récupérer la session VM
                ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
                Model.IdIntervCom = id;
                Model.OpenModifInter = vm.Ouvert;
                Model.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id); // On met à jour uniquement les interventions communs 

                ViewBag.modalModif = "show";
                return View("ModificationIntervention", Model);
            }
            else
            {
                // Déterminer la date à sauvegarder
                if (Convert.ToBoolean(vm.DatePickerFin_Matin) == true) // definir l'heure de début à 7h
                {
                    NewDate = new DateTime(vm.DateFin.Value.Year,
                        vm.DateFin.Value.Month, vm.DateFin.Value.Day, 12, 0, 0, DateTimeKind.Local);
                }
                else // Début de manip l'après-midi à 13h
                {
                    NewDate = new DateTime(vm.DateFin.Value.Year,
                        vm.DateFin.Value.Month, vm.DateFin.Value.Day, 18, 0, 0, DateTimeKind.Local);
                }

                if (NewDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Sélectionnez une date supérieure à aujourd'hui");
                    // Récupérer la session VM
                    ModifMaintenanceVM Modell = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
                    Modell.IdIntervCom = id;
                    Modell.OpenModifInter = vm.Ouvert;
                    Modell.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id); // On met à jour uniquement les interventions communs 

                    ViewBag.modalModif = "show";
                    return View("ModificationIntervention", Modell);
                }
                else
                {
                   
                    // Mettre à jour la date pour l'intervention des équipements communs
                    DateTime AncienneDateFin = modifMaintDb.ChangeDateFinEquipCommun(id, NewDate);

                    resa_maint_equip_adjacent resaCommun = modifMaintDb.ObtenirIntervEquiComm(id);
                    // Envoyer le mail à tous les utilisateurs pour informer de cette nouvelle date
                    // si enregistrement OK alors envoyer le mail
                    MsgUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> L'équipe PFL vous informe que la date fin pour l'intervention du type  : <b> " + maint.type_maintenance + "</b>. Code d'intervention:<b> "
                                   + maint.code_operation + "</b>. Descriptif du problème: <b>" + maint.description_operation + "</b> a été mise à jour."
                                    + "<p>Ancienne date: <b> " + resaCommun.date_debut + " au  " + AncienneDateFin + " </b></p><p> Nouvelle date: <b>" + resaCommun.date_debut + " au " +
                                    resaCommun.date_fin + "</b></p></b>.</p> <p> Cette intervention pénalise vos manips en cours ou à venir." +
                                   " Merci d'annuler vos manips et attendre jusqu'à la réception du mail de fin d'intervention. </p> <p>L'équipe PFL, " +
                                   "</p>" +
                                   "</body>" +
                                   "</html>";
                    // Obtenir la liste des utilisateurs
                    List<utilisateur> users = modifMaintDb.ObtenirListUtilisateursSite();

                    // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                    foreach (var usr in users)
                    {
                        success = false;
                        while (!success && retryCount > 0)
                        {
                            try
                            {
                                await emailSender.SendEmailAsync(usr.Email, "Mise à jour intervention dépannage materiel commun", MsgUser);
                                success = true;
                            }
                            catch (Exception e)
                            {
                                retryCount--;
                                if (retryCount == 0)
                                {
                                    ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                    //ViewBag.AfficherMessage = true;
                                    //ViewBag.Message = "Problème de connexion pour l'envoie du mail: " + e.Message + ".";
                                    return View("ModificationIntervention", vm);
                                }
                            }
                        }
                    }

                    // Récupérer la session VM
                    ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
                    Model.IdIntervCom = id;
                    Model.OpenModifInter = vm.Ouvert;
                    Model.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id);
                    // Sauvegarder la session data du view model car il a les infos sur l'intervention complète
                    this.HttpContext.AddToSession("ModifMaintVM", Model);

                    ViewBag.modalModif = "";
                    return View("ModificationIntervention", Model);
                }
                
            }
            
        }

        /// <summary>
        /// Action pour mettre à jour la date fin avec la date actuelle pour indiquer que l'intervention est finie
        /// </summary>
        /// <param name="id">id resa_maint_equip_adjacent</param>
        /// <returns></returns>
        public IActionResult IntervCommunFini(int id)
        {          
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            Model.IdIntervCom = id;
            // Vérifier que la date d'aujourd'hui est supérieur à la date début intervention pour pouvoir finir l'intervention
            var inter = modifMaintDb.ObtenirIntervEquiComm(id);
            if(inter.date_debut < DateTime.Now) // on peut modifier 
            {
                ViewBag.modalValid = "show";
                return View("ModificationIntervention", Model);
            }
            else
            {
                ModelState.AddModelError("", "L'intervention n'ayant pas commencée encore, vous ne pouvez pas clôturer l'opération");
                return View("ModificationIntervention", Model);
            }     
        }

        /// <summary>
        /// Action pour confirmer la finalisation d'une intervention
        /// </summary>
        /// <param name="id">id resa_maint_equip_adjacent</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmIntervComAsync(int id)
        {
            DateTime NewDate = new DateTime();
            bool success = false;
            var retryCount = 5;
            string MsgUser = "";

            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");

            // Obtenir les infos maintenance
            maintenance maint = modifMaintDb.ObtenirMaintenanceXInterv(id);

            // Déterminer si on est le matin ou l'aprèm
            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 12) // heure fin à mettre au format du matin
            {
                NewDate = new DateTime(DateTime.Today.Year,
                    DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0, DateTimeKind.Local);
            }
            else // Heure fin aprèm
            {
                NewDate = new DateTime(DateTime.Today.Year,
                    DateTime.Today.Month, DateTime.Today.Day, 18, 0, 0, DateTimeKind.Local);
            }

            // Mettre à jour la date pour l'intervention des équipements communs
            DateTime AncienneDateFin = modifMaintDb.ChangeDateFinEquipCommun(id, NewDate);

            resa_maint_equip_adjacent resaCommun = modifMaintDb.ObtenirIntervEquiComm(id);

            // Envoyer le mail à tous les utilisateurs pour informer de cette nouvelle date
            // si enregistrement OK alors envoyer le mail
            MsgUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> L'équipe PFL vous informe que l'intervention du type  : <b> " + maint.type_maintenance + "</b>. Code d'intervention:<b> "
                           + maint.code_operation + "</b>. Descriptif du problème: <b>" + maint.description_operation + " VIENT D'ETRE CLOTUREE.</b>"
                           + "<p> Vous pouvez désormais reprogrammer vos essais. </p> <p>L'équipe PFL, " +
                           "</p>" +
                           "</body>" +
                           "</html>";
            // Obtenir la liste des utilisateurs
            List<utilisateur> users = modifMaintDb.ObtenirListUtilisateursSite();

            // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
            foreach (var usr in users)
            {
                success = false;
                while (!success && retryCount > 0)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(usr.Email, "Clôture intervention dépannage materiel commun", MsgUser);
                        success = true;
                    }
                    catch (Exception e)
                    {
                        retryCount--;
                        if (retryCount == 0)
                        {
                            ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                            return View("ModificationIntervention", Model);
                        }
                    }
                }
            }

            Model.IdIntervCom = id;
            Model.OpenModifInter = Model.Ouvert;
            Model.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id);
            // Sauvegarder la session data du view model car il a les infos sur l'intervention complète
            this.HttpContext.AddToSession("ModifMaintVM", Model);

            ViewBag.modalModif = "";
            return View("ModificationIntervention", Model);
        
        }

        public IActionResult ModifierInterPfl(int id)
        {
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            Model.IdIntervPfl = id;
            Model.OpenModifInter = Model.Ouvert;

            ViewBag.modalModifPfl = "show";
            return View("ModificationIntervention", Model);
        }

        /// <summary>
        /// Action pour modifier la date fin d'une maintenance sur des équipements PFL
        /// Prise en compte du type de maintenance pour pouvoir changer la date
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="id">id reservation_maintenance</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ModifierIntervPflAsync(ModifMaintenanceVM vm, int id)
        {
            DateTime NewDate = new DateTime();
            // Retry pour envoi mail
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;
            string MsgUser = "";
            string MsgLogist = "";
            IList<utilisateur> UsersLogistic = await modifMaintDb.List_utilisateurs_logistiqueAsync();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification
            List<int> ListResasXAnnulation = new List<int>();
            essai essXres = new essai();
            string NomEquipement = "";
            bool DateOkPourModif = false;

            // Obtenir les infos maintenance
            maintenance maint = modifMaintDb.ObtenirMaintenanceXIntervPFl(id);
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");

            // Si la personne n'a pas choisi de date ou créneau
            if (ModelState.IsValid == false) // la date choisie doit etre superieure à la date d'aujourd'hui
            {
                Model.IdIntervPfl = id;
                Model.OpenModifInter = vm.Ouvert;
                Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id); //Mettre à jour uniquement les interventions PFL

                ViewBag.modalModifPfl = "show";
                return View("ModificationIntervention", Model);
            }
            else
            {                
                // Déterminer la date à sauvegarder
                if (Convert.ToBoolean(vm.DatePickerFin_Matin) == true) // definir l'heure de début à 7h
                {
                    NewDate = new DateTime(vm.DateFin.Value.Year,
                        vm.DateFin.Value.Month, vm.DateFin.Value.Day, 12, 0, 0, DateTimeKind.Local);
                }
                else // Début de manip l'après-midi à 13h
                {
                    NewDate = new DateTime(vm.DateFin.Value.Year,
                        vm.DateFin.Value.Month, vm.DateFin.Value.Day, 18, 0, 0, DateTimeKind.Local);
                }

                if (NewDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Sélectionnez une date et un créneau supérieur à aujourd'hui");
                    Model.IdIntervPfl = id;
                    Model.OpenModifInter = vm.Ouvert;
                    Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id); //Mettre à jour uniquement les interventions PFL

                    ViewBag.modalModifPfl = "show";
                    return View("ModificationIntervention", Model);
                }
                else
                {
                    // récupérer l'intervention reservation_maintenance
                    var intervention = modifMaintDb.ObtenirIntervEquipPfl(id);
                    NomEquipement = modifMaintDb.NomEquipement(intervention.equipementID);
                    switch (maint.type_maintenance)
                    {
                        case "Equipement en panne":
                        case "Maintenance curative (Dépannage sans blocage zone)":
                            // Vérifier la disponibilité sur les interventions
                            DateOkPourModif = modifMaintDb.VerifDisponibilitEquipSurInterventions(intervention.date_debut, NewDate, intervention.equipementID);
                            if (!DateOkPourModif)
                            {
                                ModelState.AddModelError("DateFin", "La date choisie est en conflit avec une autre intervention. Vérifier la disponibilité sur le calendrier PFL");
                                Model.IdIntervCom = id;
                                Model.OpenModifInter = vm.Ouvert;
                                //Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id); //Mettre à jour uniquement les interventions PFL

                                ViewBag.modalModifPfl = "show";
                                return View("ModificationIntervention", Model);
                            }
                            else
                            {
                                // Si un nouveau essai utilise l'équipement alors les annuler (essais non refusés et non supprimés)
                                ListResasXAnnulation = reservationDb.ObtenirListResasXAnnulationEquipement(intervention.date_debut, NewDate, intervention.equipementID);
                                // Annuler l'essai et envoyer mail au propiètaire pour l'informer
                                foreach (var Idreservation in ListResasXAnnulation)
                                {
                                    essXres = modifMaintDb.ObtenirEssai(Idreservation);
                                    bool isOk = modifMaintDb.SupprimerReservation(Idreservation);
                                    if (!isOk)
                                    {
                                        ModelState.AddModelError("", "Problème pour supprimer le créneau de réservation");
                                        ViewBag.AfficherMessage = true;
                                        ViewBag.Message = "Problème pour supprimer le créneau de réservation";
                                        return View("AjoutEquipements", vm);
                                    }
                                    string mail = modifMaintDb.ObtenirMailUser(essXres.compte_userID);

                                    #region Mail à envoyer
                                    if (maint.type_maintenance.Equals("Equipement en panne"))
                                    {
                                        MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'un des équipements réservés sur votre essai N° " + essXres.id + " est en panne." +
                                            "Titre essai: <b>" + essXres.titreEssai + "</b> Le créneaux réservé pour cet équipement vient d'être supprimé automatiquement." +
                                            "<br><br>Descriptif du problème: <b>" + maint.description_operation + "</b>" +
                                            ".<br>Code Intervention: <b>" + maint.code_operation + ".</b> <br> Equipement concerné :<b> " + NomEquipement +
                                            "</b>.<br> <p>Nous nous excusons du dérangement.</p> </p> <p>L'équipe PFL, " +
                                            "</p>" +
                                            "</body>" +
                                            "</html>";

                                        MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipement <b>" + NomEquipement + "</b> a été déclaré en PANNE provocant l'annulation automatique d'une réservation " +
                                            "de l'essai N°: " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail +
                                            ")</b>. <br><br>Descriptif du problème: <b>" + maint.description_operation + "</b>" +
                                            ". <br>Code Intervention: <b>" + maint.code_operation + "</b>.<br>" +
                                            "<br><br> </p> <p>L'équipe PFL, " +
                                            "</p>" +
                                            "</body>" +
                                            "</html>";
                                    }
                                    else
                                    {
                                        MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'une des réservations sur votre essai N° " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai +
                                            "</b> vient d'être supprimée automatiquement." + "<br>Une maintenance curative (Dépannage) sera appliquée les mêmes dates," +
                                            " sur un des équipements réservés. <br><br>Descriptif du problème: <b>" + maint.description_operation + "</b>" +
                                            ". <br>Code Intervention: <b>" + maint.code_operation + "</b>.Equipement concerné: " + NomEquipement +
                                            "<br><br> <p>Nous nous excusons du dérangement. </p></p> <p>L'équipe PFL, </p>" +
                                            "</body>" +
                                            "</html>";

                                        MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> Une maintenance curative (Dépannage) sera appliquée les mêmes dates sur un des équipements réservés sur l'essai N°:"
                                            + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail + ")</b>. <br><br>Descriptif du problème: <b>" +
                                            maint.description_operation + "</b>" + ". <br>Code Intervention: <b>" + maint.code_operation + "</b>.<br>" + "Equipement: " +
                                            NomEquipement + "<br><br> </p> <p>L'équipe PFL, </p>" +
                                            "</body>" +
                                            "</html>";
                                    }
                                    #endregion

                                    #region Envoi de mail pour le propiètaire essai

                                    success = false;
                                    retryCount = 5;
                                    while (!success && retryCount > 0)
                                    {
                                        try
                                        {
                                            await emailSender.SendEmailAsync(mail, "Modification automatique de votre essai", MsgUser);
                                            success = true;
                                        }
                                        catch (Exception e)
                                        {
                                            retryCount--;
                                            if (retryCount == 0)
                                            {
                                                ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                                return View("ModificationIntervention", Model);
                                            }
                                        }
                                    }
                                    #endregion

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
                                                await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Mise à jour Opération de maintenancei", MsgLogist);
                                                success = true;
                                            }
                                            catch (Exception e)
                                            {
                                                retryCount--;

                                                if (retryCount == 0)
                                                {
                                                    ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                                    return View("ModificationIntervention", Model);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                // modifier la datefin pour l'intervention
                                modifMaintDb.ChangeDateFinEquipPFL(id, NewDate);
                            }                           
                            break;
                        case "Maintenance curative (Dépannage)": // blocage de toute la zone concernée
                            // Vérifier qu'il n'y pas des interventions ou des essais en cours sur cette zone car la maintenance curative (dépannage) a besoin de la zone
                            // Si false alors une intervention est déjà déclarée pour les mêmes dates sur la zone
                            if (reservationDb.VerifDisponibilitZoneEquipSurInterventions(intervention.date_debut, NewDate, intervention.equipementID) == false)
                            {
                                ModelState.AddModelError("DateFin", "La date choisie est en conflit avec une autre intervention ayant lieu sur la même zone. Vérifiez la disponibilité sur le calendrier PFL");
                                Model.IdIntervCom = id;
                                Model.OpenModifInter = vm.Ouvert;

                                ViewBag.modalModifPfl = "show";
                                return View("ModificationIntervention", Model);
                            }
                            // Si un nouveau essai a été rajoutée alors les annuler (essais non refusés et non supprimés)
                            ListResasXAnnulation = reservationDb.ObtenirListResasXAnnulationZone(intervention.date_debut, NewDate, intervention.equipementID);
                            // Annuler l'essai et envoyer mail au propiètaire pour l'informer
                            foreach (var IDreser in ListResasXAnnulation)
                            {
                                essXres = modifMaintDb.ObtenirEssai(IDreser);
                                bool isOk = modifMaintDb.SupprimerReservation(IDreser);
                                if (!isOk)
                                {
                                    ModelState.AddModelError("", "Problème pour supprimer le créneau de réservation");
                                    ViewBag.AfficherMessage = true;
                                    ViewBag.Message = "Problème pour supprimer le créneau de réservation";
                                    return View("AjoutEquipements", vm);
                                }

                                string mail = modifMaintDb.ObtenirMailUser(essXres.compte_userID);

                                MsgUser = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> L'équipe PFL vous informe qu'un des équipements réservés sur votre essai N° " + essXres.id + ".Titre essai: <b>" + essXres.titreEssai +
                                        "</b> vient d'être supprimé automatiquement." + "<br> Une maintenance curative (Dépannage) sera appliquée les mêmes dates, sur " +
                                        "cet équipement ou dans la même zone.<br><br> Descriptif du problème: <b>" + maint.description_operation + "</b>" +
                                        ". <br>Code Intervention: <b>" + maint.code_operation + "</b>.<br> Equipement concerné: " + NomEquipement +
                                        "<br><br> Nous nous excusons du dérangement. </p> <p>L'équipe PFL, </p>" +
                                        "</body>" +
                                        "</html>";

                                MsgLogist = @"<html>
                                        <body> 
                                        <p> Bonjour, <br><br> Une maintenance curative (Dépannage) sera appliquée les mêmes dates sur une des zones ou un des équipements réservés sur l'essai N°:"
                                        + essXres.id + ".Titre essai: <b>" + essXres.titreEssai + " (Propietaire de l'essai: " + mail + ")</b>. <br><br>Descriptif du problème: <b>" +
                                        maint.description_operation + "</b>" + ". <br>Code Intervention: <b>" + maint.code_operation + "</b>.<br>" +
                                        "Suppression de la réservation sur l'équipement: " + NomEquipement + "<br><br> </p> <p>L'équipe PFL, </p>" +
                                        "</body>" +
                                        "</html>";

                                #region Envoi de mail pour le propiètaire essai

                                success = false;
                                retryCount = 5;
                                while (!success && retryCount > 0)
                                {
                                    try
                                    {
                                        await emailSender.SendEmailAsync(mail, "Modification automatique de votre essai", MsgUser);
                                        success = true;
                                    }
                                    catch (Exception e)
                                    {
                                        retryCount--;
                                        if (retryCount == 0)
                                        {
                                            ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                            return View("ModificationIntervention", Model);
                                        }
                                    }
                                }
                                #endregion

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
                                            await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Mise à jour Opération de maintenance", MsgLogist);
                                            success = true;
                                        }
                                        catch (Exception e)
                                        {
                                            retryCount--;

                                            if (retryCount == 0)
                                            {
                                                ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                                return View("ModificationIntervention", Model);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            // modifier la datefin pour l'intervention
                            modifMaintDb.ChangeDateFinEquipPFL(id, NewDate);
                            break;
                        case "Maintenance préventive (Interne)": // vérifier que la zone est dispo
                        case "Maintenance préventive (Externe)":
                        case "Amélioration":
                            // Si un essai est en conflit alors indiquer à l'opérateur de choisir une autre date
                            // Vérifier que la date choisie ne se croise pas avec des autres essais ou maintenances
                            DateOkPourModif = modifMaintDb.ModifZoneDisponibleXIntervention(intervention.date_debut, NewDate, intervention.equipementID, maint.id);
                            if (!DateOkPourModif)
                            {
                                ModelState.AddModelError("DateFin", "La date choisie est en conflit avec un autre essai ou maintenance. Vérifier la disponibilité sur le calendrier PFL");
                                Model.IdIntervCom = id;
                                Model.OpenModifInter = vm.Ouvert;
                                //Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id); //Mettre à jour uniquement les interventions PFL

                                ViewBag.modalModifPfl = "show";
                                return View("ModificationIntervention", Model);
                            }
                            else
                            {
                                // modifier la date fin de mon intervention
                                modifMaintDb.ChangeDateFinEquipPFL(id, NewDate);
                            }
                            break;
                        case "Maintenance préventive (Interne sans blocage de zone)":
                        case "Maintenance préventive (Externe sans blocage de zone)":
                        case "Amélioration (sans blocage de zone)":
                            // Si un essai est en conflit alors indiquer à l'opérateur de choisir une autre date
                            // Vérifier que la date choisie ne se croise pas avec des autres essais ou maintenances
                            DateOkPourModif = modifMaintDb.ModifEquipementDisponibleXIntervention(intervention.date_debut, NewDate, intervention.equipementID, maint.id);
                            if (!DateOkPourModif)
                            {
                                ModelState.AddModelError("DateFin", "La date choisie est en conflit avec un autre essai ou maintenance. Vérifier la disponibilité sur le calendrier PFL");
                                Model.IdIntervCom = id;
                                Model.OpenModifInter = vm.Ouvert;
                                //Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id); //Mettre à jour uniquement les interventions PFL

                                ViewBag.modalModifPfl = "show";
                                return View("ModificationIntervention", Model);
                            }
                            else
                            {
                                // modifier la date fin de mon intervention
                                modifMaintDb.ChangeDateFinEquipPFL(id, NewDate);
                            }
                            break;                      
                    }
                    Model.IdIntervPfl = id;
                    Model.OpenModifInter = Model.Ouvert;
                    Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id);
                    // Sauvegarder la session data du view model car il a les infos sur l'intervention complète
                    this.HttpContext.AddToSession("ModifMaintVM", Model);
                }             
            }
            return View("ModificationIntervention", Model);
        }

        
        public IActionResult IntervPflFini(int id)
        {
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            Model.IdIntervPfl = id;
            // Vérifier que la date d'aujourd'hui est supérieur à la date début intervention pour pouvoir finir l'intervention
            var inter = modifMaintDb.ObtenirIntervEquipPfl(id);
            if (inter.date_debut < DateTime.Now) // on peut modifier 
            {
                ViewBag.modalValidPfl = "show";
                return View("ModificationIntervention", Model);
            }
            else
            {
                ModelState.AddModelError("", "L'intervention n'ayant pas commencée encore, vous ne pouvez pas clôturer l'opération");
                return View("ModificationIntervention", Model);
            }
        }

        [HttpPost]
        public IActionResult ConfirmIntervPfl(int id)
        {
            // Obtenir les infos maintenance
            maintenance maint = modifMaintDb.ObtenirMaintenanceXIntervPFl(id);

            DateTime NewDate = new DateTime();
            // Déterminer si on est le matin ou l'aprèm
            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 12) // heure fin à mettre au format du matin
            {
                NewDate = new DateTime(DateTime.Today.Year,
                    DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0, DateTimeKind.Local);
            }
            else // Heure fin aprèm
            {
                NewDate = new DateTime(DateTime.Today.Year,
                    DateTime.Today.Month, DateTime.Today.Day, 18, 0, 0, DateTimeKind.Local);
            }
            // modifier la datefin pour l'intervention
            modifMaintDb.ChangeDateFinEquipPFL(id, NewDate);
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(maint.id);

            return View("ModificationIntervention", Model);
        }

        /// <summary>
        /// Action pour ouvrir une pop-up et saisir la raison de suppression
        /// </summary>
        /// <param name="id">id maintenance</param>
        /// <returns></returns>
        public IActionResult SupprimerMaintenance(int id)
        {
            // Prendre en compte les maintenances supprimées lors du chargement du calendrier 
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            //Model.IdIntervPfl = id;
            Model.OpenModifInter = Model.Ouvert;

            ViewBag.modalSuppression = "show";
            return View("ModificationIntervention", Model);
        }

        /// <summary>
        /// Action pour supression de maintenance
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="id">id maintenance</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SupprimerOpeMaintAsync(ModifMaintenanceVM vm, int id)
        {
            // Récupérer la session VM
            ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
            string MsgUser = "";
            bool success = false;
            int retryCount = 4;

            if (vm.RaisonSuppression != null)
            {
                // si type de maintenance = curative (panne) alors envoyer un mail à tout le monde
                maintenance maint = modifMaintDb.ObtenirMaintenanceByID(id);

                bool IsOK = modifMaintDb.SupprimerMaintenance(id, vm.RaisonSuppression);

                if (IsOK) // intervention supprimée
                {
                    if (maint.type_maintenance == "Maintenance curative (Panne)")
                    {
                        // envoyer mail aux utilisateurs PFL pour les informer de la suppression
                        // Envoyer le mail à tous les utilisateurs pour informer de cette nouvelle date
                        // si enregistrement OK alors envoyer le mail
                        MsgUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> L'équipe PFL vous informe que l'intervention du type  : <b> " + maint.type_maintenance + "</b>. Code d'intervention:<b> "
                                       + maint.code_operation + "</b>. Descriptif du problème: <b>" + maint.description_operation + " VIENT D'ETRE SUPPRIMEE.</b>"
                                       + "<p> Vous pouvez désormais reprogrammer vos essais. </p> <p>L'équipe PFL, " +
                                       "</p>" +
                                       "</body>" +
                                       "</html>";
                        // Obtenir la liste des utilisateurs
                        List<utilisateur> users = modifMaintDb.ObtenirListUtilisateursSite();

                        // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                        foreach (var usr in users)
                        {
                            success = false;
                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(usr.Email, "Clôture intervention dépannage materiel commun", MsgUser);
                                    success = true;
                                }
                                catch (Exception e)
                                {
                                    retryCount--;
                                    if (retryCount == 0)
                                    {
                                        ModelState.AddModelError("", "Problème de connexion pour l'envoie du mail: " + e.Message + ". ");
                                        return View("ModificationIntervention", Model);
                                    }
                                }
                            }
                        }
                    }

                    /*string code = modifMaintDb.ObtenirCodeIntervention(id);
                    Model.InfosMaint = modifMaintDb.ObtenirInfosMaint(code);
                    Model.OpenModifInter = vm.Ouvert;
                    Model.ListeEquipsPfl = modifMaintDb.ListIntervPFL(id);*/
                    return View("ConfirmationSupInterv");
                }
                else
                {
                    ModelState.AddModelError("RaisonSuppression", "Un problème a été recontré lors de la supression de l'intervention, réessayez ultériurement");
                    Model.OpenModifInter = vm.Ouvert;
                    ViewBag.modalSuppression = "show";
                    return View("ModificationIntervention", Model);
                }
            }
            else
            {
                ModelState.AddModelError("RaisonSuppression", "Vous devez saisir au moins une raison de supression");
                Model.OpenModifInter = Model.Ouvert;

                ViewBag.modalSuppression = "show";
                return View("ModificationIntervention", Model);
            }
        }
    }
}
