﻿namespace BooksForYou.Web.ViewModels.Administration.User
{
    using System.Collections.Generic;

    using BooksForYou.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
