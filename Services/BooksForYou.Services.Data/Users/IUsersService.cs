namespace BooksForYou.Services.Data.Users
{
    using System.Threading.Tasks;

    using BooksForYou.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        Task<UserListViewModel> GetUsersAsync(int pageNumber, int pageSize);

        Task<UserEditViewModel> GetUserForEditAsync(string id);

        Task UpdateUserAsync(string id, UserEditViewModel model);

        Task<UserDeleteViewModel> GetUserByIdAsync(string id);

        Task DeleteUserAsync(string id, UserDeleteViewModel model);
    }
}
