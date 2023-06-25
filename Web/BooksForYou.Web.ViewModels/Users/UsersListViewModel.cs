namespace BooksForYou.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UsersListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<UserInListViewModel> Users { get; set; }
    }
}
