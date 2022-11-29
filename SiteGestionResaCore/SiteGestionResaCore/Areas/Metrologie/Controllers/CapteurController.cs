using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Metrologie.Data.Capteur;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Controllers
{
    [Area("Metrologie")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class CapteurController : Controller
    {
        private readonly ICapteurDB capteurDB;

        public CapteurController(
            ICapteurDB capteurDB)
        {
            this.capteurDB = capteurDB; 
        }

        public IActionResult OperationCapteurs()
        {
            OperationCapteurVM vm = new OperationCapteurVM();
            vm.ListCapteurs = capteurDB.ObtenirListCapteurs();
            return View("OperationsCapteur", vm);
        }

        public IActionResult AjouterCapteur()
        {
            AjouterCapteurVM vm = new AjouterCapteurVM();
            IList<equipement> list = capteurDB.ObtListEquipements();
            // Créer la liste des équipements à afficher pour sélection
            var ListEquipItem = list.Select(f => new SelectListItem { Value = f.id.ToString(), Text = "(" + f.numGmao +") " + f.nom});
            var listPeriodicite = new List<SelectListItem>();

            listPeriodicite.Add(new SelectListItem() { Value = "0", Text = "- Sélectionnez la periodicité-" });
            listPeriodicite.Add(new SelectListItem() { Value = "1", Text = "6 mois" });
            listPeriodicite.Add(new SelectListItem() { Value = "2", Text = "1 an" });
            listPeriodicite.Add(new SelectListItem() { Value = "3", Text = "2 ans" });

            vm.PeriodiciteItem = listPeriodicite;
            vm.PiloteItem = ListEquipItem;
            this.HttpContext.AddToSession("AjouterCapteurVM", vm);

            return View("AjouterCapteur", vm);
        }

        public IActionResult ValiderAjoutCapteur(AjouterCapteurVM vm)
        {
            double period = 0.0;
            AjouterCapteurVM model = HttpContext.GetFromSession<AjouterCapteurVM>("AjouterCapteurVM");
            ModelState.Remove("PiloteItem");
            ModelState.Remove("PeriodiciteItem");
            if (ModelState.IsValid)
            {
                if(vm.DateProchaineVerif < DateTime.Now)
                {
                    ModelState.AddModelError("DateProchaineVerif", "La date de la prochaine vérification ne peut pas être inferieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if(vm.DateDernierVerif > DateTime.Now)
                {
                    ModelState.AddModelError("DateDernierVerif", "La date de la dernière vérification métrologie ne peut pas être superieur à la date d'aujourd'hui");
                    goto ERR;
                }
                if(vm.CapteurConforme == false && vm.FacteurCorrectif == null) // demander le renseignement du facteur de correction
                {
                    ModelState.AddModelError("FacteurCorrectif", "Si le capteur est non conforme vous devez indiquer un facteur de correction");
                    goto ERR;
                }
                // Ajouter l'information sur le capteur
                // Calculer la periode métrologie
                if (vm.SelectPeriodID == 1)
                    period = 0.5;
                else if (vm.SelectPeriodID == 2)
                    period = 1;
                else
                    period = 2;
                bool IsOk = capteurDB.AjouterCapteur(vm);
            }
            else
            {
                goto ERR;
            }
           
        ERR: 
            return View("AjouterCapteur", model);                       
        }
    }
}
