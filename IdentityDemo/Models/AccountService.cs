using IdentityDemo.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class AccountService
    {
        private readonly UserManager<MyIdentityUser> userManager;
        private readonly SignInManager<MyIdentityUser> signInManager;

        public AccountService(UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> TryRegisterAsync(AccountRegisterVM viewModel)
        {
            var result = await userManager.CreateAsync(new MyIdentityUser { UserName = viewModel.Username }, viewModel.Password);
            return result.Succeeded;
        }

        public async Task<bool> TryLoginAsync(AccountLoginVM viewModel)
        {
            var result = await signInManager.PasswordSignInAsync(
                viewModel.UserName,
                viewModel.Password,
                isPersistent:false,
                lockoutOnFailure: false);
            return result.Succeeded;
        }

        internal async Task TryLogOffAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
