namespace BooksForYou.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.User;

    public interface IUserService
    {
        Task<UserListViewModel> GetUsersAsync(int pageNumber, int pageSize);

        Task<ApplicationUser> GetUserById(string id);

        Task<UserEditViewModel> GetUserEditAsync(string id);

        Task UpdateUserAsync(string id, UserEditViewModel model);

        Task<UserDeleteViewModel> GetUserForDeleteAsync(string id);

        Task DeleteUserAsync(string id, UserDeleteViewModel model);
    }
}
