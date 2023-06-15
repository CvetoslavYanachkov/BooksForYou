namespace BooksForYou.Web.Controllers
{
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Books;
    using BooksForYou.Web.ViewModels.Authors;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BookController : BaseController
    {
        private readonly IBooksService _booksService;

        public BookController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 6)
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
    }
}
