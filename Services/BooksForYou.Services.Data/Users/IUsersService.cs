namespace BooksForYou.Services.Data.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<UserListViewModel> GetUsersAsync(int pageNumber, int pageSize);

        Task<UserEditViewModel> GetUserForEditAsync(string id);

        Task UpdateUserAsync(string id, UserEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task DeleteUserAsync(string id, UserDeleteViewModel model);

        Task<string> GetNameOfUser(string id);

        Task<bool> UserWithWebsiteExists(string website);

        Task<bool> ExistsById(string id);

        IEnumerable<T> GetUsers<T>();
    }
}
