﻿namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Author;

    public class Author : BaseDeletableModel<int>
    {
        public Author()
        {
            this.Books = new List<Book>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxAuthorDescription)]
        public string Description { get; set; }

        [Required]
        [StringLength(MaxAuthorBorn)]
        public string Born { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }

        [Required]
        public string Website { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
