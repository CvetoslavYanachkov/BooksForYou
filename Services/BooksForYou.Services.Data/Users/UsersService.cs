namespace BooksForYou.Services.Data.Users;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BooksForYou.Common;
using BooksForYou.Data.Common.Repositories;
using BooksForYou.Data.Models;
using BooksForYou.Services.AzureServices;
using BooksForYou.Services.Data.Books;
using BooksForYou.Services.Data.Genres;
using BooksForYou.Services.Mapping;
using BooksForYou.Services.Messaging;
using BooksForYou.Web.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

public class UsersService : IUsersService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGenresService _genresService;
    private readonly IAzureImageService _azureImageService;
    private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;

    public UsersService(
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IGenresService genresService,
        IAzureImageService azureImageService,
        IDeletableEntityRepository<ApplicationUser> userRepository)
    {
        _signInManager = signInManager;
        _emailSender = emailSender;
        _userManager = userManager;
        _roleManager = roleManager;
        _genresService = genresService;
        _azureImageService = azureImageService;
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.All()
            .Where(u => u.Id == id)
            .Include(u => u.UsersBooks)
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<string> GetNameOfUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var nameofUser = user.FirstName + " " + user.LastName;

        return nameofUser;
    }

    public async Task<UserEditViewModel> GetUserForEditAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var roles = await _roleManager.Roles.ToListAsync();

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

    public async Task<UsersListViewModel> GetUsersAsync(int pageNumber, int pageSize)
    {
        var users = await _userManager.Users.
        Select(x => new UserInListViewModel()
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
        }).ToListAsync();

        var result = new UsersListViewModel()
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
        var user = await _userManager.FindByIdAsync(id);

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

            if (rolesToAdd.Contains(GlobalConstants.AuthorRoleName))
            {
                var html = new StringBuilder();
                html.AppendLine($"<h1>{"Congratulations!"}</h1>");
                html.AppendLine($"<h3>{"You are already Author. Please signIn and fill the author form."}</h3>");
                await _emailSender.SendEmailAsync("cyanachkov@gmail.com", "Books For You!", GlobalConstants.AdminMail, "Author", html.ToString());
            }
        }

        if (rolesToRemove.Any())
        {
            await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> UserWithWebsiteExists(string website)
    {
        return await _userManager.Users.AnyAsync(u => u.Website == website);
    }

    public async Task<bool> ExistsById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return await _userManager.IsInRoleAsync(user, GlobalConstants.AuthorRoleName);
    }

    public async Task<UsersAuthorsListViewModel> GetUsersAuthorsAsync(int pageNumber, int pageSize)
    {
        var roleAuthor = await _roleManager.FindByNameAsync(GlobalConstants.AuthorRoleName);

        var usersAuthors = await _userManager.Users
            .Include(x => x.Genre)
            .ThenInclude(x => x.Books)
            .Where(x => x.Roles.Any(x => x.RoleId == roleAuthor.Id))
            .Select(x => new UserAuthorInListViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Description = x.Description,
                Born = x.Born,
                Genre = x.Genre.Name,
                ImageUrl = x.ImageUrl,
                Website = x.Website
            }).ToListAsync();

        var result = new UsersAuthorsListViewModel()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = usersAuthors.Count()
        };

        result.Users = usersAuthors
            .OrderBy(x => x.FirstName)
            .OrderByDescending(x => x.LastName)
            .Skip((pageNumber * pageSize) - pageSize)
            .Take(pageSize)
            .ToList();

        return result;
    }

    public async Task<UserBecomesAuthorViewModel> GetUserBecomeAuthorAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        var genres = await _genresService.GetGenresToCreateAsync();
        return new UserBecomesAuthorViewModel()
        {
            Id = id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Description = user.Description,
            Born = user.Born,
            GenreId = user.GenreId,
            Genres = genres,
            Website = user.Website
        };
    }

    public async Task UserBecomeAuthorAsync(string id, UserBecomesAuthorViewModel model, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(id);
        string nameOfUserAuthor = user.FirstName + " " + user.LastName;

        string imageName = nameOfUserAuthor.ToString().Replace(' ', '-').Trim(' ');
        Uri blobImage = await _azureImageService.UploadImageToAzureAsync(file, imageName);
        string image = blobImage.ToString().Replace('"', ' ').Trim();

        user.Id = model.Id;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Description = model.Description;
        user.Born = model.Born;
        user.Website = model.Website;
        user.ImageUrl = image;
        user.GenreId = model.GenreId;

        await _userManager.UpdateAsync(user);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersAuthorsToCreateAsync()
    {
        var roleAuthor = await _roleManager.FindByNameAsync(GlobalConstants.AuthorRoleName);

        var usersAuthors = await _userManager.Users.
            Include(x => x.Genre)
            .ThenInclude(x => x.Books)
            .Where(x => x.Roles.Any(x => x.RoleId == roleAuthor.Id))
            .ToListAsync();

        return usersAuthors;
    }

    public async Task<T> GetUserAuthorByIdAsync<T>(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var roleAuthor = await _roleManager.FindByNameAsync(GlobalConstants.AuthorRoleName);

        var userAuthor = await _userManager.Users
            .Where(x => x.Roles.Any(x => x.RoleId == roleAuthor.Id) && x.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();
        return userAuthor;
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }
}
