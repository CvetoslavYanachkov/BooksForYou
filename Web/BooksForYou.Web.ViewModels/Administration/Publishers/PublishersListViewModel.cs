namespace BooksForYou.Web.ViewModels.Administration.Publishers
{
    using System.Collections.Generic;

    public class PublishersListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<PublisherInListViewModel> Publishers { get; set; }
    }
}
