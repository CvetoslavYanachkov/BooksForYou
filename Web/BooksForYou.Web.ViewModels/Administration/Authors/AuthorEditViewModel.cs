namespace BooksForYou.Web.ViewModels.Administration.Authors
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Models;

    using static BooksForYou.Data.Common.DataConstants.Author;

    public class AuthorEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(MaxAuthorName, MinimumLength = MinAuthorName)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxAuthorDescription, MinimumLength = MinAuthorDescription)]
        public string Description { get; set; }

        [Required]
        [StringLength(MaxAuthorBorn, MinimumLength = MinAuthorBorn)]
        public string Born { get; set; }

        [Required]
        public string Website { get; set; }

        public int GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
    }
}
