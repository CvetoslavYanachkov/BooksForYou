namespace BooksForYou.Web.ViewModels.Books
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Models;

    using static BooksForYou.Data.Common.DataConstants.Book;

    public class BookCreateViewModel
    {
        [Required]
        public string ISBN { get; set; }

        [Required]
        [StringLength(MaxBookTitle, MinimumLength = MinBookTitle)]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxBookDescription, MinimumLength = MinBookDescription)]
        public string Description { get; set; }

        public string UserAuthorId { get; set; }

        public int GenreId { get; set; }

        public int PublisherId { get; set; }

        public int Pages { get; set; }

        public int LanguageId { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublisheDate { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        public IEnumerable<ApplicationUser> UsersAuthors { get; set; } = new List<ApplicationUser>();

        public IEnumerable<Language> Languages { get; set; } = new List<Language>();

        public IEnumerable<Publisher> Publishers { get; set; } = new List<Publisher>();
    }
}
