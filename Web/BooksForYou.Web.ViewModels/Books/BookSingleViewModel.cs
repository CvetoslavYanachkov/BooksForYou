namespace BooksForYou.Web.ViewModels.Books
{
    using System;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;

    public class BookSingleViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public string UserAuthorFirstName { get; set; }

        public string UserAuthorLastName { get; set; }

        public string GenreName { get; set; }

        public string PublisherName { get; set; }

        public int Pages { get; set; }

        public string LanguageName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublisheDate { get; set; }
    }
}
