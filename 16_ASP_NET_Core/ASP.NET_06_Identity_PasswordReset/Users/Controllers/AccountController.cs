using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.Models;
using Users.Services.Email;

namespace Users.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, IEmailSender emailSender)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    if (!userManager.IsEmailConfirmedAsync(user).Result) //EMAIL CONFIRMED OR NOT
                    {
                        ModelState.AddModelError(nameof(LoginModel.Email), "Email not confirmed!");
                        return View("Login");
                    }
                    else
                    {
                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                        if (result.Succeeded)
                        {
                            return Redirect(returnUrl ?? "/");
                        }
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                "Nieprawidłowa nazwa użytkownika lub hasło.");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetRequest()
        {
            return View("Request");
        }

        [AllowAnonymous]
        public async Task<IActionResult> SendPasswordResetLinkAsync(string email)
        {
            AppUser user = userManager.FindByEmailAsync(email).Result; //CHECK WITH MODEL VALIDATION

            if (user == null || !(userManager.IsEmailConfirmedAsync(user).Result))
            {
                ViewBag.Message = "Error while resetting your password!";
                return View("Error");
            }

            var token = userManager.GeneratePasswordResetTokenAsync(user).Result;

            var resetLink = Url.Action("ResetPassword", "Account", new { token = token },

                             protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Reset password", resetLink); //!!!!!!!!!!!!!!!!!!!!!!!!

            ViewBag.Message = "Password reset link has been sent to your email address!";

            return View("Login");
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            return View("Reset");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetPassword
                (ResetPasswordViewModel obj)
        {
            AppUser user = userManager.FindByEmailAsync(obj.Email).Result;

            IdentityResult result = userManager.ResetPasswordAsync(user, obj.Token, obj.Password).Result;
            if (result.Succeeded)
            {
                ViewBag.Message = "Password reset successful!";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "Error while resetting the password!";
                return View("Error");
            }
        }
    }
}
