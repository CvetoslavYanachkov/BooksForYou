namespace BooksForYou.Services.Data.Authors
{
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Authors;
    using Microsoft.AspNetCore.Http;

    public interface IAuthorsService
    {
        Task<AuthorsListViewModel> GetAuthorsAsync(int pageNumber, int pageSize);

        Task<Author> CreateAuthorAsync(AuthorCreateViewModel model, IFormFile file);
    }
}
