﻿namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Web.ViewModels.Administration.Authors;
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
        public async Task<IActionResult> Create()
        {
            var model = new AuthorCreateViewModel()
            {
                Genres = await _genresService.GetGenresToCreateAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateViewModel model, IFormFile file)
        {
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
                var author = await _authorsService.CreateAuthorAsync(model, file);

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

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
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
