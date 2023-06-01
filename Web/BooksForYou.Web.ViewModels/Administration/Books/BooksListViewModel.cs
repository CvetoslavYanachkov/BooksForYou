namespace BooksForYou.Web.ViewModels.Administration.Books
{
    using System.Collections.Generic;

    public class BooksListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<BookInListViewModel> Books { get; set; }
    }
}
