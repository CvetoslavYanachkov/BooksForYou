namespace BooksForYou.Services.Data.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<UsersListViewModel> GetUsersAsync(int pageNumber, int pageSize);

        Task<UserEditViewModel> GetUserForEditAsync(string id);

        Task UpdateUserAsync(string id, UserEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<string> GetNameOfUser(string id);

        Task<bool> UserWithWebsiteExists(string website);

        Task<bool> ExistsById(string id);

        Task<UsersAuthorsListViewModel> GetUsersAuthorsAsync(int pageNumber, int pageSize);

        Task UserBecomeAuthorAsync(string id, UserBecomesAuthorViewModel model, IFormFile file);

        Task<UserBecomesAuthorViewModel> GetUserBecomeAuthorAsync(string id);

        Task<IEnumerable<ApplicationUser>> GetUsersAuthorsToCreateAsync();

        Task<T> GetUserAuthorByIdAsync<T>(string id);
    }
}
