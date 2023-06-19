namespace BooksForYou.Services.Data.Authors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Http;

    public interface IAuthorsService
    {
        Task<AuthorsListViewModel> GetAuthorsAsync(int pageNumber, int pageSize);

        Task<Author> CreateAuthorAsync(string userId, AuthorCreateViewModel model, IFormFile file);

        Task<IEnumerable<Author>> GetAuthorsToCreateAsync();

        Task UpdateAuthorAsync(int id, AuthorEditViewModel model);

        Task<AuthorEditViewModel> GetAuthorForEditAsync(int id);

        Task<T> GetAuthorByIdAsync<T>(int id);

        Task DeleteAuthorAsync(int id);

        Task<bool> UserWithWebsiteExists(string website);

        Task<bool> ExistsById(string id);
    }
}
