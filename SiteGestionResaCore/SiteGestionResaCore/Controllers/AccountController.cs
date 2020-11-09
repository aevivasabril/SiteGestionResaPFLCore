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
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                // PasswordSignInAsync authentifie l'utilisateur 
                var result = await userManager.CheckPasswordAsync(user, model.Password);
                if(result)
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
            if (ModelState.IsValid)
            {
                var user = new utilisateur { UserName = model.Email, Email = model.Email, nom = model.Nom, prenom= model.Prenom, organismeID = model.SelectedOrganId };
                var result = await userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    // Cette ligne on peut la supprimer pour eviter que l'utilisateur soit connecté tout de suite(Voir s'il peut s'authentifier tout de suite après et empecher cela!)
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // Vérifier le format du mail
                    //await userManager.SendEmailAsync(user.Id, "Confirmez votre compte mail", "Votre compte est validé! \n Pour confirmer votre adresse mail veuillez cliquer sur le lien suivant: \"" + callbackUrl + "\"");
                    //TODO: Changer le contenu du mail!!!!!!!!!!!!!!!!
                    //await userManager.SendEmailAsync(user.Id, "Test", "Votre compte est en attente de validation!!!!!!!!!!!");

                    // tout nouveau enregistré est utilisateur par défaut jusqu'à ce que un admin l'ajoute dans le groupe admin
                    result = await userManager.AddToRoleAsync(user, "Utilisateur");

                    if(result.Succeeded)
                    {
                        //créer l'utilisateur hors les infos de gestion de compte individuelle

                        //accountResaDB.CreerUtilisateur(model.Nom, model.Prenom, model.SelectedOrganId, model.Email);

                    }
                    // viewbag pour activer le popup d'info
                    ViewBag.ModalState = "show";
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
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
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
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
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {           
                userManager?.Dispose();         
            }
            accountResaDB.Dispose();

            base.Dispose(disposing);
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