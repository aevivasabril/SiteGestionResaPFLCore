using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.StatistiquePFL.Data;
using SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats;
using SiteGestionResaCore.Extensions;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Controllers
{
    [Area("StatistiquePFL")]
    public class StatsController : Controller
    {
        private readonly IStatistiquesDB statistiquesDB;

        public StatsController(
            IStatistiquesDB statistiquesDB)
        {
            this.statistiquesDB = statistiquesDB;
        }

        public IActionResult MenuStats()
        {
            return View("MenuStats");
        }

        public IActionResult AccueilStats()
        {
            AccueilStatsVM vm = new AccueilStatsVM();
            //vm.ListZones = statistiquesDB.ObtenirListZones();
            vm.AnneeActuel = DateTime.Today.Year;
            vm.ListProvenances = statistiquesDB.ListeProvenances();
            //vm.QuantiteLaitAnnee = statistiquesDB.LaitAnneeEnCours();
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            this.HttpContext.AddToSession("AccueilStatsVM", vm);
            return View("AccueilStats", vm);
        }

        [HttpPost]
        public IActionResult GenererExcelResas(AccueilStatsVM vm)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            HeadersCsvResas headersCsv = new HeadersCsvResas();

            if (vm.DateAu != null && vm.DateDu != null) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (vm.DateDu.Value <= vm.DateAu.Value)
                {
                    List<InfosReservations> List = statistiquesDB.ObtenirResasDuAu(vm.DateDu.Value, vm.DateAu.Value);
                    // Déterminer les headers tableau
                    var headers = new string[] { headersCsv.NumProjet, headersCsv.TitreProjet, headersCsv.RespProjet, headersCsv.TypeProjet, headersCsv.NomOrganisme, 
                                        headersCsv.TitreEssai, headersCsv.IdEssai, headersCsv.NomEquipe, headersCsv.DateCreation, headersCsv.NomEquipement, headersCsv.ZonEquipement,
                                        headersCsv.DateDebutResa, headersCsv.DateFinResa, headersCsv.NbJours};

                    foreach (var col in headers)
                    {
                        csv.Append(col);
                        csv.Append(";");
                    }
                    csv.AppendLine();
                    csv.AppendLine();

                    foreach(var resa in List)
                    {
                        #region Ecriture dans le fichier excel
                        csv.Append(resa.NumProjet);
                        csv.Append(";");
                        csv.Append(resa.TitreProjet);
                        csv.Append(";");
                        csv.Append(resa.RespProjet);
                        csv.Append(";");
                        csv.Append(resa.TypeProjet);
                        csv.Append(";");
                        csv.Append(resa.NomOrganisme);
                        csv.Append(";");
                        csv.Append(resa.TitreEssai);
                        csv.Append(";");
                        csv.Append(resa.IdEssai);
                        csv.Append(";");
                        csv.Append(resa.NomEquipe);
                        csv.Append(";");
                        csv.Append(resa.DateCreation);
                        csv.Append(";");
                        csv.Append(resa.NomEquipement);
                        csv.Append(";");
                        csv.Append(resa.ZoneEquipement);
                        csv.Append(";");
                        csv.Append(resa.DateDebutResa);
                        csv.Append(";");
                        csv.Append(resa.DateFinResa);
                        csv.Append(";");
                        csv.Append(resa.NbJours);
                        csv.Append(";");
                        csv.AppendLine();
                        #endregion
                    }

                    model.DateAu = vm.DateAu;
                    model.DateDu = vm.DateDu;

                    titreCsv = "Reservations_" + vm.DateDu.Value.ToShortDateString() + "_Au_" + vm.DateAu.Value.ToShortDateString() + ".csv";
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);
                }
                else
                {
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                }
            }
            else
            {
                ModelState.AddModelError("", "Oups! Vous avez oublié de saisir les dates! ");
            }

            return View("AccueilStats", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id zone</param>
        /// <returns></returns>
        public IActionResult EquipVsDays(int id)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");

            EquipsVsJoursVM vm = new EquipsVsJoursVM();
            vm.ListeEquipVsJours = statistiquesDB.ObtListEquipsVsJours(id, model.AnneeActuel);
            vm.NomZone = statistiquesDB.ObtNomZone(id).nom_zone;
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            //this.HttpContext.AddToSession("AccueilStatsVM", vm);
            return PartialView("_EquipsVsJours", vm);
        }

        public IActionResult SelectProjet()
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            ViewBag.ModalUseProjet = "show";
            model.ProjetItem = statistiquesDB.ObtenirListProjet().Select(f => new SelectListItem { Value = f.id.ToString(), Text = "Projet N°" + f.num_projet + ": " + f.titre_projet });
            this.HttpContext.AddToSession("AccueilStatsVM", model);
            return View("AccueilStats", model);
        }

        [HttpPost]
        public IActionResult RecapXProjet(AccueilStatsVM vm)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            HeadersCsvResas headersCsv = new HeadersCsvResas();
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            ModelState.Remove("SelectEquipeId");
            ModelState.Remove("SelectOrgId");
            ModelState.Remove("DateDuEquip");
            ModelState.Remove("DateAuEquip");
            if (ModelState.IsValid)
            {
                List<InfosReservations> list = statistiquesDB.ObtRecapitulatifXProjet(vm.SelectProjetId);
                if(list.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des essais valides pour ce projet
                {
                    ModelState.AddModelError("SelectProjetId", "Ce projet ne contient pas des essais valides!");
                    ViewBag.ModalUseProjet = "show";
                    return View("AccueilStats", model);
                }
                else
                {
                    var projet = statistiquesDB.ObtenirProjet(vm.SelectProjetId);
                    // Déterminer les headers tableau
                    var headers = new string[] { headersCsv.NumProjet, headersCsv.TitreProjet, headersCsv.RespProjet, headersCsv.TypeProjet, headersCsv.NomOrganisme,
                                        headersCsv.TitreEssai, headersCsv.IdEssai, headersCsv.NomEquipe, headersCsv.DateCreation, headersCsv.NomEquipement, headersCsv.ZonEquipement,
                                        headersCsv.DateDebutResa, headersCsv.DateFinResa};

                    foreach (var col in headers)
                    {
                        csv.Append(col);
                        csv.Append(";");
                    }
                    csv.AppendLine();
                    csv.AppendLine();

                    foreach (var resa in list)
                    {
                        #region Ecriture dans le fichier excel
                        csv.Append(resa.NumProjet);
                        csv.Append(";");
                        csv.Append(resa.TitreProjet);
                        csv.Append(";");
                        csv.Append(resa.RespProjet);
                        csv.Append(";");
                        csv.Append(resa.TypeProjet);
                        csv.Append(";");
                        csv.Append(resa.NomOrganisme);
                        csv.Append(";");
                        csv.Append(resa.TitreEssai);
                        csv.Append(";");
                        csv.Append(resa.IdEssai);
                        csv.Append(";");
                        csv.Append(resa.NomEquipe);
                        csv.Append(";");
                        csv.Append(resa.DateCreation);
                        csv.Append(";");
                        csv.Append(resa.NomEquipement);
                        csv.Append(";");
                        csv.Append(resa.ZoneEquipement);
                        csv.Append(";");
                        csv.Append(resa.DateDebutResa);
                        csv.Append(";");
                        csv.Append(resa.DateFinResa);
                        csv.Append(";");

                        csv.AppendLine();
                        #endregion
                    }

                    titreCsv = "Reservations_projet-" + projet.num_projet + ".csv";
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);
                }   
            }
            else
            {
                ModelState.AddModelError("", "Veuillez choisir un projet");
                ViewBag.ModalUseProjet = "show";
                return View("AccueilStats", model);
            }          
        }

        public IActionResult SelectEquipe()
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            ViewBag.ModalUseEquipe = "show";
            model.EquipeStloItem = statistiquesDB.ObtenirListEquip().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_equipe });
            this.HttpContext.AddToSession("AccueilStatsVM", model);
            return View("AccueilStats", model);
        }

        [HttpPost]
        public IActionResult RecapXEquip(AccueilStatsVM vm)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            HeadersCsvResas headersCsv = new HeadersCsvResas();
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            ModelState.Remove("SelectProjetId");
            ModelState.Remove("SelectOrgId");
            if (ModelState.IsValid)
            {
                if (vm.DateAuEquip < vm.DateDuEquip)
                {
                    ViewBag.ModalUseEquipe = "show";
                    ModelState.AddModelError("", "La date fin doit être superieure à la date debut");
                    return View("AccueilStats", model);
                }
                else
                {
                    List<InfosReservations> list = statistiquesDB.ObtRecapitulatifXEquipe(vm.SelectEquipeId, vm.DateDuEquip.Value, vm.DateAuEquip.Value);
                    if (list.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des essais valides pour cet équipe
                    {
                        ModelState.AddModelError("SelectEquipeId", "Cet équipe ne contient pas des essais valides!");
                        ViewBag.ModalUseEquipe = "show";
                        return View("AccueilStats", model);
                    }
                    else
                    {
                        var equipeStlo = statistiquesDB.ObtInfosEquipe(vm.SelectEquipeId);
                        // Déterminer les headers tableau
                        var headers = new string[] { headersCsv.NumProjet, headersCsv.TitreProjet, headersCsv.RespProjet, headersCsv.TypeProjet, headersCsv.NomOrganisme,
                                        headersCsv.TitreEssai, headersCsv.IdEssai, headersCsv.NomEquipe, headersCsv.DateCreation, headersCsv.NomEquipement, headersCsv.ZonEquipement,
                                        headersCsv.DateDebutResa, headersCsv.DateFinResa};

                        foreach (var col in headers)
                        {
                            csv.Append(col);
                            csv.Append(";");
                        }
                        csv.AppendLine();
                        csv.AppendLine();

                        foreach (var resa in list)
                        {
                            #region Ecriture dans le fichier excel
                            csv.Append(resa.NumProjet);
                            csv.Append(";");
                            csv.Append(resa.TitreProjet);
                            csv.Append(";");
                            csv.Append(resa.RespProjet);
                            csv.Append(";");
                            csv.Append(resa.TypeProjet);
                            csv.Append(";");
                            csv.Append(resa.NomOrganisme);
                            csv.Append(";");
                            csv.Append(resa.TitreEssai);
                            csv.Append(";");
                            csv.Append(resa.IdEssai);
                            csv.Append(";");
                            csv.Append(resa.NomEquipe);
                            csv.Append(";");
                            csv.Append(resa.DateCreation);
                            csv.Append(";");
                            csv.Append(resa.NomEquipement);
                            csv.Append(";");
                            csv.Append(resa.ZoneEquipement);
                            csv.Append(";");
                            csv.Append(resa.DateDebutResa);
                            csv.Append(";");
                            csv.Append(resa.DateFinResa);
                            csv.Append(";");

                            csv.AppendLine();
                            #endregion
                        }

                        titreCsv = "Reservations_equipe-" + equipeStlo.nom_equipe + "_Du-" + vm.DateDuEquip.Value.ToShortDateString() + "-Au-" + vm.DateAuEquip.Value.ToShortDateString() + ".csv";
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);
                    }         
                }
            }
            else
            {
                ViewBag.ModalUseEquipe = "show";
                return View("AccueilStats", model);
            }
            
        }

        public IActionResult SelectOrganisme()
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            ViewBag.ModalUseOrg = "show";
            model.OrgItem = statistiquesDB.ListeOrganismes().Select(f => new SelectListItem { Value = f.id.ToString(), Text = f.nom_organisme });
            this.HttpContext.AddToSession("AccueilStatsVM", model);
            return View("AccueilStats", model);
        }

        [HttpPost]
        public IActionResult RecapXOrg(AccueilStatsVM vm)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            HeadersCsvResas headersCsv = new HeadersCsvResas();
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            ModelState.Remove("SelectProjetId");
            ModelState.Remove("SelectEquipeId");

            if (ModelState.IsValid)
            {
                if (vm.DateAuEquip < vm.DateDuEquip)
                {
                    ViewBag.ModalUseOrg = "show";
                    ModelState.AddModelError("", "La date fin doit être superieure à la date debut");
                    return View("AccueilStats", model);
                }
                else
                {
                    List<InfosReservations> list = statistiquesDB.ObtRecapitulatifXOrg(vm.SelectOrgId, vm.DateDuEquip.Value, vm.DateAuEquip.Value);

                    if (list.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des essais valides pour cet organisme
                    {
                        ModelState.AddModelError("SelectOrgId", "Cet organisme ne contient pas des essais valides!");
                        ViewBag.ModalUseOrg = "show";
                        return View("AccueilStats", model);
                    }
                    else
                    {
                        var organisme = statistiquesDB.ObtenirOrganisme(vm.SelectOrgId);
                        // Déterminer les headers tableau
                        var headers = new string[] { headersCsv.NumProjet, headersCsv.TitreProjet, headersCsv.RespProjet, headersCsv.TypeProjet, headersCsv.NomOrganisme,
                                        headersCsv.TitreEssai, headersCsv.IdEssai, headersCsv.NomEquipe, headersCsv.DateCreation, headersCsv.NomEquipement, headersCsv.ZonEquipement,
                                        headersCsv.DateDebutResa, headersCsv.DateFinResa};

                        foreach (var col in headers)
                        {
                            csv.Append(col);
                            csv.Append(";");
                        }
                        csv.AppendLine();
                        csv.AppendLine();

                        foreach (var resa in list)
                        {
                            #region Ecriture dans le fichier excel
                            csv.Append(resa.NumProjet);
                            csv.Append(";");
                            csv.Append(resa.TitreProjet);
                            csv.Append(";");
                            csv.Append(resa.RespProjet);
                            csv.Append(";");
                            csv.Append(resa.TypeProjet);
                            csv.Append(";");
                            csv.Append(resa.NomOrganisme);
                            csv.Append(";");
                            csv.Append(resa.TitreEssai);
                            csv.Append(";");
                            csv.Append(resa.IdEssai);
                            csv.Append(";");
                            csv.Append(resa.NomEquipe);
                            csv.Append(";");
                            csv.Append(resa.DateCreation);
                            csv.Append(";");
                            csv.Append(resa.NomEquipement);
                            csv.Append(";");
                            csv.Append(resa.ZoneEquipement);
                            csv.Append(";");
                            csv.Append(resa.DateDebutResa);
                            csv.Append(";");
                            csv.Append(resa.DateFinResa);
                            csv.Append(";");

                            csv.AppendLine();
                            #endregion
                        }

                        titreCsv = "Reservations_-" + organisme.nom_organisme + "_Du-" + vm.DateDuEquip.Value.ToShortDateString() + "-Au-" + vm.DateAuEquip.Value.ToShortDateString() + ".csv";
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);
                    }                    
                }
            }
            else
            {
                ViewBag.ModalUseOrg = "show";
                return View("AccueilStats", model);
            }

        }

        public IActionResult ProvenanceXProjet(int id)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");

            ProvXProjVM vm = new ProvXProjVM();
            vm.ListProjetsXProv = statistiquesDB.ListProjXProvenance(id);
            vm.ProvenanceProjet = statistiquesDB.NomProvenance(id).nom_provenance;
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            //this.HttpContext.AddToSession("AccueilStatsVM", vm);
            return PartialView("_ListeProjXProvenanc", vm);
        }

        public IActionResult ProvenanceXProjetSans()
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");

            ProvXProjVM vm = new ProvXProjVM();
            vm.ListProjetsXProv = statistiquesDB.ListProjXNonProv();
            vm.ProvenanceProjet = "Sans provenance";
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            //this.HttpContext.AddToSession("AccueilStatsVM", vm);
            return PartialView("_ListeProjXProvenanc", vm);
        }

        [HttpPost]
        public IActionResult CalculLtsLait(ConsultStatsVM vm)
        {
            ConsultStatsVM model = HttpContext.GetFromSession<ConsultStatsVM>("ConsultStatsVM");
            model.QuantiteLaitPeriode = statistiquesDB.LaitXDates(vm.DateDu.Value, vm.DateAu.Value);
            return View("AccueilStats", model);
        }

        public IActionResult ConsultStats()
        {
            ConsultStatsVM model = new ConsultStatsVM();
            model.ListZones = statistiquesDB.ObtenirListZones();
            model.QuantiteLaitAnnee = statistiquesDB.LaitAnneeEnCours();
            return View("ConsultStats", model);
        }
    }
}
