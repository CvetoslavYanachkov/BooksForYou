namespace BooksForYou.Services.Data.Books
{
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Authors;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Http;

    public interface IBooksService
    {
        Task<BooksListViewModel> GetBooksAsync(int pageNumber, int pageSize);

        Task<Book> CreateBookAsync(BookCreateViewModel model, IFormFile file);

        Task UpdateBookAsync(int id, BookEditViewModel model);

        Task<BookEditViewModel> GetBookForEditAsync(int id);

        Task DeleteBookAsync(int id);

        Task<T> GetBookByIdAsync<T>(int id);
    }
}
