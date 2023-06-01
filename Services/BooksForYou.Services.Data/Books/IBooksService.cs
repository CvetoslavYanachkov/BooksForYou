namespace BooksForYou.Services.Data.Books
{
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using BooksForYou.Web.ViewModels.Administration.Genres;

    public interface IBooksService
    {
        Task<BooksListViewModel> GetBooksAsync(int pageNumber, int pageSize);

        Task<Book> CreateBookAsync(BookCreateViewModel model);
    }
}
