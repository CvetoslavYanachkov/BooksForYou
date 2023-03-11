namespace BooksForYou.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Web.ViewModels.Administration.User;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepo;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            _userRepo = userRepo;
        }

        public Task<ApplicationUser> GetById(string Id)
        {
            var user = _userRepo.All().FirstOrDefaultAsync(x => x.Id == Id);

            return user;
        }

        public async Task<UserViewModel> GetUserEditAsync(string id)
        {
            var user = await _userRepo.All().FirstOrDefaultAsync(x => x.Id == id);

            return new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
        {
            var users = await _userRepo.All().ToListAsync();

            return users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            });
        }
    }
}
