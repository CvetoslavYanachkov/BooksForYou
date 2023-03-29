﻿namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Genre;

    public class Genre : BaseDeletableModel<int>
    {
        [MaxLength(MaxGenreName)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxGenreDescription)]
        public string Description { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
