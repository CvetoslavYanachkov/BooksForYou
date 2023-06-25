﻿namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Author;

    public class Author : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxAuthorDescription)]
        public string Description { get; set; }

        [Required]
        [StringLength(MaxAuthorBorn)]
        public string Born { get; set; }

        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }

        [Required]
        public string Website { get; set; }

        public string ImageUrl { get; set; }
    }
}
