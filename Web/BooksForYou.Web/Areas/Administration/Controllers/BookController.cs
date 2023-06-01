namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Books;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using BooksForYou.Web.ViewModels.Administration.Genres;
    using Microsoft.AspNetCore.Mvc;

    public class BookController : AdministrationController
    {
        private readonly IBooksService _booksService;

        public BookController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var books = await _booksService.GetBooksAsync(p, s);

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new BookCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var genre = await _booksService.CreateBookAsync(model);

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
