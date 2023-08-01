namespace BooksForYou.Services.Data.Books
{
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Http;

    public interface IBooksService
    {
        Task<BooksListViewModel> GetBooksAsync(int pageNumber, int pageSize, BookSorting sorting, string searchTerm = null, string genre = null);

        Task<BooksListViewModel> GetBooksAsync(int pageNumber, int pageSize);

        Task<Book> CreateBookAsync(BookCreateViewModel model, IFormFile file);

        Task UpdateBookAsync(int id, BookEditViewModel model);

        Task<BookEditViewModel> GetBookForEditAsync(int id);

        Task DeleteBookAsync(int id);

        Task<T> GetBookByIdAsync<T>(int id);

        Task<Book> GetBookByIdAsync(int id);

        Task AddBookToMyBooksAsync(string userId, int bookId);

        Task RemoveBookFromMyBooksAsync(int bookId, string userId);

        Task<BooksListViewModel> GetMyBooksAsync(int pageNumber, int pageSize, string userId);
    }
}
