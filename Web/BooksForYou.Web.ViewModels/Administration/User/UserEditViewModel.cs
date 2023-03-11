namespace BooksForYou.Web.ViewModels.Administration.User
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class UserEditViewModel
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public IList<SelectListItem> Roles { get; set; }
    }
}
