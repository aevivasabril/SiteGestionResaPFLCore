using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Services;

namespace SiteGestionResaCore.Areas.Reservation.Controllers
{
    [Area("Reservation")]
    [Authorize(Roles = "Admin, Logistic, MainAdmin")] // Il faut être ou Admin ou Logistic ou MainAdmin si on met authorize pour chaque rôle il faut être parti des 3 rôles pour accèder
    public class ValidationController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IEmailSender emailSender;
        private readonly IResaAValiderDb resaAValiderDb;

        public ValidationController(
            UserManager<utilisateur> userManager,
            IEmailSender emailSender,
            IResaAValiderDb resaAValiderDb)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.resaAValiderDb = resaAValiderDb;
        }
        /// <summary>
        /// Affichage page pour sélection des sous rubriques
        /// </summary>
        /// <returns></returns>
        public IActionResult MenuConsultation()
        {
            return View();
        }

        /// <summary>
        /// Action pour afficher la page "Réservations à valider"
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ReservationsAValiderAsync(ResasPourValidationViewModel ResaVm)
        {
            ResaVm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = new InfosProjet(),
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>()                               
            };
            
            return View(ResaVm);
        }

        /// <summary>
        /// Action pour charger les infos sur un essai de la liste 
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public IActionResult VoirInfosEssai(int id)
        {
            ConsultInfosEssaiChildVM InfosEssai = resaAValiderDb.ObtenirInfosEssai(id);
            return PartialView("~/Views/Shared/_DisplayInfosEssai.cshtml", InfosEssai);
        }

        /// <summary>
        /// Action pour charger les infos sur le projet d'un essai de la liste
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public IActionResult VoirInfosProjet(int id)
        {
            /*ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = resaAValiderDb.ObtenirInfosProjet(id),
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>()
            };
            ViewBag.modalProj = "show";*/
            InfosProjet vm = resaAValiderDb.ObtenirInfosProjet(id);
            return PartialView("~/Views/Shared/_DisplayInfosProjet.cshtml", vm);
        }

        /// <summary>
        /// Action pour charges les infos sur les réservations d'un essai
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConsulterResasProjetAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = new InfosProjet(),
                Reservations = resaAValiderDb.InfosReservations(id),
                InfosConflits = new List<InfosConflit>()
            };
            ViewBag.modalResas = "show";
            return View("ReservationsAValider", vm);
        }

        /// <summary>
        /// Action pour charger les autres réservations en conflit avec un essai
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public async Task<IActionResult> VoirConflitEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = new InfosProjet(),
                Reservations = new List<InfosReservation>(),
                InfosConflits = resaAValiderDb.InfosConflits(id),
                IdEss = id,
                TitreEssaiPrincipal = resaAValiderDb.ObtenirEssai(id).titreEssai
            };
            ViewBag.modalConflit = "show";
            return View("ReservationsAValider", vm);
        }

        /// <summary>
        /// Action pour charger les infos sur l'essai à valider
        /// </summary>
        /// <param name="id">id essai</param>
        /// <returns></returns>
        public async Task<IActionResult> ValiderEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                //InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = resaAValiderDb.ObtenirInfosProjetFromEssai(id), //obtenir les infos projet à partir de l'id essai je l'utilise pour l'affichage!
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>(),
                IdEss = id
            };
            ViewBag.modalValid = "show";
            return View("ReservationsAValider", vm);
        }

        /// <summary>
        /// Action post qui permet de traiter la validation d'un essai après confirmation de l'utilisateur
        /// </summary>
        /// <param name="vm"> view model vue</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ValiderEssaiAsync(ResasPourValidationViewModel vm)
        {
            // Retry pour envoi mail
            string message;
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;

            bool isValidOk = resaAValiderDb.ValiderEssai(vm.IdEss);
            if(isValidOk)
            {
                ViewBag.SuccessMessage = "Ok";
                ViewBag.Action = "Validation";

                #region envoi de mail validation
                var essai = resaAValiderDb.ObtenirEssai(vm.IdEss);
                var proj = resaAValiderDb.ObtenirInfosProjet(essai.projetID);
                message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Votre demande de réservation pour le projet N° : <b> " + proj.NumProjet + "</b> (Essai N°: " + essai.id + " ) " +
                             " vient d'être validée."
                            + "</p><p>L'équipe PFL, </p> </body></html>" ;

                // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                retryCount = NumberOfRetries;
                success = false;

                while (!success && retryCount > 0)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(userManager.FindByIdAsync(essai.compte_userID.ToString()).Result.Email, "Notification de validation essai", message);
                        success = true;
                    }
                    catch (Exception e)
                    {
                        retryCount--;

                        if (retryCount == 0)
                        {
                            ModelState.AddModelError("", "Problème de connexion pour l'envoie de mail! : " + e.Message + ".");
                            goto ENDT;
                        }
                    }
                }
                #endregion
            }
            else
            {
                ModelState.AddModelError("", "Problème lors de la validation réservation. Veuillez essayer ultérieurement");               
            }

        ENDT:
            vm.resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync();
            vm.InfosEssai = new ConsultInfosEssaiChildVM();
            vm.InfosProj = new InfosProjet();
            vm.Reservations = new List<InfosReservation>();
            vm.InfosConflits = new List<InfosConflit>();
            vm.IdEss = vm.IdEss;

            return View("ReservationsAValider", vm);
        }

        /// <summary>
        /// Action pour charger les infos sur un essai à refuser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RefuserEssaiAsync(int id)
        {
            ResasPourValidationViewModel vm = new ResasPourValidationViewModel()
            {
                resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync(),
                //InfosEssai = new ConsultInfosEssaiChildVM(),
                InfosProj = resaAValiderDb.ObtenirInfosProjetFromEssai(id), //obtenir les infos projet à partir de l'id essai
                Reservations = new List<InfosReservation>(),
                InfosConflits = new List<InfosConflit>(),
                IdEss = id
            };
            ViewBag.modalRefus = "show";
            return View("ReservationsAValider", vm);
        }

        /// <summary>
        /// Action post qui permet de traiter le refus d'un essai après confirmation de l'utilisateur
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RefuserEssaiAsync(ResasPourValidationViewModel vm)
        {
            // Retry pour envoi mail
            string message;
            int NumberOfRetries = 5;
            var retryCount = NumberOfRetries;
            var success = false;

            if (ModelState.IsValid)
            {
                bool isRefusOk = resaAValiderDb.RefuserEssai(vm.IdEss, vm.RaisonRefus);
                if (isRefusOk)
                {
                    ViewBag.SuccessMessage = "Ok";
                    ViewBag.Action = "Refus";

                    #region envoi de mail validation
                    var essai = resaAValiderDb.ObtenirEssai(vm.IdEss);
                    var proj = resaAValiderDb.ObtenirInfosProjet(essai.projetID);
                    message = @"<html>
                            <body> 
                            <p> Bonjour, <br><br> Votre demande de réservation pour le projet N° : <b> " + proj.NumProjet + "</b> (Essai N°: " + essai.id + " ) " +
                                 " est refusée par notre équipe. Raison du refus: " + essai.raison_refus 
                                + ".<br> Nous nous excusons du désagrément, pour toute question n'hesitez pas à nous contacter.</p><p>L'équipe PFL, </p> </body></html>";

                    // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                    retryCount = NumberOfRetries;
                    success = false;

                    while (!success && retryCount > 0)
                    {
                        try
                        {
                            await emailSender.SendEmailAsync(userManager.FindByIdAsync(essai.compte_userID.ToString()).Result.Email, "Notification de refus essai", message);
                            success = true;
                        }
                        catch (Exception e)
                        {
                            retryCount--;

                            if (retryCount == 0)
                            {
                                ModelState.AddModelError("", "Problème de connexion pour l'envoie de mail! : " + e.Message + ".");
                                goto ENDT;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    ModelState.AddModelError("", "Problème pour refuser la réservation. Veuillez essayer ultérieurement");
                }
            }
            else
            {
                ViewBag.modalRefus = "show";
            }

        ENDT:
            vm.resasAValider = await resaAValiderDb.ObtenirInfosAffichageAsync();
            vm.InfosEssai = new ConsultInfosEssaiChildVM();
            vm.InfosProj = new InfosProjet();
            vm.Reservations = new List<InfosReservation>();
            vm.InfosConflits = new List<InfosConflit>();
            vm.IdEss = vm.IdEss;

            return View("ReservationsAValider", vm);
        }
    }
}