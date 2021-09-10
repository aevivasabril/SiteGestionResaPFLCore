using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
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

        public MaintenanceController(
            IFormulaireIntervDB intervDb)
        {
            this.intervDb = intervDb;
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
            return View("SaisirIntervention",vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoumettreInfosMaintAsync(MaintenanceViewModel vm)
        {
            if (vm.IntervenantExterne == "true") // Si l'utilisateur coche "oui" alors il est obligé de taper le nom de la sociète
                ModelState.AddModelError("NomSociete", "Veuillez indiquer le nom de la sociète intervenante");

            if(ModelState.IsValid)
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
                IList<utilisateur> logistMaint = await intervDb.List_utilisateurs_logistiqueAsync();
                // Obtenir la liste des valeurs à pre-remplir pour charger le formulaire
                var Intervenants = logistMaint.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });
                var ListMaints = intervDb.List_Type_Maintenances().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_type_maintenance });
                var code = intervDb.CodeOperation();
                vm = new MaintenanceViewModel
                {
                    IntervenantItem = Intervenants,
                    InterventionItem = ListMaints,
                    CodeMaintenance = code
                };
            return View("SaisirIntervention", vm);           
        }

        public IActionResult AjoutEquipements()
        {
            AjoutEquipementsViewModel vm = new AjoutEquipementsViewModel()
            {
                EquipementSansZoneVM = new EquipementSansZoneVM(),
                ListEquipsSansZone = new List<EquipementSansZoneVM>(),
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

            return View("AjoutEquipements", model);
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
            AjoutEquipementsViewModel vm = new AjoutEquipementsViewModel()
            {
                EquipementSansZoneVM = new EquipementSansZoneVM(),
                ListEquipsSansZone = new List<EquipementSansZoneVM>(),
                OuvrirEquipSansZone = "none"
            };
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
    }
}
