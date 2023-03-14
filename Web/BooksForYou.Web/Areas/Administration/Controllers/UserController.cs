﻿namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data;
    using BooksForYou.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : AdministrationController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public UserController(
           RoleManager<ApplicationRole> roleManager,
           SignInManager<ApplicationUser> signInManager,
           IUserService userService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<IActionResult> CreateRole()
        {
            ////await _roleManager.CreateAsync(new ApplicationRole()
            ////{
            //    Name = "Author"
            ////});

            return Ok();
        }

        public async Task<IActionResult> All(int pageNumber = 1)
        {
            int pageSize = 10;
            var users = await _userService.GetUsersAsync(pageNumber, pageSize);

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

            await _userService.UpdateUserAsync(id, model);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> ById(string id)
        {
            var user = await _userService.GetUserById(id);

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(All));
        }
    }
}
