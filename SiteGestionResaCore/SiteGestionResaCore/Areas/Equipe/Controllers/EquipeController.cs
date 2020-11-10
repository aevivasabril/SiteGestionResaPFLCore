using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteGestionResaCore.Areas.Reservation.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SiteGestionResaCore.Areas.Equipe.Controllers
{
    // Ce controleur est destiné à la gestion des utilisateurs (pour les autres options créer un autre controleur dans l'area Equipe)
    //TODO: donner l'accès uniquement aux administrateurs [Authorize (Roles ="Admin")]
    [Area("Equipe")]
    public class EquipeController : Controller
    {
        private readonly IEquipeResaDb EquipeResaDb;
        private readonly UserManager<utilisateur> userManager;
        private readonly IEmailSender emailSender;

        public EquipeController(
            IEquipeResaDb EquipeResaDb,
            UserManager<utilisateur> userManager,
            IEmailSender emailSender)
        {
            this.EquipeResaDb = EquipeResaDb;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
        
        // GET: Equipe/Equipe
        public async Task<ActionResult> GestionUtilisateurs(GestionUsersViewModel vm)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users, 
                ListUsersWaiting = ListUsr.UsersWaitingValid, 
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(), 
                UserItem = UsersItem,
                AdminItem = AdminItem
            };
            return View(vm);
        }


        public async Task<ActionResult> AdminToUserAcces(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
            };
            ViewBag.modalState = "show";
            ViewBag.NameRole = "Utilisateur";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public ActionResult AdminToUserAcces(GestionUsersViewModel model)
        {
            EquipeResaDb.ChangeAccesToUser(model.UserToChange.Id);
            return RedirectToAction("GestionUtilisateurs"); //Cette redirection rentre dans le GET et reconstruit le model :)
        }

        public async Task<ActionResult> AddingAdminAsync()
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem
            };
            ViewBag.modalAdm = "show";
            return View("GestionUtilisateurs", vm);
        }

        //TODO: dévélopper la méthode post et faire le test une fois que on aura crée et validé autre compte
        [HttpPost]
        public ActionResult AddingAdmin(GestionUsersViewModel model)
        {
            EquipeResaDb.ChangeAccesToAdminAsync(model.UserToUpdateId);
            return RedirectToAction("GestionUtilisateurs");
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> AddingLogistic()
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem
            };

            ViewBag.modalLogic = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> AddingLogisticAsync(GestionUsersViewModel model)
        {
            await EquipeResaDb.AddAdminToLogisticRoleAsync(model.AdminToLogisticId);
            return RedirectToAction("GestionUtilisateurs");
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> RemoveLogisticUser(int ?id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
            };

            ViewBag.modalRemove = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> RemoveLogisticUserAsync(int id)
        {
            await EquipeResaDb.RemoveLogisticRoleAsync(id);
            return RedirectToAction("GestionUtilisateurs");
        }

        public async Task<ActionResult> Valider(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value),
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
                EquipeResaDb.ValidateAccount(id);
                //aspNetID = resaDb.IdAspNetUser(id);
                var user = await userManager.FindByIdAsync(id.ToString());

                // envoyer le mail à l'utilisateur concerné
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Scheme);
                string html = @"<html>
                                <body>
                                    <p>
                                Bonjour <br/>
                                Votre compte a été validé par nos administrateurs! <br/>
                                Pour finir le processus de validation, <a href='[CALLBACK_URL]'>veuillez cliquer ici</a><br/>
                                    </p>
                                <p>
                                L'équipe PFL
                                </p>
                                </body>
                                </html>";
                await emailSender.SendEmailAsync(user.Email, "Votre compte PFL", html.Replace("[CALLBACK_URL]", callbackUrl));                
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
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value),
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
                //Effacer de la BDD pfl
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
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();

            var UsersItem = ListUsr.Users.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            var AdminItem = ListAdmin.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;

            GestionUsersViewModel vm = new GestionUsersViewModel()
            {
                UsersAdmin = ListAdmin,
                ListUsers = ListUsr.Users,
                ListUsersWaiting = ListUsr.UsersWaitingValid,
                ListAdminLogistic = await EquipeResaDb.ObtenirUsersLogisticAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
    }
}