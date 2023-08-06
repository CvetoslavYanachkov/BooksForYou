namespace BooksForYou.Web.ViewModels.Books
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllBooksQueryModel
    {
        public const int BooksPerPage = 4;

        public string? Genre { get; set; }

        public string? Publisher { get; set; }

        public string? SearchTerm { get; set; }

        public BookSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalRecords { get; set; }

        public IEnumerable<string> Genres { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> Publishers { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<BookInListViewModel> Books { get; set; } = Enumerable.Empty<BookInListViewModel>();
    }
}
