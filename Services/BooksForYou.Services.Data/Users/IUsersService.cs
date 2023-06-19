namespace BooksForYou.Services.Data.Users
{
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
    }
}
