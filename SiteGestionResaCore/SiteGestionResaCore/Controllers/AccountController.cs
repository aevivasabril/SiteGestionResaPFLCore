using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Reservation.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;

namespace SiteGestionResaCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountResaDB accountResaDB;
        private readonly UserManager<utilisateur> userManager;
        private readonly SignInManager<utilisateur> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IFormulaireResaDb formulaireResaDb;

        public AccountController(
            IAccountResaDB accountResaDB,
            UserManager<utilisateur> userManager,
            SignInManager<utilisateur> signInManager,
            IEmailSender emailSender, 
            IFormulaireResaDb formulaireResaDb)
        {
            this.accountResaDB = accountResaDB;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.formulaireResaDb = formulaireResaDb;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)//, string returnUrl)
        {
            string returnUrl = Request.Form["ReturnUrl"];
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user.EmailConfirmed) 
            {
                if(user.compteInactif != true) // Vérifier que le compte n'est pas archivé
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    // PasswordSignInAsync authentifie l'utilisateur 
                    var result = await userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        await signInManager.SignInAsync(user, new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
                        });
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tentative de connexion non valide. Compte mail non enregistré");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Votre compte est inactif! Contactez l'administration PFL");
                    return View(model);
                }                
            }
            else
            {
                ModelState.AddModelError("", "Vous devez confirmer votre adresse mail en cliquant sur le lien que vous a été envoyé par mail");
                return View(model);
            }         
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var allOrgs = formulaireResaDb.ObtenirListOrg().Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_organisme
            });
            RegisterViewModel vm = new RegisterViewModel 
            {
                OrganItem = allOrgs
            };
            return View(vm);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            IList<utilisateur> UsersLogistic = new List<utilisateur>();         // Liste des Administrateurs/Logistic à récupérer pour envoi de notification
            IList<utilisateur> UsersSuperAdm = new List<utilisateur>();

            // Retry pour envoi mail
            int NumberOfRetries = 3;
            var retryCount = NumberOfRetries;
            bool success = false;

            if (ModelState.IsValid)
            {
                var user = new utilisateur { UserName = model.Email, Email = model.Email, nom = model.Nom, prenom= model.Prenom, organismeID = model.SelectedOrganId };
                var result = await userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    // tout nouveau enregistré est utilisateur par défaut jusqu'à ce que un admin l'ajoute dans le groupe admin
                    try
                    {
                        result = await userManager.AddToRoleAsync(user, "Utilisateur");
                        // viewbag pour activer le popup d'info
                        ViewBag.ModalState = "show";
                        // Envoyer un mail au super Admin et le groupe logistic pour qu'il valide le compte 
                        UsersLogistic = await formulaireResaDb.ObtenirLogisticUsersAsync();
                        UsersSuperAdm = await formulaireResaDb.ObtenirMainAdmUsersAsync();
                        string html = @"<html>
                                    <body>
                                    <p>
                                        Bonjour,  <br><br>
                                        Un nouveau utilisateur vient de créer un compte! Vous pouvez valider ou refuser l'ouverture de compte<br/></p>
                                        <p> Nom : " + user.nom + "</p><p> Prénom:" + user.prenom + "</p><p> Mail: " + user.Email
                                        + "</p> <br> L'équipe PFL" +
                                    " </ body >     " +
                                    "</ html > ";

                        // Faire une boucle pour reesayer l'envoi de mail si jamais il y a un pb de connexion
                        // LOGISTIC
                        for (int index = 0; index < UsersLogistic.Count(); index++)
                        {
                            NumberOfRetries = 3;
                            retryCount = NumberOfRetries;
                            success = false;

                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(UsersLogistic[index].Email, "Création d'un nouveau compte utilisateur", html);
                                    success = true;
                                }
                                catch (Exception e)
                                {
                                    retryCount--;

                                    if (retryCount == 0)
                                    {
                                        ViewBag.Message = e.ToString() + "Problème de connexion pour l'envoie de mail! : " + e.Message + ".";
                                        return View("Error");
                                    }
                                }
                            }
                        }
                        // MAINADMIN
                        for (int index = 0; index < UsersSuperAdm.Count(); index++)
                        {
                            NumberOfRetries = 3;
                            retryCount = NumberOfRetries;
                            success = false;

                            while (!success && retryCount > 0)
                            {
                                try
                                {
                                    await emailSender.SendEmailAsync(UsersSuperAdm[index].Email, "Création d'un nouveau compte utilisateur", html);
                                    success = true;
                                }
                                catch (Exception e)
                                {
                                    retryCount--;

                                    if (retryCount == 0)
                                    {
                                        ViewBag.Message = e.ToString() + "Problème de connexion pour l'envoie de mail! : " + e.Message + ".";
                                        return View("Error");
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        ViewBag.Message = e.ToString() + ". Problème pour ajouter ce nouveau utilisateur dans le rôle 'utilisateur' ";
                        return View("Error");
                    }

                }
                AddErrors(result);
            }

            var allOrgs = formulaireResaDb.ObtenirListOrg().Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_organisme
            });
            model.OrganItem = allOrgs;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await  userManager.FindByIdAsync(userId);
            if (userId == null || code == null)
            {
                ViewBag.Message = "Un problème est survenu lors du clic sur le lien de confirmation. UserId = " + userId +". code = " + code + ".";
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            ViewBag.Message = "Un problème est survenu lors du clic sur le lien de confirmation. Il faut une connexion vers le réseau stlo!";
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)) || user.compteInactif == true)
                {
                    // Don't reveal that the user does not exist, is not confirmed or inactive account 
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);		
                await emailSender.SendEmailAsync(user.Email, "Reinitialisation mot de passe PFL", "Pour reinitialiser votre mot de passe cliquez sur le lien suivant: \"" + callbackUrl + "\"" + "\n\nL'équipe PFL,");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
       
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}