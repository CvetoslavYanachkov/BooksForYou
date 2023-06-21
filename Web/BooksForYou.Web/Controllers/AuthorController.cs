namespace BooksForYou.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Services.Messaging;
    using BooksForYou.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NuGet.Packaging.Signing;

    public class AuthorController : Controller
    {
        private readonly IAuthorsService _authorsService;

        private readonly IGenresService _genresService;

        private readonly IEmailSender _emailSender;

        private readonly IUsersService _userService;

        public AuthorController(
            IAuthorsService authorsService,
            IEmailSender emailSender,
            IGenresService genresService,
            IUsersService userService)
        {
            _authorsService = authorsService;
            _emailSender = emailSender;
            _genresService = genresService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var authors = await _authorsService.GetAuthorsAsync(p, s);

            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nameOfAuthor = await _userService.GetNameOfUser(userId);

            var model = new AuthorCreateViewModel()
            {
                Name = nameOfAuthor,
                Genres = await _genresService.GetGenresToCreateAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateViewModel model, IFormFile file)
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
                var author = await _authorsService.CreateAuthorAsync(userId, model, file);

                return RedirectToAction(nameof(AuthorById));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AuthorById(int id)
        {
            var authorModel = await _authorsService.GetAuthorByIdAsync<AuthorSingleViewModel>(id);

            return View(authorModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RequestToBecome()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userService.ExistsById(userId) == true)
            {
                //Some error message

                return RedirectToAction(nameof(Index), nameof(HomeController));
            }

            var model = new AuthorBecomeViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RequestToBecome(AuthorBecomeViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userService.UserWithWebsiteExists(model.Website))
            {
                ModelState.AddModelError(nameof(model.Website),
                    "The website already exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var callbackUrl = Url.Page(
                       $"/User/Edit/{userId}",
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
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if (userId != id)
            //{
            //    return BadRequest();
            //}
            var model = await _authorsService.GetAuthorForEditAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AuthorEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _genresService.GetGenresToCreateAsync();
                return View(model);
            }

            try
            {
                await _authorsService.UpdateAuthorAsync(id, model);

                return RedirectToAction(nameof(AuthorById));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }
    }
}
