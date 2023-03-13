namespace BooksForYou.Services.Data;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BooksForYou.Data.Common.Repositories;
using BooksForYou.Data.Models;
using BooksForYou.Web.ViewModels.Administration.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly IDeletableEntityRepository<ApplicationUser> _userRepo;
    private readonly IDeletableEntityRepository<ApplicationRole> _roleRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(
        IDeletableEntityRepository<ApplicationUser> userRepo,
        IDeletableEntityRepository<ApplicationRole> roleRepo,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)

    {
        _userRepo = userRepo;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleRepo = roleRepo;
    }

    public async Task<ApplicationUser> GetUserById(string id)
    {
        var user = await _userRepo.All().Where(u => u.Id == id).FirstOrDefaultAsync();

        return user;
    }

    public async Task<UserEditViewModel> GetUserEditAsync(string id)
    {
        var user = await _userRepo.All().FirstOrDefaultAsync(x => x.Id == id);

        var roles = await _roleRepo.All().ToListAsync();

        var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

        var roleitems = roles.Select(role =>
        new SelectListItem(
            role.Name,
            role.Id,
            userRoles.Any(ur => ur.Contains(role.Name)))).ToList();


        return new UserEditViewModel()
        {
            Id = id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roleitems
        };
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
    {
        var users = await _userRepo.All().
            Select(x => new UserViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
            }).ToListAsync();
        return users;
    }

    public async Task UpdateAsync(string id, UserEditViewModel model)
    {
        var user = await _userRepo.All().FirstOrDefaultAsync(u => u.Id == id);

        var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

        var rolesToAdd = new List<string>();
        var rolesToRemove = new List<string>();

        foreach (var role in model.Roles)
        {
            var assigned = userRoles.FirstOrDefault(ur => ur == role.Text);
            if (role.Selected)
            {
                if (assigned == null)
                {
                    rolesToAdd.Add(role.Text);
                }
            }
            else
            {
                if (assigned != null)
                {
                    rolesToRemove.Add(role.Text);
                }
            }
        }

        if (rolesToAdd.Any())
        {
            await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
        }

        if (rolesToRemove.Any())
        {
            await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        await _userRepo.SaveChangesAsync();
    }
}
