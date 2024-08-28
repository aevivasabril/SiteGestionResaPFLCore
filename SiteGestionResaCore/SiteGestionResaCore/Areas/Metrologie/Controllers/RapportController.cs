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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Metrologie.Data.Rapport;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
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
            model.EquipementExterneItem = listEquip;
            model.EquipementInterneItem = listEquip;

            this.HttpContext.AddToSession("SelectCapteurVM", model);
            return View("SelectionCapteur", model);
        }

        [HttpPost]
        public async Task<IActionResult> AjouterRapportInterneAsync(SelectCapteurVM model)
        {
            ModelState.Remove("SelectecEquipementExtId");
            if (ModelState.IsValid)
            {
                FormRapportVM vmun = new FormRapportVM();
                var capt = rapportDB.ObtenirCapteur(model.SelectecEquipementIntId);
                vmun.idCapteur = capt.id;
                vmun.nomCapteur = capt.nom_capteur;
                vmun.emtCapteur = capt.emt_capteur;
                vmun.TypeMetrologie = "Interne";
                vmun.nomEquipement = rapportDB.NomEquipementXCapteur(capt.equipementID);
                //vmun.facteurCorrectif = capt.facteur_correctif;
                // recuperer la liste des utilisateurs "admin"
                IList<utilisateur> listUsrs = await rapportDB.ObtenirAdminUsrs();
                vmun.OperateurItem = listUsrs.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });

                this.HttpContext.AddToSession("FormRapportVM", vmun);
                return View("FormRapportMetro", vmun);
            }
            else
            {
                SelectCapteurVM vm = HttpContext.GetFromSession<SelectCapteurVM>("SelectCapteurVM");
                return View("SelectionCapteur", vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AjouterRapportExterneAsync(SelectCapteurVM model)
        {
            ModelState.Remove("SelectecEquipementIntId");

            if (ModelState.IsValid)
            {
                FormRapportVM vmun = new FormRapportVM();
                var capt = rapportDB.ObtenirCapteur(model.SelectecEquipementExtId);
                vmun.idCapteur = capt.id;
                vmun.nomCapteur = capt.nom_capteur;
                vmun.emtCapteur = capt.emt_capteur;
                vmun.TypeMetrologie = "Externe";
                vmun.nomEquipement = rapportDB.NomEquipementXCapteur(capt.equipementID);
                //vmun.facteurCorrectif = capt.facteur_correctif;
                // recuperer la liste des utilisateurs "admin"
                IList<utilisateur> listUsrs = await rapportDB.ObtenirAdminUsrs();
                vmun.OperateurItem = listUsrs.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )" });

                this.HttpContext.AddToSession("FormRapportVM", vmun);
                return View("FormRapportMetro", vmun);
            }
            else
            {
                SelectCapteurVM vm = HttpContext.GetFromSession<SelectCapteurVM>("SelectCapteurVM");
                return View("SelectionCapteur", vm);
            }
        }

        public IActionResult AddDocMetro(FormRapportVM model)
        {
            // Récupérer la session "FormRapportVM"
            FormRapportVM vm = HttpContext.GetFromSession<FormRapportVM>("FormRapportVM");
            model.capteurConforme = vm.capteurConforme;
            model.DateVerifMetro = vm.DateVerifMetro;
            model.SelectecOperateurId = vm.SelectecOperateurId;
            model.Commentaire = vm.Commentaire;
            ViewBag.ModalRapport = "show";
            return View("FormRapportMetro", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ValidAddRapportAsync(IFormFile file, FormRapportVM model)
        {
            // Récupérer la session "FormRapportVM"
            FormRapportVM vm = HttpContext.GetFromSession<FormRapportVM>("FormRapportVM");

            vm.nomDocRapport = file.FileName.ToString();
            // Creates a new MemoryStream object , convert file to memory object and appends into our model’s object.
            using (var datastream = new MemoryStream())
            {
                await file.CopyToAsync(datastream);
                vm.contenuRapport = datastream.ToArray();
            }
            this.HttpContext.AddToSession("FormRapportVM", vm);
            return View("FormRapportMetro", vm);
        }

        public IActionResult ValiderRapport(FormRapportVM model)
        {
            // Récupérer la session "FormRapportVM"
            FormRapportVM vm = HttpContext.GetFromSession<FormRapportVM>("FormRapportVM");
            // recuperer la liste des operateurs
            vm.DateVerifMetro = model.DateVerifMetro;
            vm.capteurConforme = model.capteurConforme;
            vm.facteurCorrectif = model.facteurCorrectif;
            vm.SelectecOperateurId = model.SelectecOperateurId;
            vm.Commentaire = model.Commentaire;

            this.HttpContext.AddToSession("FormRapportVM", vm);           

            if (ModelState.IsValid)
            {
                if (model.contenuRapport == null && vm.contenuRapport == null)
                {
                    ModelState.AddModelError("", "Ajoutez le rapport métrologique");
                    return View("FormRapportMetro", vm);
                }
                if (model.capteurConforme == false && (model.facteurCorrectif == null || model.facteurCorrectif == 0) ) // demander le renseignement du facteur de correction
                {
                    ModelState.AddModelError("FacteurCorrectif", "Si le capteur est non conforme vous devez indiquer un facteur de correction");
                    return View("FormRapportMetro", vm);
                }
                // Enregistrer l'opération de métrologie
                if (rapportDB.CreerRapportMetrologie(vm.contenuRapport, vm.nomDocRapport, vm.idCapteur, vm.DateVerifMetro.Value, vm.TypeMetrologie, vm.Commentaire) == true)
                {
                    // maj du facteur correctif et les dates de vérif et date prochaine verification
                    bool isOk = rapportDB.majCapteurxRapport(vm.capteurConforme.Value, vm.facteurCorrectif.GetValueOrDefault(), vm.DateVerifMetro.Value, vm.idCapteur, vm.TypeMetrologie);
                  
                    if(isOk == true)
                    {
                        // Afficher message OK
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Rapport métrologie " + vm.TypeMetrologie + " ajouté!";
                        ViewBag.Color = "green";
                    }
                    else
                    {
                        // Afficher message d'erreur
                        ViewBag.AfficherMessage = true;
                        ViewBag.Message = "Problème lors de la mise à jour du capteur";
                        ViewBag.Color = "red";
                    }
                }
                else
                {
                    // Afficher message d'erreur
                    ViewBag.AfficherMessage = true;
                    ViewBag.Message = "Problème lors de l'ajout du rapport  " + vm.TypeMetrologie ;
                    ViewBag.Color = "red";
                }
            }

            return View("FormRapportMetro", vm);
        }

        public IActionResult ListRapports()
        {
            RappXDownloadVM model = new RappXDownloadVM();
            model.ListCapteursXrapports = rapportDB.ListRapports();
            return View("ListRapportXDownload", model);
        }

 
        public IActionResult TelechargementRapport(int? id)
        {
            rapport_metrologie rapport = rapportDB.ObtenirRapport(id.Value);
            return File(rapport.contenu_rapport, System.Net.Mime.MediaTypeNames.Application.Octet, rapport.nom_document);
        }
    }
}
