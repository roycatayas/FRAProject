using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRA.IdentityProvider.Entities;
using FRA.Web.Models.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHostingEnvironment hostingEnvironment, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            // If the user is already authenticated we do not need to display the login page, so we redirect to the landing page.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Response = new LoginResponseViewModel
                {
                    Succeeded = false,
                    Description = "Request does not contain the required information to log the user in."
                };

                return View();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    ViewBag.Response = new LoginResponseViewModel
                    {
                        Succeeded = false,
                        Description = "Please confirm your account before you try to log in."
                    };

                    return View();
                }

                if (user.LockoutEnabled)
                {
                    ViewBag.Response = new LoginResponseViewModel
                    {
                        Succeeded = false,
                        Description = "Your account is locked. Please contact support."
                    };

                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
            }

            ViewBag.Response = new LoginResponseViewModel
            {
                Succeeded = false,
                Description = "Please check your credentials"
            };

            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}