using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Areas.Maintenance.Data.Modification;
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

        public ModifMaintController(
        IModifMaintenanceDB modifMaintDb,
            IEmailSender emailSender)
        {
            this.emailSender = emailSender;
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
                ModelState.AddModelError("NumMaintenance", "Le numéro d'intervention n'existe pas!");
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

        [HttpPost]
        public async Task<IActionResult> ModifierInterCommunAsync(ModifMaintenanceVM vm, int id)
        {
            DateTime NewDate = new DateTime();
            bool success = false;
            var retryCount = 5;
            string MsgUser = "";

            // Obtenir les infos maintenance
            maintenance maint = modifMaintDb.ObtenirMaintenance(id);

            // Si la personne n'a pas choisi de date ou créneau
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Sélectionnez une date et un créneau");
                // Récupérer la session VM
                ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
                Model.IdIntervCom = id;
                Model.OpenModifInter = vm.Ouvert;
                Model.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id);

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
                // Mettre à jour la date pour l'intervention des équipements communs
                DateTime AncienneDateFin = modifMaintDb.ChangeDateFinEquipCommun(id, NewDate);
                
                resa_maint_equip_adjacent resaCommun = modifMaintDb.ObtenirIntervEquiComm(id);
                // Envoyer le mail à tous les utilisateurs pour informer de cette nouvelle date
                // si enregistrement OK alors envoyer le mail
                MsgUser = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> L'équipe PFL vous informe que la date fin pour l'intervention du type  : <b> " + maint.type_maintenance + "</b>. Code d'intervention:<b> "
                               + maint.code_operation + "</b>. Descriptif du problème: <b>" + maint.description_operation + "</b> a été mise à jour."
                                + "<p>Ancienne date: <b> " + resaCommun.date_debut + " au  " + AncienneDateFin +" </b></p><p> Nouvelle date: <b>" + resaCommun.date_debut + " au "+
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

                // Récupérer la session VM
                ModifMaintenanceVM Model = HttpContext.GetFromSession<ModifMaintenanceVM>("ModifMaintVM");
                Model.IdIntervCom = id;
                Model.OpenModifInter = vm.Ouvert;
                Model.ListEquipsCommuns = modifMaintDb.ListMaintAdj(maint.id);

                ViewBag.modalModif = "";
                return View("ModificationIntervention", Model);
            }
            
        }
    }
}
