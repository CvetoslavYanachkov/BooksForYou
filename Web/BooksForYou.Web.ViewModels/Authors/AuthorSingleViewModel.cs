namespace BooksForYou.Web.ViewModels.Authors;

using System;

using BooksForYou.Data.Models;
using BooksForYou.Services.Mapping;

public class AuthorSingleViewModel : IMapFrom<Author>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Born { get; set; }

    public string Website { get; set; }

    public string GenreName { get; set; }

    public string ImageUrl { get; set; }
}
