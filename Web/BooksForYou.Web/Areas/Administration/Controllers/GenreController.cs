namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Web.ViewModels.Administration.Genres;
    using Microsoft.AspNetCore.Mvc;

    public class GenreController : AdministrationController
    {
        private readonly IGenresService _genresService;

        public GenreController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var genres = await _genresService.GetGenresAsync(p, s);

            return View(genres);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new GenreCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var genre = await _genresService.CreateGenreAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _genresService.GetGenreForEditAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GenreEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _genresService.UpdateGenreAsync(id, model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _genresService.GetGenreForDeleteAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, GenreDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _genresService.DeleteGenreAsync(id, model);

            return RedirectToAction(nameof(All));
        }
    }
}
