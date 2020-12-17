using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using SiteGestionResaCore.Services;
using SiteGestionResaCore.Models;

namespace SiteGestionResaCore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly SignInManager<utilisateur> signInManager;
        private readonly IEmailSender emailSender;
        private readonly object claimTypes;

        public ManageController(
            UserManager<utilisateur> userManager,
            SignInManager<utilisateur> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user =  await userManager.FindByIdAsync(User.GetUserId());
            var model = new IndexViewModel
            {
                HasPassword = await HasPasswordAsync(),
                PhoneNumber = await userManager.GetPhoneNumberAsync(user),
                TwoFactor = await userManager.GetTwoFactorEnabledAsync(user),
                Logins = await userManager.GetLoginsAsync(user)//,
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(user)
            };
            return View(model);
        }


        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(User.GetUserId());
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                if (user != null)
                {
                    // TODO: vérifier "remember browser" voir avec christophe
                    //await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    await signInManager.SignInAsync(user, new AuthenticationProperties
                    {
                        IsPersistent = false,                        
                    });
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(User.GetUserId());
                var result = await userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    //var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        //await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        await signInManager.SignInAsync(user, new AuthenticationProperties
                        {
                            IsPersistent = false,
                        });
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task<bool> HasPasswordAsync()
        {
            var user = await userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}