/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

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
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", vm);
        }
        

        public IActionResult ListEquipVsDonnees(int id) 
        {
            // id essai
            EquipVsDonneesVM vm = new EquipVsDonneesVM();
            List<InfosResasEquipement> ListResa = donneesUsrDB.ListEquipVsDonnees(id);
            vm.EquipementsReserves = ListResa;
            vm.TitreEssai = donneesUsrDB.ObtenirInfosEssai(id).TitreEssai;
            return PartialView("~/Views/Shared/_EquipVsDonnees.cshtml", vm);
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
            csv.Append("Date");
            csv.Append(";");
            csv.Append("Heure");
            
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
                csv.Append(group.Key.ToShortDateString());
                csv.Append(";");
                csv.Append(group.Key.ToLongTimeString());
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
        }
    }
}
