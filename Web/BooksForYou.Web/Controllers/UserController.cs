namespace BooksForYou.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Services.Messaging;
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
        private readonly IEmailSender _emailSender;

        public UserController(
           RoleManager<ApplicationRole> roleManager,
           SignInManager<ApplicationUser> signInManager,
           IUsersService userService,
           IGenresService genresService,
           IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
            _genresService = genresService;
            _emailSender = emailSender;
        }

        //[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new ApplicationRole()
        //    {
        //        Name = "Author"
        //    });

        //    return Ok();
        //}

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.GetUserForEditAsync(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
        [AllowAnonymous]
        public async Task<IActionResult> RequestToBecome()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userService.ExistsById(userId) == true)
            {
                return BadRequest();
            }

            var model = new UserRequestToBecomeAuthorViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RequestToBecome(UserRequestToBecomeAuthorViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userService.UserWithWebsiteExists(model.Website))
            {
                ModelState.AddModelError(nameof(model.Website), "The website already exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var callbackUrl = Url.Page(
                       $"/User/BecomeAuthor/{userId}",
                       pageHandler: null,
                       values: new { },
                       protocol: Request.Scheme);
            var html = new StringBuilder();
            html.AppendLine($"<h1>{"Request to become author!"}</h1>");
            html.AppendLine($"<h3>{"Dear Administrator,pls would you like to make me an author?"}</h3>");
            html.AppendLine($"<h3>{$"Dear Administrator,pls visit this link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>"}</h3>");
            await _emailSender.SendEmailAsync("cyanachkov@gmail.com", "Books For You!", "ceno1902@gmail.com", "Admin", html.ToString());

            //To do new view about confirmation of request from Author to Admin!
            return RedirectToAction("All", "Book");
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
