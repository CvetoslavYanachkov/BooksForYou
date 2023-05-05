namespace BooksForYou.Services.Data;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BooksForYou.Data.Common.Repositories;
using BooksForYou.Data.Models;
using BooksForYou.Services.Messaging;
using BooksForYou.Web.ViewModels.Administration.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class UsersService : IUsersService
{
    private readonly IDeletableEntityRepository<ApplicationUser> _userRepo;
    private readonly IDeletableEntityRepository<ApplicationRole> _roleRepo;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public UsersService(
        IDeletableEntityRepository<ApplicationUser> userRepo,
        IDeletableEntityRepository<ApplicationRole> roleRepo,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
    {
        _userRepo = userRepo;
        _signInManager = signInManager;
        _roleRepo = roleRepo;
        _emailSender = emailSender;
    }

    public async Task<UserDeleteViewModel> GetUserForDeleteAsync(string id)
    {
        var user = await _userRepo.All().Where(u => u.Id == id).FirstOrDefaultAsync();

        return new UserDeleteViewModel()
        {
            Id = id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task DeleteUserAsync(string id, UserDeleteViewModel model)
    {
        var user = await _userRepo.All().Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user != null)
        {
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
        }

        _userRepo.Delete(user);
        await _userRepo.SaveChangesAsync();
    }

    public async Task<UserEditViewModel> GetUserForEditAsync(string id)
    {
        var user = await _userRepo.All().Where(x => x.Id == id).FirstOrDefaultAsync();

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

    public async Task<UserListViewModel> GetUsersAsync(int pageNumber, int pageSize)
    {
        var users = await _userRepo.All().
       Select(x => new UserViewModel()
       {
           Id = x.Id,
           FirstName = x.FirstName,
           LastName = x.LastName,
           Email = x.Email,
       }).ToListAsync();

        var result = new UserListViewModel()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = users.Count()
        };

        result.Users = users
            .OrderByDescending(x => x.FirstName)
            .ThenByDescending(x => x.LastName)
            .Skip((pageNumber * pageSize) - pageSize)
            .Take(pageSize)
            .ToList();

        return result;
    }

    public async Task UpdateUserAsync(string id, UserEditViewModel model)
    {
        var user = await _userRepo.All().Where(u => u.Id == id).FirstOrDefaultAsync();

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

        var rolesAlreadyExist = await _signInManager.UserManager.GetRolesAsync(user);

        if (rolesToAdd.Any())
        {
            await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToAdd.Contains("Author"))
            {
                var html = new StringBuilder();
                html.AppendLine($"<h1>{"Congratulations!"}</h1>");
                html.AppendLine($"<h3>{"You are already Author. Please go in your profile and fill in the author form."}</h3>");
                await _emailSender.SendEmailAsync("cyanachkov@gmail.com", "Books For You!", "ceno1902@gmail.com", "Author", html.ToString());
            }

            if (rolesToAdd.Contains("Publisher"))
            {
                var html = new StringBuilder();
                html.AppendLine($"<h1>{"Congratulations!"}</h1>");
                html.AppendLine($"<h3>{"You are already Publisher. Please go in your profile and fill in the publisher form."}</h3>");
                await _emailSender.SendEmailAsync("cyanachkov@gmail.com", "Books For You!", "ceno1902@gmail.com", "Publisher", html.ToString());
            }
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
