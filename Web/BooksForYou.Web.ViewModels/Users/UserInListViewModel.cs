namespace BooksForYou.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using BooksForYou.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserInListViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
