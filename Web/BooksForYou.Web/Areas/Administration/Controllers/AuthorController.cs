namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Web.ViewModels.Administration.Authors;
    using BooksForYou.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class AuthorController : AdministrationController
    {
        private readonly IAuthorsService _authorsService;

        private readonly IGenresService _genresService;

        public AuthorController(
            IAuthorsService authorsService,
            IGenresService genresService)
        {
            _authorsService = authorsService;
            _genresService = genresService;
        }

        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var authors = await _authorsService.GetAuthorsAsync(p, s);

            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _authorsService.GetAuthorByIdAsync<AuthorDeleteViewModel>(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorsService.DeleteAuthorAsync(id);

            return RedirectToAction(nameof(All));
        }
    }
}
