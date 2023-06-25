namespace BooksForYou.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Web.ViewModels.Authors;
    using BooksForYou.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    public class UserController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsersService _userService;
        private readonly IGenresService _genresService;

        public UserController(
           RoleManager<ApplicationRole> roleManager,
           SignInManager<ApplicationUser> signInManager,
           IUsersService userService,
           IGenresService genresService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
            _genresService = genresService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "Author"
            });

            return Ok();
        }

        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var users = await _userService.GetUsersAsync(p, s);

            return View(users);
        }

        public async Task<IActionResult> AllUsersAuthors([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var users = await _userService.GetUsersWithRoleAuthorAsync(p, s);

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
        public async Task<IActionResult> BecomeAuthor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nameOfAuthor = await _userService.GetNameOfUser(userId);

            var model = new UserBecomesAuthorViewModel()
            {
                Genres = await _genresService.GetGenresToCreateAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BecomeAuthor(UserBecomesAuthorViewModel model, IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (file == null || file.Length == 0)
            {
                model.Genres = await _genresService.GetGenresToCreateAsync();
                return this.View(model);
            }

            if (!ModelState.IsValid)
            {
                model.Genres = await _genresService.GetGenresToCreateAsync();
                return View();
            }

            try
            {
                await _userService.UserBecomeAuthorAsync(userId, model, file);

                return RedirectToAction(nameof(AllUsersAuthors));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }
    }
}
