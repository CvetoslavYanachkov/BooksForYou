namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data;
    using BooksForYou.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : AdministrationController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsersService _userService;

        public UserController(
           RoleManager<ApplicationRole> roleManager,
           SignInManager<ApplicationUser> signInManager,
           IUsersService userService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        // public async Task<IActionResult> CreateRole()
        // {
        //    await _roleManager.CreateAsync(new ApplicationRole()
        //    {
        //        Name = "Publisher"
        //    });

        /// return Ok();
        // }
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var users = await _userService.GetUsersAsync(p, s);

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.GetUserForEditAsync(id);

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

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _userService.GetUserForDeleteAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, UserDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.DeleteUserAsync(id, model);

            return RedirectToAction(nameof(All));
        }
    }
}
