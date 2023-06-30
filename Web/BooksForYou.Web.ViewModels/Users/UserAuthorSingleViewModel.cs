namespace BooksForYou.Web.ViewModels.Users;

using System;

using BooksForYou.Data.Models;
using BooksForYou.Services.Mapping;

public class UserAuthorSingleViewModel : IMapFrom<ApplicationUser>
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Description { get; set; }

    public string Born { get; set; }

    public string Website { get; set; }

    public string GenreName { get; set; }

    public string ImageUrl { get; set; }
}
