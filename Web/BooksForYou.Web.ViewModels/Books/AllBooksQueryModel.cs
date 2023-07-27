namespace BooksForYou.Web.ViewModels.Books
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllBooksQueryModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int p { get; set; } = 1;

        public int s { get; set; } = 6;

        public string? Genre { get; set; }

        public string? SearchTerm { get; set; }

        public BookSorting Sorting { get; set; }

        public IEnumerable<string> Genres { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<BookInListViewModel> Books { get; set; } = Enumerable.Empty<BookInListViewModel>();
    }
}
