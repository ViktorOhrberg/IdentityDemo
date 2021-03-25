using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityDemo.Models;
using IdentityDemo.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        AccountService accountService;
        private readonly SignInManager<MyIdentityUser> signInManager;

        public AccountController(AccountService accountService, SignInManager<MyIdentityUser> signInManager)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("members")]
        public IActionResult Members()
        {
            return View(new AccountMemberVM { UserName = User.Identity.Name });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(AccountRegisterVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Try to register user
            var success = await accountService.TryRegisterAsync(viewModel);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(string.Empty, "Failed to create user");
                return View(viewModel);
            }

            // Redirect user
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new AccountLoginVM { ReturnUrl = returnUrl });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(AccountLoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check if credentials is valid (and set auth cookie)
            var success = await accountService.TryLoginAsync(viewModel);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(nameof(AccountLoginVM.UserName), "Login failed");
                return View(viewModel);
            }

            // Redirect user
            if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                return RedirectToAction(nameof(Members));
            else
                return Redirect(viewModel.ReturnUrl);
        }
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await accountService.TryLogOffAsync();
            return Redirect("login");
        }
    }
}
