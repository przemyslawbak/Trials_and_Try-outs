using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.Models;
using Users.Services.Email;

namespace Users.Controllers
{
    //[Authorize(Roles = "Administratorzy")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly IEmailSender _emailSender;
        public AdminController(UserManager<AppUser> usrMgr, IUserValidator<AppUser> userValid, IPasswordValidator<AppUser> passValid, IPasswordHasher<AppUser> passwordHash, IEmailSender emailSender)
        {
            _userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            _emailSender = emailSender;
        }

        public ViewResult Index()
        {
            return View(_userManager.Users);
        }

        public ViewResult Create()
        {
            return View();
        }

        public IActionResult Confirmation()
        {
            return View("Confirmation");
        }

        public IActionResult ConfirmEmail(string userid, string token) //!!!!!!!!!!!!!!!!!!!!!!!!
        {
            AppUser user = _userManager.FindByIdAsync(userid).Result;
            IdentityResult result = _userManager.ConfirmEmailAsync(user, token).Result;
            if (result.Succeeded)
            {
                ViewBag.Message = "Email confirmed successfully!";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "Error while confirming your email!";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result; //!!!!!!!!!!!!!!!!!!!!!!!!
                    string confirmationLink = Url.Action("ConfirmEmail", "Admin", new //!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        userid = user.Id,
                        token = confirmationToken
                    },

                    protocol: HttpContext.Request.Scheme); //!!!!!!!!!!!!!!!!!!!!!!!!

                    await _emailSender.SendEmailAsync(user.Email, "Confirmation email", confirmationLink); //!!!!!!!!!!!!!!!!!!!!!!!!

                    return RedirectToAction("Confirmation"); //!!!!!!!!!!!!!!!!!!!!!!!!
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            }
            return View("Index", _userManager.Users);
        }
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email,
        string password)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                = await userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(_userManager,
                    user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                        password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                || (validEmail.Succeeded
                && password != string.Empty && validPass.Succeeded))
            {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            }
            return View(user);
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
