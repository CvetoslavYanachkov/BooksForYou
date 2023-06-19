namespace BooksForYou.Web.ViewModels.Authors
{
    using System.Collections.Generic;

    public class AuthorsListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<AuthorInListViewModel> Authors { get; set; }
    }
}
