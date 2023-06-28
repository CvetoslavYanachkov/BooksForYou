namespace BooksForYou.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    using BooksForYou.Data.Models;

    using static BooksForYou.Data.Common.DataConstants.ApplicationUser;

    public class UserAuthorEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(MaxUserFirstname, MinimumLength = MinUserFirstname)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(MaxUserLastname, MinimumLength = MinUserLastName)]
        public string LastName { get; set; }

        [Required]
        [StringLength(MaxUserDescription, MinimumLength = MinUserDescription)]
        public string Description { get; set; }

        [Required]
        [StringLength(MaxUserBorn, MinimumLength = MinUserBorn)]
        public string Born { get; set; }

        public int? GenreId { get; set; }

        [Required]
        public string Website { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
    }
}
