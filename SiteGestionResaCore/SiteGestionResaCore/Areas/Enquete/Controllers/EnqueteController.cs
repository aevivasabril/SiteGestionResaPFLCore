using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Enquete.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Controllers
{
    [Area("Enquete")]
    public class EnqueteController : Controller
    {
        private readonly IEnqueteDb EnqueteDb;
        private readonly IEmailSender emailSender;

        public EnqueteController(
            IEnqueteDb EnqueteDb,
            IEmailSender emailSender)
        {
            this.EnqueteDb = EnqueteDb;
            this.emailSender = emailSender;
        }

        /// <summary>
        /// Methode GET pour recharger le model view pour affichage de l'enquete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EnqueteSatisfaction(int id)
        {
            //TODO: effacer j'essai de forcer le titre essai pour éviter les erreurs
            //int idx = 13;
            essai ess = EnqueteDb.ObtenirEssai(id);
            projet pr = EnqueteDb.ObtenirProjet(ess);
            enquete enquete = EnqueteDb.ObtenirEnqueteFromEssai(id);

            if (enquete.reponduEnquete == null || enquete.reponduEnquete == false)
            {
                EnqueteViewModel vm = new EnqueteViewModel
                {
                    TitreEssai = ess.titreEssai,
                    IdEssai = ess.id,
                    NumProjet = pr.num_projet,
                    TitreProj = pr.titre_projet
                };
                return View(vm);
            }
            else
            {
                return View("EnqueteDejaRepondue");
            }
        }

        /// <summary>
        /// Méthode POST suite à validation de l'enquete de satisfaction
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnvoyerEnquete(EnqueteViewModel vm, int id)
        {
            // requete pour obtenir juste le commentaire pour la rubrique 1: -1(.*?)1         1113-1hola la cosa esta aqui1-2223-
            // REGEX pour obtenir les contenus dans les balises: <1Mcomm>(.*?)<\/1Mcomm>

            // Example:  EXAMPLE: < 1M1 > 1 </ 1M1 >< 1M2 > 1 </ 1M2 >< 1Mcomm > hola la cosa esta aqui</ 1Mcomm >< 2M1 > 2 </ 2M1 >
            // < 2M2 > 2 </ 2M2 >< 2M3 > 3 </ 2M3 >< 2Mcomm > problème pour utiliser la cuve</2Mcomm >< 3M1 > 4 </ 3M1 >< 3M2 > 4 </ 3M2 >
            // < 3M3 > 4 </ 3M3 >< 3M3bit > 1 </ 3M3bit >< 3M4 > 3 </ 3M4 >< 3Mcomm > Non concerné par toutes ces rubriques </ 3Mcomm >
            // < 4M1 > 4 </ 4M1 >< 4M2 > 2 </ 4M2 >< 4M3 > 3 </ 4M3 >< 4Mcomm > espace de travail agreable </ 4Mcomm >< 5M1 > 1 </ 5M1 >
            // < 5M2 > 2 </ 5M2 >< 5M3 > 3 </ 5M3 >< 5Mcomm > Commentaire </ 5mcomm >
            // Format pour sauvegarder les réponses aux enquêtes: N°Rubrique

            string ReponseBalise;
            if (ModelState.IsValid) // Ajouter le créneau de réservation dans la liste
            {
                ReponseBalise = "<1M1>" + vm.LogicielMeth + "</1M1><1M2>" + vm.AccessibiliteMeth + "</1M2><1Mcomm>" + vm.CommentMéthodes +
                "</1Mcomm><2M1>" + vm.DisponibiliteMat + "</2M1><2M2>" + vm.OperationnabiliteMat + "</2M2><2M3>" + vm.MaintenanceMat + "</2M3><2Mcomm>" + vm.CommentMateriels +
                "</2Mcomm><3M1>" + vm.DisponibiliteMain + "</3M1><3M2>" + vm.CompetenceMain + "</3M2 ><3M3>" + vm.FormationMain + "</3M3><3M3bit>" + vm.IsNotConcerneFormationMain + "</3M3bit><3M4>" + vm.ReactiviteMain + "</3M4><3Mcomm>" + vm.CommentaireMain +
                "</3Mcomm><4M1>" + vm.HygieneMil + "</4M1><4M2>" + vm.ConfidentMil + "</4M2><4M3>" + vm.EspaceMil + "</4M3><4Mcomm>" + vm.CommentaireMil +
                "</4Mcomm><5M1>" + vm.ApprovisioMatiere + "</5M1><5M2>" + vm.EnergieMatiere + "</5M2><5M3>" + vm.MaterielMatiere + "</5M3><5Mcomm>" + vm.CommentaireMatiere + "</5Mcomm>";

                enquete enquete = EnqueteDb.ObtenirEnqueteFromEssai(id);
                // Enregistrer les résultats de l'enquete
                EnqueteDb.UpdateEnqueteWithResponse(ReponseBalise, enquete);

                // envoyer vers la vue confirmation prise en compte enquête
                return View("ConfirmationEnquete");
            }
            else
            {
                // Recharger mon model view 
                // Obtenir l'essai à partir de l'ID enquete
                essai ess = EnqueteDb.ObtenirEssai(id);
                projet pr = EnqueteDb.ObtenirProjet(ess);

                vm.TitreEssai = ess.titreEssai;
                vm.IdEssai = ess.id;
                vm.NumProjet = pr.num_projet;
                vm.TitreProj = pr.titre_projet;

                // Renvoyer vers la vue enquete pour remplir l'enquete
                return View("EnqueteSatisfaction", vm);
                //return RedirectToAction("EnqueteSatisfaction", "Enquete", new { area = "Enquete", id= 5 });
            }
        }
    }
}
