using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
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
    }
}
