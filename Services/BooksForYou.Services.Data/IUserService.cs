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
        Task<IEnumerable<UserViewModel>> GetUsersAsync();

        Task<ApplicationUser> GetUserById(string id);

        Task<UserEditViewModel> GetUserEditAsync(string id);

        Task UpdateAsync(string id, UserEditViewModel model);
    }
}
