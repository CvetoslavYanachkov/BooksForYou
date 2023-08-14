namespace BooksForYou.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Services.Data.Books;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Languages;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.Extensions.FileSystemGlobbing;

    public class BookController : BaseController
    {
        private readonly IBooksService _booksService;
        private readonly IGenresService _genresService;
        private readonly IUsersService _userService;
        private readonly ILanguagesService _languagesService;
        private readonly IPublishersService _publishersService;

        public BookController(
            IBooksService booksService,
            IGenresService genresService,
            IUsersService userService,
            ILanguagesService languagesService,
            IPublishersService publishersService)
        {
            _booksService = booksService;
            _genresService = genresService;
            _userService = userService;
            _languagesService = languagesService;
            _publishersService = publishersService;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> AllFromAdmin([FromQuery] int p = 1, [FromQuery] int s = 6)
        {
            var books = await _booksService.GetBooksAsync(p, s);

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> BookById(int id)
        {
            var bookModel = await _booksService.GetBookByIdAsync<BookSingleViewModel>(id);

            return View(bookModel);
        }

        public async Task<IActionResult> BookByIdFromMyBooks(int id)
        {
            var bookModel = await _booksService.GetBookByIdAsync<BookSingleViewModel>(id);

            return View(bookModel);
        }


        [HttpGet]
        public async Task<IActionResult> All([FromQuery]AllBooksQueryModel query)
        {
            var result = await _booksService.GetBooksAsync(
                query.Sorting,
                query.SearchTerm,
                query.Genre,
                query.Publisher,
                query.CurrentPage,
                AllBooksQueryModel.BooksPerPage);

            query.TotalRecords = result.TotalBooksRecords;
            query.Genres = await _genresService.GetGenreNamesAsync();
            query.Publishers = await _publishersService.GetPublishersNameAsync();
            query.Books = result.Books;

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BookCreateViewModel()
            {
                UsersAuthors = await _userService.GetUsersAuthorsToCreateAsync(),
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
                model.UsersAuthors = await _userService.GetUsersAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.UsersAuthors = await _userService.GetUsersAuthorsToCreateAsync();
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
                model.UsersAuthors = await _userService.GetUsersAuthorsToCreateAsync();
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

        [HttpPost]
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
                model.UsersAuthors = await _userService.GetUsersAuthorsToCreateAsync();
                model.Genres = await _genresService.GetGenresToCreateAsync();
                model.Languages = await _languagesService.GetLanguagesToCreateAsync();
                model.Publishers = await _publishersService.GetPublishersToCreateAsync();

                return View(model);
            }

            try
            {
                await _booksService.UpdateBookAsync(id, model);

                return RedirectToAction(nameof(AllFromAdmin));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }

        public async Task<IActionResult> AddBookToMyBooks(int bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _booksService.AddBookToMyBooksAsync(userId, bookId);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> MyBooks([FromQuery] int p = 1, [FromQuery] int s = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _booksService.GetMyBooksAsync(p, s, userId);

            return View(model);
        }

        public async Task<IActionResult> RemoveBookFromMyBooks(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _booksService.RemoveBookFromMyBooksAsync(bookId, userId);

            return RedirectToAction(nameof(All));
        }
    }
}
