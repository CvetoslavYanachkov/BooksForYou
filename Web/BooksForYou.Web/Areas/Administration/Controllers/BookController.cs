namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Services.Data.Books;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Languages;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class BookController : AdministrationController
    {
        private readonly IBooksService _booksService;
        private readonly IGenresService _genresService;
        private readonly IAuthorsService _authorService;
        private readonly ILanguagesService _languagesService;
        private readonly IPublisherService _publishersService;

        public BookController(
            IBooksService booksService,
            IGenresService genresService,
            IAuthorsService authorService,
            ILanguagesService languagesService,
            IPublisherService publishersService)
        {
            _booksService = booksService;
            _genresService = genresService;
            _authorService = authorService;
            _languagesService = languagesService;
            _publishersService = publishersService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var books = await _booksService.GetBooksAsync(p, s);

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BookCreateViewModel()
            {
                Authors = await _authorService.GetAuthorsToCreateAsync(),
                Genres = await _genresService.GetGenresToCreateAsync(),
                Languages = await _languagesService.GetLanguagesToCreateAsync(),
                Publishers = await _publishersService.GetPublishersToCreateAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateViewModel model, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                model.Authors = await _authorService.GetAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return this.View(model);
            }

            if (!ModelState.IsValid)
            {
                model.Authors = await _authorService.GetAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return View();
            }

            try
            {
                var book = await _booksService.CreateBookAsync(model, file);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                model.Authors = await _authorService.GetAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _booksService.GetBookByIdAsync<BookDeleteViewModel>(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _booksService.DeleteBookAsync(id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _booksService.GetBookForEditAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = await _authorService.GetAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return View(model);
            }

            try
            {
                await _booksService.UpdateBookAsync(id, model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }
    }
}
