namespace BooksForYou.Web.ViewModels.Administration.User
{
    using System.Collections.Generic;

    using BooksForYou.Data.Models;

    public class UserListViewModel
    {
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public List<UserViewModel> Users { get; set; }
    }
}
