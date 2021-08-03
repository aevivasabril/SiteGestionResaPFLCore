using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Enquete.Data.PostEnquete;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Controllers
{
    [Area("Enquete")]
    public class PostEnqueteController : Controller
    {
        private readonly IPostEnqueteDB postEnqueteDB;

        public PostEnqueteController(
            IPostEnqueteDB postEnqueteDB)
        {
            this.postEnqueteDB = postEnqueteDB;
        }

        public IActionResult RecupNotesEnquete()
        {
            PostEnqueteViewModel vm = new PostEnqueteViewModel(){ };
            return View("RecupNotesEnquete", vm);
        }

        [HttpPost]
        public IActionResult RecupNotesEnquete(PostEnqueteViewModel vm)
        {
            StringBuilder csv = new StringBuilder();
            string titreCsv = null;
            HeadersNotesEnquetes headersCsv = new HeadersNotesEnquetes();

            #region Patterns pour les Expressions regulieres
            string pattMIQun = @"<1M1>(.*?)<\/1M1>";
            string pattMIQdeux = @"<1M2>(.*?)<\/1M2>";
            string pattMIComm = @"<1Mcomm>(.*?)<\/1Mcomm>";
            string pattMIIQun = @"<2M1>(.*?)<\/2M1>";
            string pattMIIQdeux = @"<2M2>(.*?)<\/2M2>";
            string pattMIIQtrois = @"<2M3>(.*?)<\/2M3>";
            string pattMIIQtroisConcerne = @"<2M3bit>(.*?)<\/2M3bit>";
            string pattMIIComm = @"<2Mcomm>(.*?)<\/2Mcomm>";
            string pattMIIIQun = @"<3M1>(.*?)<\/3M1>";
            string pattMIIIQdeux = @"<3M2>(.*?)<\/3M2>";
            string pattMIIIQtrois = @"<3M3>(.*?)<\/3M3>";
            string pattMIIIQquatre = @"<3M4>(.*?)<\/3M4>";
            string pattMIIIQcinq = @"<3M5>(.*?)<\/3M5>";
            string pattMIIIComm = @"<3Mcomm>(.*?)<\/3Mcomm>";
            string pattMIVQun = @"<4M1>(.*?)<\/4M1>";
            string pattMIVQunConcerne = @"<4M1bit>(.*?)<\/4M1bit>";
            string pattMIVQdeux = @"<4M2>(.*?)<\/4M2>";
            string pattMIVQtrois = @"<4M3>(.*?)<\/4M3>";
            string pattMIVComm = @"<4Mcomm>(.*?)<\/4Mcomm>";
            // Expressions regulieres
            Regex Rg = new Regex(pattMIQun);

            #endregion

            if (vm.DateAu != null && vm.DateDu != null) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (vm.DateDu.Value <= vm.DateAu.Value)
                {
                    #region  Créer un excel avec les données

                    // Déterminer les headers tableau
                    var headers = new string[] { headersCsv.Projet, headersCsv.TitreEssai, headersCsv.MailRespPro, headersCsv.DateFinManip, headersCsv.MIQun, headersCsv.MIQdeux, headersCsv.MIComm,
                                        headersCsv.MIIQun, headersCsv.MIIQdeux, headersCsv.MIIQtrois, headersCsv.MIIQtroisConcerne, headersCsv.MIIComm,
                                        headersCsv.MIIIQun, headersCsv.MIIIQdeux, headersCsv.MIIIQtrois, headersCsv.MIIIQquatre, headersCsv.MIIIQcinq, headersCsv.MIIIComm,
                                        headersCsv.MIVQun, headersCsv.MIVQunConcerne, headersCsv.MIVQdeux, headersCsv.MIVQtrois, headersCsv.MIVComm};

                    foreach (var col in headers)
                    {
                        csv.Append(col);
                        csv.Append(";");
                    }
                    csv.AppendLine();

                    List<enquete> enquetes = postEnqueteDB.GetReponsesEnquetes(vm.DateDu.Value, vm.DateAu.Value);

                    foreach(var enq in enquetes)
                    {
                        var essai = postEnqueteDB.GetEssai(enq.essaiId);
                        var proj = postEnqueteDB.GetProjet(essai.projetID);
                        csv.Append(proj.titre_projet + "(N°: "+ proj.num_projet + ")");
                        csv.Append(";");
                        csv.Append(essai.titreEssai);
                        csv.Append(";");
                        csv.Append(proj.mailRespProjet);
                        csv.Append(";");
                        csv.Append(enq.date_premier_envoi);
                        csv.Append(";");
                        // rubrique 1 question 1 
                        MatchCollection collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 1 question 2
                        Rg = new Regex(pattMIQdeux);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 1 commentaire
                        Rg = new Regex(pattMIComm);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 2 question1
                        Rg = new Regex(pattMIIQun);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 2 question 2
                        Rg = new Regex(pattMIIQdeux);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 2 question 3
                        Rg = new Regex(pattMIIQtrois);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 2 question 3 concerné
                        Rg = new Regex(pattMIIQtroisConcerne);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 2 commentaire
                        Rg = new Regex(pattMIIComm);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 question 1
                        Rg = new Regex(pattMIIIQun);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 question 2
                        Rg = new Regex(pattMIIIQdeux);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 question 3
                        Rg = new Regex(pattMIIIQtrois);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 question 4
                        Rg = new Regex(pattMIIIQquatre);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 question 5
                        Rg = new Regex(pattMIIIQcinq);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 3 commentaire
                        Rg = new Regex(pattMIIIComm);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 4 question 1
                        Rg = new Regex(pattMIVQun);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 4 question 1 concerné
                        Rg = new Regex(pattMIVQunConcerne);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 4 question 2
                        Rg = new Regex(pattMIVQdeux);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 4 question 3
                        Rg = new Regex(pattMIVQtrois);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");
                        // rubrique 4 commentaire
                        Rg = new Regex(pattMIVComm);
                        collect = Rg.Matches(enq.fichierReponse);
                        csv.Append(collect[0].Groups[1].Value);
                        csv.Append(";");

                        csv.AppendLine();
                    }

                    titreCsv = "EnquetesDeSatisfaction_" + vm.DateDu.Value.ToShortDateString() + "_Au_" + vm.DateAu.Value.ToShortDateString() +".csv";
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    return File(encoding.GetBytes(csv.ToString()), "text/csv", titreCsv);

                    #endregion

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

            return View("RecupNotesEnquete", vm);
        }
    }
}
