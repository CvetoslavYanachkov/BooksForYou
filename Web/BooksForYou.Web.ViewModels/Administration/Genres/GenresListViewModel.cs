namespace BooksForYou.Web.ViewModels.Administration.Genres
{
    using System.Collections.Generic;

    public class GenresListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<GenreInListViewModel> Genres { get; set; }
    }
}
