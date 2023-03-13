namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data;
    using BooksForYou.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.CodeAnalysis.VisualBasic.Syntax;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;

    public class UserController : AdministrationController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public UserController
       (RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IUserService userService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<IActionResult> CreateRole()
        {
            //await _roleManager.CreateAsync(new ApplicationRole()
            //{
            //    Name = "Author"
            //});

            return Ok();
        }

        public async Task<IActionResult> All()
        {
            var users = await _userService.GetUsersAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.GetUserEditAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.UpdateAsync(id, model);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> ById(string id)
        {
            var user = await _userService.GetUserById(id);

            return View(user);
        }
    }
}
