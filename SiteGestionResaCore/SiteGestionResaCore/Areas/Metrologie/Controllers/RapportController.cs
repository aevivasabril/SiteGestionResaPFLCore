using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Controllers
{
    [Area("Metrologie")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class RapportController : Controller
    {
        private readonly IRapportDB rapportDB;

        public RapportController(
            IRapportDB rapportDB)
        {
            this.rapportDB = rapportDB;
        }
        public IActionResult SelectionCapteur()
        {
            SelectCapteurVM model = new SelectCapteurVM();
            // Récuperer la liste des équipements qui ont au moins un capteur de déclaré
            IList<EquipVsCapteur> list = rapportDB.GetEquipements();

            var listEquip = list.Select(f => new SelectListItem { Value = f.capteurId.ToString(), Text = f.nomEquipement + "(N° GMAO: " + f.numGmao + ") - Capteur: " + f.nomCapteur });
            model.EquipementItem = listEquip;

            return View("SelectionCapteur", model);
        }

        [HttpPost]
        public IActionResult AjouterRapportInterne(SelectCapteurVM model)
        {

            return View();
        }

        [HttpPost]
        public IActionResult AjouterRapportExterne(SelectCapteurVM model)
        {
            return View();
        }
    }
}
