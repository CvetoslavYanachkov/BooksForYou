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

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string GenreName { get; set; }

        public string PublisherName { get; set; }

        public int Pages { get; set; }

        public string LanguageName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublisheDate { get; set; }
    }
}
