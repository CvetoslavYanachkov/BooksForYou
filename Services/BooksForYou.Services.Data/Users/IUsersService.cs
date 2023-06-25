namespace BooksForYou.Services.Data.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Authors;
    using BooksForYou.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<UsersListViewModel> GetUsersAsync(int pageNumber, int pageSize);

        Task<UserEditViewModel> GetUserForEditAsync(string id);

        Task UpdateUserAsync(string id, UserEditViewModel model);

        Task UpdateUserAuthorAsync(string id, UserAuthorEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<string> GetNameOfUser(string id);

        Task<bool> UserWithWebsiteExists(string website);

        Task<bool> ExistsById(string id);

        Task<UsersAuthorsListViewModel> GetUsersWithRoleAuthorAsync(int pageNumber, int pageSize);

        Task<IEnumerable<ApplicationUser>> GetUsersWithRoleAuthorAsync();

        Task UserBecomeAuthorAsync(string id, UserBecomesAuthorViewModel model, IFormFile file);
    }
}
