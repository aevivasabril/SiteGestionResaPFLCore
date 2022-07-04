using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.DonneesPGD.Data;
using SiteGestionResaCore.Areas.Enquete.Data.PostEnquete;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Services.ScheduleTask;
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
        private readonly IEmailSender emailSender;
        private readonly IEnqueteTaskDB enquete;
        private readonly IEssaisXEntrepotDB entrepotDB;

        public PostEnqueteController(
            IPostEnqueteDB postEnqueteDB,
            IEmailSender emailSender,
            IEnqueteTaskDB enquete,
            IEssaisXEntrepotDB entrepotDB)
        {
            this.postEnqueteDB = postEnqueteDB;
            this.emailSender = emailSender;
            this.enquete = enquete;
            this.entrepotDB = entrepotDB;
        }

        public IActionResult RecupNotesEnquete()
        {
            PostEnqueteViewModel vm = new PostEnqueteViewModel(){ };
            vm.ListEnquetesSansRp = postEnqueteDB.ObtEnquetesSansRp();
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
                        Rg = new Regex(pattMIQun);
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
                        // Rajouter oui ou non au lieu de true ou false
                        if (collect[0].Groups[1].Value == "true")
                        {
                            csv.Append("oui");
                            csv.Append(";");
                        }
                        else
                        {
                            csv.Append("non");
                            csv.Append(";");
                        }                        
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
                        // Rajouter oui ou non au lieu de true ou false
                        if (collect[0].Groups[1].Value == "true")
                        {
                            csv.Append("oui");
                            csv.Append(";");
                        }
                        else
                        {
                            csv.Append("non");
                            csv.Append(";");
                        }
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

        public async Task<IActionResult> ExecuterEnvoiEnqAsync()
        {
            List<enquete> ListEnquetesFirstTime = new List<enquete>();
            List<enquete> ListEnquetesXRelance = new List<enquete>();

            string message;

            #region Rajouter les enquetes pour les essais dont elle a pas été crée automatiquement (Environnement Prod) TODO: A effacer une fois la MAJ est faite en Prod

            bool isOK = enquete.AreEnquetesCreated();

            #endregion

            #region Reperer les enquetes dont l'envoi se fait pour la première fois

            ListEnquetesFirstTime = enquete.GetEnquetesXFirstTime();

            #region Envoi mail pour remplissage enquete

            foreach (var enque in ListEnquetesFirstTime)
            {
                var essai = enquete.GetEssaiParEnquete(enque.essaiId);
                var proj = enquete.GetProjetParEnquete(essai.projetID);
                var email = enquete.GetEmailCreatorEssai(essai.compte_userID);

                string callbackUrl = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                //string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>" + essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": " + proj.titre_projet + "). Pour répondre à cette enquete: " + "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

                await emailSender.SendEmailAsync(email, "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));

                // Mettre à jour la date-envoi_enquete
                enquete.UpdateDateEnvoiEnquete(enque);
            }

            #endregion
            // TODO: effacer!! c'est juste pour tester l'envoi des mails tous les 2 minutes
            await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "TEST tâche côté serveur", DateTime.Now.ToString());

            #endregion


            #region Relancer l'enquête de satisfaction si non répondu après 7 jours 

            ListEnquetesXRelance = enquete.GetEnquetesPourRelance();

            #region Envoi mail pour remplissage enquete

            foreach (var enque in ListEnquetesXRelance)
            {
                var essai = enquete.GetEssaiParEnquete(enque.essaiId);
                var proj = enquete.GetProjetParEnquete(essai.projetID);
                var email = enquete.GetEmailCreatorEssai(essai.compte_userID);

                string callbackUrl = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // lien pour le serveur caseine! 

                //string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Vous recevez cette enquête suite à votre essai : <b> </b>  N°" + essai.id + ": <strong>" + essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                            ": " + proj.titre_projet + "). Pour répondre à cette enquete: " + "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                                "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                            + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

                await emailSender.SendEmailAsync(email, "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
                // Mettre à jour la date-envoi_enquete
                enquete.UpdateDateEnvoiEnquete(enque);
            }

            #endregion

            #endregion

            ViewBag.Message = "Tâche d'envoie des enquêtes executé";
            ViewBag.AfficherMessage = true;
            PostEnqueteViewModel vm = new PostEnqueteViewModel() { };
            vm.ListEnquetesSansRp = postEnqueteDB.ObtEnquetesSansRp();
            return View("RecupNotesEnquete", vm);
        }

        public async Task<IActionResult> EnvoyerRelanceAsync(int id)
        {
            var essai = enquete.GetEssaiParEnquete(id);
            var proj = enquete.GetProjetParEnquete(essai.projetID);
            var email = enquete.GetEmailCreatorEssai(essai.compte_userID);
            var enq = postEnqueteDB.GetEnqueteXEssai(id);

            string callbackUrl = "http://147.99.161.143/SiteGestionResa/Enquete/Enquete/EnqueteSatisfaction?id=" + id; // lien pour le serveur caseine! 

            //string callbackUrl = "http://localhost:55092/Enquete/Enquete/EnqueteSatisfaction?id=" + essai.id; // Lien sur mon ordi (FONCTIONNE!!! :D )

            string message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Tu reçois ce rappel d'enquête car nous avons besoin de tes réponses avant la réunion utilisateurs : <b> </b>  N°" + essai.id 
                            + ": <strong>" + essai.titreEssai + "</strong> (Projet " + proj.num_projet +
                        ": " + proj.titre_projet + "). Pour répondre à l'enquête: " + "<a href='[CALLBACK_URL]'>Veuillez cliquer ici</a>.<br/> " +
                            "Cette enquête s'inscrit dans la démarche qualité de la PFL. <br>Merci par avance de prendre un court instant pour y répondre."
                        + "</p><p>Cordialement, </p><br><p>L'équipe PFL! </p> </body></html>";

            await emailSender.SendEmailAsync(email, "(RELANCE ++) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
            //await emailSender.SendEmailAsync("anny.vivas@inrae.fr", "(RELANCE) Enquête de satisfaction PFL", message.Replace("[CALLBACK_URL]", callbackUrl));
            // Mettre à jour la date-envoi_enquete et la date premier envoie
            enquete.UpdateDateEnvoiEnqueteManuel(enq);
            PostEnqueteViewModel vm = new PostEnqueteViewModel() { };
            vm.ListEnquetesSansRp = postEnqueteDB.ObtEnquetesSansRp();
            return View("RecupNotesEnquete", vm);
        }

        /// <summary>
        /// Action pour afficher les réservations d'un essai sur la liste
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirReservations(int id)
        {
            EquipementsReservesVM vm = new EquipementsReservesVM()
            {
                Reservations = entrepotDB.InfosReservations(id)
            };
            return PartialView("~/Views/Shared/_DisplayEquipsReserves.cshtml", vm);
        }
    }
}
