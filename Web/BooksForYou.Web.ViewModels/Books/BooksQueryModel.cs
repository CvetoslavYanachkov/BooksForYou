namespace BooksForYou.Web.ViewModels.Books
{
    using System.Collections.Generic;

    public class BooksQueryModel
    {
        public int TotalBooksRecords { get; set; }

        public List<BookInListViewModel> Books { get; set; }
    }
}
