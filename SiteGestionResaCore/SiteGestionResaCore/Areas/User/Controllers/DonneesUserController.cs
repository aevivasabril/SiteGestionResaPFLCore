using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.User.Data.DataPcVue;
using SiteGestionResaCore.Areas.User.Data.DonneesUser;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class DonneesUserController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IDonneesUsrDB donneesUsrDB;

        public DonneesUserController(
            UserManager<utilisateur> userManager,
            IDonneesUsrDB donneesUsrDB)
        {
            this.userManager = userManager;
            this.donneesUsrDB = donneesUsrDB;
        }

        public async Task<IActionResult> ListEssaisDonneesAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            ListResasDonneesVM vm = new ListResasDonneesVM()
            {
                ResasUser = donneesUsrDB.ObtenirResasUser(user.Id),
                EquipVsDonnees = new EquipVsDonneesVM(),
                ConsultInfosEssai = new ConsultInfosEssaiChildVM()
            };
            return View(vm);
        }

        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM vm = donneesUsrDB.ObtenirInfosEssai(id);
            //vm.ActionName = "ListEssaisDonnees";
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", vm);
        }
        

        public IActionResult ListEquipVsDonnees(int id) 
        {
            // id essai
            EquipVsDonneesVM vm = new EquipVsDonneesVM();
            List<InfosResasEquipement> ListResa = donneesUsrDB.ListEquipVsDonnees(id);
            vm.EquipementsReserves = ListResa;
            vm.IdEssai = id;
            return PartialView("_EquipVsDonnees", vm);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id réservation</param>
        /// <returns></returns>
        public IActionResult ObtenirDonnees(int id)
        {
            AllDataPcVue Donnees = donneesUsrDB.ObtenirDonneesPcVue(id);
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;

            #region  Créer un excel avec les données

            // Déterminer les headers tableau
            var headers = Donnees.DataEquipement.Select(d => d.NomCapteur).Distinct().ToList();
            // Ajouter la colonne de date 
            csv.Append("Date et heure");
            
            foreach (var dc in headers)
            {
                csv.Append(";");
                csv.Append(dc);
            }
            csv.AppendLine();

            // Reagrouper les données par date pour identifier chaque future ligne tableau
            var reg = Donnees.DataEquipement.GroupBy(d => d.Chrono);
            foreach (var group in reg)
            {
                var x = group.Key;
                int u = group.Count();
                csv.Append(group.Key.ToString());
                //csv.Append(";");
                foreach (var r in group)
                {
                    csv.Append(";");
                    csv.Append(r.Value);     
                }
                csv.AppendLine();
            }

            titreCsv = "DonneesProjet_" + Donnees.NomEquipement + ".csv";

            return File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", titreCsv);


            #endregion

            //return View();
        }
    }
}
