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
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            this.HttpContext.AddToSession("AccueilStatsVM", vm);
            return View("AccueilStats", vm);
        }

        [HttpPost]
        public IActionResult GenererExcelResas(AccueilStatsVM vm)
        {
            try
            {
                //AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
                StringBuilder csv = new StringBuilder();
                string titreCsv = null;
                HeadersCsvResas headersCsv = new HeadersCsvResas();
                ModelState.Remove("SelectEquipeId");
                ModelState.Remove("SelectOrgId");
                ModelState.Remove("SelectProjetId");
                ModelState.Remove("DateDuEquip");
                ModelState.Remove("DateAuEquip");
                ModelState.Remove("DateDuMaintenance");
                ModelState.Remove("DateAuMaintenance");
                if (ModelState.IsValid)
                {
                    if (vm.DateDu.Value < vm.DateAu.Value)
                    {
                        List<InfosReservations> List = statistiquesDB.ObtenirResasDuAu(vm.DateDu.Value, vm.DateAu.Value);
                        if (List.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des reservations valides pour ces dates
                        {
                            ModelState.AddModelError("", "Il n'y a pas des réservations valides pour les dates saisies");
                            return View("AccueilStats", vm);
                        }
                        else
                        {
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

                            foreach (var resa in List)
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

                            vm.DateAu = vm.DateAu;
                            vm.DateDu = vm.DateDu;

                            titreCsv = "Reservations_" + vm.DateDu.Value.ToShortDateString() + "_Au_" + vm.DateAu.Value.ToShortDateString() + ".csv";
                            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                            this.HttpContext.AddToSession("AccueilStatsVM", vm);
                            return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                        return View("AccueilStats", vm);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Oups! Vous avez oublié de saisir les dates! ");
                    return View("AccueilStats", vm);
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "PROBLEME: " + e.ToString());
                return View("AccueilStats", vm);
            }
            
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
            vm.ListeEquipVsJours = statistiquesDB.ObtListEquipsVsJours(id, DateTime.Today.Year);
            vm.NomZone = statistiquesDB.ObtNomZone(id).nom_zone;
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
            //AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            HeadersCsvResas headersCsv = new HeadersCsvResas();
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            ModelState.Remove("SelectEquipeId");
            ModelState.Remove("SelectOrgId");
            ModelState.Remove("DateDuEquip");
            ModelState.Remove("DateAuEquip");
            ModelState.Remove("DateDuMaintenance");
            ModelState.Remove("DateAuMaintenance");

            if (vm.SelectProjetId >= 0)
            {
                List<InfosReservations> list = statistiquesDB.ObtRecapitulatifXProjet(vm.SelectProjetId);
                if(list.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des essais valides pour ce projet
                {
                    //AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
                    ModelState.AddModelError("SelectProjetId", "Ce projet ne contient pas des essais valides!");
                    ViewBag.ModalUseProjet = "show";
                    return View("AccueilStats", vm);
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
                ModelState.AddModelError("", "Veuillez choisir un projet!");
                ViewBag.ModalUseProjet = "show";
                return View("AccueilStats", vm);
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
            ModelState.Remove("DateDuMaintenance");
            ModelState.Remove("DateAuMaintenance");
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
            ModelState.Remove("DateDuMaintenance");
            ModelState.Remove("DateAuMaintenance");
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
            
            CategXProjVM vm = new CategXProjVM();
            vm.ListProjetsXCat = statistiquesDB.ListProjXProvenance(id);
            vm.ProvenanceProjet = statistiquesDB.NomProvenance(id).nom_provenance;
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListeProjXProvenanc", vm);
        }

        public IActionResult ProvenanceXProjetSans()
        {

            CategXProjVM vm = new CategXProjVM();
            vm.ListProjetsXCat = statistiquesDB.ListProjXNonProv();
            vm.ProvenanceProjet = "Sans provenance";
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListeProjXProvenanc", vm);
        }

        public IActionResult ConsultStats()
        {
            ConsultStatsVM model = new ConsultStatsVM();
            model.ListZones = statistiquesDB.ObtenirListZones();
            model.QuantiteLaitAnnee = statistiquesDB.LaitAnneeEnCours();
            model.ListProvenances = statistiquesDB.ListeProvenances();
            model.ListTypeProj = statistiquesDB.ListTypeProjet();
            model.ListProdIn = statistiquesDB.ListProdsEntree();
            model.AnneeActuel = DateTime.Today.Year;
            this.HttpContext.AddToSession("ConsultStatsVM", model);
            return View("ConsultStats", model);
        }

        [HttpPost]
        public IActionResult CalculLtsLait(ConsultStatsVM vm)
        {
            ConsultStatsVM model = HttpContext.GetFromSession<ConsultStatsVM>("ConsultStatsVM");
            model.QuantiteLaitPeriode = statistiquesDB.LaitXDates(vm.DateDu.Value, vm.DateAu.Value);
            return View("ConsultStats", model);
        }

        public IActionResult RecapMaintenance(AccueilStatsVM vm)
        {
            AccueilStatsVM model = HttpContext.GetFromSession<AccueilStatsVM>("AccueilStatsVM");
            HeadersCsvMaintenances headersCsv = new HeadersCsvMaintenances();
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            ModelState.Remove("SelectProjetId");
            ModelState.Remove("SelectEquipeId");
            ModelState.Remove("SelectOrgId");
            ModelState.Remove("DateDuEquip");
            ModelState.Remove("DateAuEquip");
            if (ModelState.IsValid)
            {
                if (vm.DateAuMaintenance < vm.DateDuMaintenance)
                {
                    ModelState.AddModelError("", "La date fin doit être superieure à la date debut");
                    return View("AccueilStats", model);
                }
                else
                {
                    List<MaintenanceInfos> list = statistiquesDB.ListMaintenances(vm.DateDuMaintenance.Value, vm.DateAuMaintenance.Value);

                    if (list.Count() == 0) // Si la liste est vide pas besoin de télécharger excel, il n'y a pas des essais valides pour cet organisme
                    {
                        ModelState.AddModelError("", "Il n'y a pas des opérations de maintenance pour les dates saisies");
                        return View("AccueilStats", model);
                    }
                    else
                    {
                        // Déterminer les headers tableau
                        var headers = new string[] { headersCsv.CodeMaint, headersCsv.TypeMainte, headersCsv.MailOper, headersCsv.IntervExt, headersCsv.NomIntervExt, headersCsv.DescripOpe,
                                        headersCsv.MaintSupp, headersCsv.DateSupp, headersCsv.RaisonSupp, headersCsv.MaintTermin, headersCsv.NomEquip, headersCsv.DateDebut,
                                        headersCsv.DateFin, headersCsv.ZoneAffecte};

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
                            csv.Append(resa.CodeMaintenance);
                            csv.Append(";");
                            csv.Append(resa.TypeMaintenance);
                            csv.Append(";");
                            csv.Append(resa.MailOperateur);
                            csv.Append(";");
                            csv.Append(resa.IntervenantExt);
                            csv.Append(";");
                            csv.Append(resa.NomIntervExt);
                            csv.Append(";");
                            csv.Append(resa.DescripOperation);
                            csv.Append(";");
                            csv.Append(resa.MaintSupprimee);
                            csv.Append(";");
                            csv.Append(resa.DateSuppression);
                            csv.Append(";");
                            csv.Append(resa.RaisonSuppression);
                            csv.Append(";");
                            csv.Append(resa.MaintTerminee);
                            csv.Append(";");
                            csv.Append(resa.NomEquipement);
                            csv.Append(";");
                            csv.Append(resa.DateDebut);
                            csv.Append(";");
                            csv.Append(resa.DateFin);
                            csv.Append(";");
                            csv.Append(resa.ZoneAffectee);
                            csv.Append(";");

                            csv.AppendLine();
                            #endregion
                        }

                        titreCsv = "Maintenances_Du-" + vm.DateDuMaintenance.Value.ToShortDateString() + "-Au-" + vm.DateAuMaintenance.Value.ToShortDateString() + ".csv";
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

        public IActionResult TypeXProjet(int id)
        {
            CategXProjVM vm = new CategXProjVM();
            vm.ListProjetsXCat = statistiquesDB.ListProjetXType(id);
            vm.ProvenanceProjet = statistiquesDB.NomTypeProj(id).nom_type_projet;
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListProjetXType", vm);
        }

        public IActionResult TypeXProjetSans()
        {
            CategXProjVM vm = new CategXProjVM();
            vm.ListProjetsXCat = statistiquesDB.ListProjsSansType();
            vm.ProvenanceProjet = "Non défini";
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListProjetXType", vm);
        }

        public IActionResult TypeProdXEssai(int id)
        {

            EssaisXTypeProdVM vm = new EssaisXTypeProdVM();
            vm.ListEssais = statistiquesDB.ListEssaisXprod(id);
            vm.NomProduit = statistiquesDB.NomTypeProd(id);
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListEssaiXProd", vm);
        }

        public IActionResult SansProdXEssai()
        {
            EssaisXTypeProdVM vm = new EssaisXTypeProdVM();
            vm.ListEssais = statistiquesDB.ListEssaisSansprod();
            vm.NomProduit = "Non défini";
            // Sauvegarder la session data du formulaire projet pour le traiter après (cette partie fonctionne)
            return PartialView("_ListEssaiXProd", vm);
        }
    }
}
