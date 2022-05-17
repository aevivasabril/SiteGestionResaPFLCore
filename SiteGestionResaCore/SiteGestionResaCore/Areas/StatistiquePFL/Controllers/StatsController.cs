using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.StatistiquePFL.Data;

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

        public IActionResult AccueilStats()
        {
            AccueilStatsVM vm = new AccueilStatsVM();
            return View("AccueilStats", vm);
        }

        [HttpPost]
        public IActionResult GenererExcelResas(AccueilStatsVM vm)
        {
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
                                        headersCsv.TitreEssai, headersCsv.IdEssai, headersCsv.NomEquipe, headersCsv.DateCreation, headersCsv.NomEquipement,
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
                        csv.Append(resa.DateDebutResa);
                        csv.Append(";");
                        csv.Append(resa.DateFinResa);
                        csv.Append(";");
                        csv.Append(resa.NbJours);
                        csv.Append(";");
                        csv.AppendLine();
                    }
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

            return View("AccueilStats", vm);
        }
    }
}
