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
    // Pas nécessaire d'ajouter l'autorisation car le attribut est déjà ajouté dans le Home controller [Authorize (Roles ="Admin")]
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
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };
            return View(vm);
        }


        public async Task<ActionResult> AdminToUserAcces(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
            };
            ViewBag.modalState = "show";
            ViewBag.NameRole = "Utilisateur";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public async Task<ActionResult> AdminToUserAccesAsync(int id)
        {
            try
            {
                await EquipeResaDb.ChangeAccesToUserAsync(id);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de changement de rôle \"Admin\" vers \"Utilisateur\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Changement de rôle \"Admin\" vers \"Utilisateur\" réussi! ";

            return View("GestionUtilisateurs", vm); //Cette redirection rentre dans le GET et reconstruit le model :)
        }

        public async Task<ActionResult> AddingAdminAsync()
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };
            ViewBag.modalAdm = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public async Task<ActionResult> AddingAdminAsync(GestionUsersViewModel model)
        {
            try
            {
                await EquipeResaDb.ChangeAccesToAdminAsync(model.UserToUpdateId);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de l'ajout utilisateur dans le rôle \"Admin\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem, 
                MaintItem = MaintItem
            };
            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Ajout de l'utilisateur dans le rôle \"Admin\" réussi! ";

            return View("GestionUtilisateurs", vm);
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> AddingLogistic()
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            ViewBag.modalLogic = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> AddingLogisticAsync(GestionUsersViewModel model)
        {
            try
            {
                await EquipeResaDb.AddAdminToLogisticRoleAsync(model.AdminToLogisticId);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de l'ajout utilisateur dans le rôle \"Logistic\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };
            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Ajout de l'utilisateur dans le rôle \"Logistic\" réussi! ";

            return View("GestionUtilisateurs", vm);
        }

        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> RemoveLogisticUser(int ?id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
            };

            ViewBag.modalRemove = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public async Task<ActionResult> RemoveLogisticUserAsync(int id)
        {
            try
            {
                await EquipeResaDb.RemoveLogisticRoleAsync(id);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de la extraction de l'utilisateur du rôle \"Logistic\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem, 
                MaintItem = MaintItem
            };

            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Extraction de l'utilisateur du rôle \"Logistic\" réussi! ";

            return View("GestionUtilisateurs", vm);
        }

        public async Task<ActionResult> Valider(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value),
                ActionName = "Valider"
            };
            ViewBag.modalWait = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        public async Task<ActionResult> Valider(int id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());

                // envoyer le mail à l'utilisateur concerné
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Scheme);
                string html = @"<html>
                                <body>
                                    <p>
                                    Bonjour,  <br>
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

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Compte validé! ";

            return View("GestionUtilisateurs", vm);
        }

        public async Task<ActionResult> Refuser(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
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
                // Effacer de la BDD AspNet
                await emailSender.SendEmailAsync(user.Email, "Votre compte PFL", "Bonjour,\n\nVotre demande d'ouverture de compte est refusée, nous vous prions de nous excuser pour la gêne occasionée.\nVenez nous voir ou contactez-nous si vous avez des questions.\n\n L'équipe PFL,");
                await userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de suppression de la demande";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Compte refusé! ";

            return View("GestionUtilisateurs", vm);
        }

        public async Task<ActionResult> DeleteUser(int? id)
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
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
                var user = await userManager.FindByIdAsync(id.ToString());
                //int x = EquipeResaDb.NbEssaiXUser(user);
                if (EquipeResaDb.NbEssaiXUser(user) != 0) // alors mettre le compte à inactif pour garder l'historique!
                {
                    //Effacer de tous les roles AspNet
                    //await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
                    // mettre le compte utilisateur à inactif pour ignorer sa connexion
                    user.compteInactif = true;
                    await userManager.UpdateAsync(user);
                }
                else // alors on peut supprimer l'utilisateur définitivement
                {
                    //Effacer de tous les roles AspNet
                    await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
                    // Effacer de la BDD AspNet
                    await userManager.DeleteAsync(user);
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de la suppression compte";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };
            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Utilisateur supprimé! ";

            return View("GestionUtilisateurs", vm);
        }

        [Authorize(Roles = "LogisticMaint, MainAdmin")]
        public async Task<IActionResult> AddingLogisticIntervAsync()
        {
            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            ViewBag.modalInterv = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "LogisticMaint, MainAdmin")]
        public async Task<IActionResult> AddingAdminIntervAsync(GestionUsersViewModel model)
        {
            try
            {
                await EquipeResaDb.AddingAdminToInterv(model.AdminToIntervId);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de l'ajout utilisateur dans le rôle \"LogisticMaint\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };
            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Ajout de l'utilisateur dans le rôle \"LogisticMaint\" réussi! ";

            return View("GestionUtilisateurs", vm);
        }

        [Authorize(Roles = "LogisticMaint, MainAdmin")]
        public async Task<IActionResult> RemoveIntervUserAsync(int? id)
        {
            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem,
                UserToChange = EquipeResaDb.ObtenirUtilisateur(id.Value)
            };
            #endregion

            ViewBag.modalRemoveInterv = "show";
            return View("GestionUtilisateurs", vm);
        }

        [HttpPost]
        [Authorize(Roles = "LogisticMaint, MainAdmin")]
        public async Task<IActionResult> RemoveIntervUserAsync(int id)
        {
            try
            {
                await EquipeResaDb.RemoveLogisticMaintRoleAsync(id);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString() + ". Problème survenue lors de la extraction de l'utilisateur du rôle \"LogisticMaint\"";
                return View("Error");
            }

            #region Initialiser le view model pour pouvoir afficher le message dans le ViewBag

            listAutresUtilisateurs ListUsr = await EquipeResaDb.ObtenirListAutresAsync();
            List<utilisateur> ListAdmin = EquipeResaDb.ObtenirListAdmins();
            IList<utilisateur> ListInterv = await EquipeResaDb.ObtenirUsersIntervAsync();

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

            var MaintItem = ListInterv.Select(f => new SelectListItem
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
                ListAdminInterv = await EquipeResaDb.ObtenirUsersIntervAsync(),
                UserItem = UsersItem,
                AdminItem = AdminItem,
                MaintItem = MaintItem
            };

            #endregion

            // Afficher message pour indiquer que l'opération s'est bien passé
            ViewBag.AfficherMessage = true;
            ViewBag.Message = "Extraction de l'utilisateur du rôle \"LogisticMaint\" réussi! ";

            return View("GestionUtilisateurs", vm);
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