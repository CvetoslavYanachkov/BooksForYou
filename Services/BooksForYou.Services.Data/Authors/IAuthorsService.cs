namespace BooksForYou.Services.Data.Authors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Authors;
    using BooksForYou.Web.ViewModels.Administration.Genres;
    using Microsoft.AspNetCore.Http;

    public interface IAuthorsService
    {
        Task<AuthorsListViewModel> GetAuthorsAsync(int pageNumber, int pageSize);

        Task<Author> CreateAuthorAsync(AuthorCreateViewModel model, IFormFile file);

        Task<IEnumerable<Author>> GetAuthorsToCreateAsync();

        Task UpdateAuthorAsync(int id, AuthorEditViewModel model);

        Task<AuthorEditViewModel> GetAuthorForEditAsync(int id);

        Task DeleteAuthorAsync(int id);
    }
}
