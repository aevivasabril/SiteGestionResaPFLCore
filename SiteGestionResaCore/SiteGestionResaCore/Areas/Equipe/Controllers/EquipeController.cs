using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Services;
using SiteReservationGestionPFL.Areas.Equipe.Data;
using SiteReservationGestionPFL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteReservationGestionPFL.Areas.Equipe.Controllers
{
    //TODO: donner l'accès uniquement aux administrateurs [Authorize (Roles ="Admin")]
    [Area("Equipe")]
    public class EquipeController : Controller
    {
        private readonly IResaDB resaDb;
        private readonly UserManager<utilisateur> userManager;
        private readonly IEmailSender emailSender;

        public EquipeController(
            IResaDB resaDb,
            UserManager<utilisateur> userManager,
            IEmailSender emailSender)
        {
            this.resaDb = resaDb;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
        
        // GET: Equipe/Equipe
        public async Task<ActionResult> GestionUtilisateurs(GestionUsersViewModel vm)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users, 
                ListUsersWaiting = ListUsr.UsersWaitingValid, 
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(), 
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin)
            };
            return View(vm);
        }


        public async Task<ActionResult> AdminToUserAcces(int? id)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin),
                UserToChange = resaDb.ObtenirUtilisateur(id.Value)
            };
            ViewBag.modalState = "show";
            ViewBag.NameRole = "Utilisateur";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public ActionResult AdminToUserAcces(int id)
        {
            resaDb.ChangeAccesToUser(id);
            return RedirectToAction("GestionUtilisateurs"); //Cette redirection rentre dans le GET et reconstruit le model :)
        }

        public ActionResult AddingAdmin()
        {
            GestionUsersViewModel vm = new GestionUsersViewModel();
            ViewBag.modalAdm = "show";
            return View("GestionUtilisateurs", vm);
        }

        //TODO: dévélopper la méthode post et faire le test une fois que on aura crée et validé autre compte
        [HttpPost]
        public ActionResult AddingAdmin(GestionUsersViewModel model)
        {
            resaDb.ChangeAccesToAdminAsync(model.UserToUpdateId);
            return RedirectToAction("GestionUtilisateurs");
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> AddingLogistic()
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin)
            };

            ViewBag.modalLogic = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public ActionResult AddingLogistic(GestionUsersViewModel model)
        {
            resaDb.AddAdminToLogisticRoleAsync(model.AdminToLogisticId);
            return RedirectToAction("GestionUtilisateurs");
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> RemoveLogisticUser(int ?id)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin),
                UserToChange = resaDb.ObtenirUtilisateur(id.Value)
            };

            ViewBag.modalRemove = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public ActionResult RemoveLogisticUser(int id)
        {
            resaDb.RemoveLogisticRoleAsync(id);
            return RedirectToAction("GestionUtilisateurs");
        }

        public async Task<ActionResult> Valider(int? id)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin),
                UserToChange = resaDb.ObtenirUtilisateur(id.Value),
                ActionName = "Valider"
            };
            ViewBag.modalWait = "show";
            return View("GestionUtilisateurs", vm);
        }

        // TODO: tester les exceptions!! 
        [HttpPost]
        public async Task<ActionResult> Valider(int id)
        {
            //string aspNetID;
            //IdentityUser user;

            try
            {
                resaDb.ValidateAccount(id);
                //aspNetID = resaDb.IdAspNetUser(id);
                var user = await userManager.FindByIdAsync(id.ToString());

                // envoyer le mail à l'utilisateur concerné
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Scheme);
                await emailSender.SendEmailAsync(user.Email, "Votre compte PFL", "Bonjour,\n\nVotre compte a été validé par nos administrateurs! \nPour finir le processus de validation, veuillez cliquer sur le lien suivant: \"" + callbackUrl + "\"" + "\n\nL'équipe PFL,");                
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de l'écriture dans la BDD ou l'envoi de mail.";
                return View("Error");
            }
           
            return RedirectToAction("GestionUtilisateurs");
        }

        public async Task<ActionResult> Refuser(int? id)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin),
                UserToChange = resaDb.ObtenirUtilisateur(id.Value),
                ActionName = "Refuser"
            };
            ViewBag.modalWait = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public async Task<ActionResult> Refuser(int id)
        {          
            try
            {
                //aspNetID = resaDb.IdAspNetUser(id);
                //user = await userManager.FindByIdAsync(aspNetID);
                //Effacer de la BDD pfl
                await resaDb.DeleteRequestAccount(id);
                var user = await userManager.FindByIdAsync(id.ToString());
                // Retirer des rôles
                var allUserRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, allUserRoles);
                // TODO: Voir si cela se passe bien puisque on efface l'utilisateur! sinon inverser les 2 lignes
                // Effacer de la BDD AspNet
                await emailSender.SendEmailAsync(user.Email, "Votre compte PFL", "Bonjour,\n\nVotre demande d'ouverture de compte est refusée, nous vous prions de nous excuser pour la gêne occasionée.\nVenez nous voir ou contactez-nous si vous avez des questions.\n\n L'équipe PFL,");
                await userManager.DeleteAsync(user);

            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de suppression de la demande";
                return View("Error");
            }

            return RedirectToAction("GestionUtilisateurs");
        }

        public async Task<ActionResult> DeleteUser(int? id)
        {
            listAutresUtilisateurs ListUsr = resaDb.ObtenirListAutres();
            List<utilisateur> ListAdmin = resaDb.ObtenirListAdmins();

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await resaDb.ObtenirUsersLogisticAsync(),
                UserItem = resaDb.ListToSelectItem(ListUsr.Users),
                AdminItem = resaDb.ListToSelectItem(ListAdmin),
                UserToChange = resaDb.ObtenirUtilisateur(id.Value)
            };
            ViewBag.modalDelete = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {

                //Effacer de la BDD pfl
                await resaDb.DeleteRequestAccount(id);
                var user = await userManager.FindByIdAsync(id.ToString());
                //Effacer de tous les roles AspNet
                await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
                // Effacer de la BDD AspNet
                await userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de la suppression de la demande";
                return View("Error");
            }

            return RedirectToAction("GestionUtilisateurs");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userManager?.Dispose();
            }
            // Rajouter pour éviter des pbs d'accès aux ressources resaDb
            resaDb.Dispose();
            base.Dispose(disposing);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
    }
}