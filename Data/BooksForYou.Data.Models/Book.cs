namespace BooksForYou.Data.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Book;

    public class Book : BaseDeletableModel<int>
    {
        public Book()
        {
            Votes = new HashSet<Vote>();
        }

        [Required]
        public string ISBN { get; set; }

        [Required]
        [StringLength(MaxBookTitle)]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxBookDescription)]
        public string Description { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser UserAuthor { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int PublisherId { get; set; }

        [ForeignKey(nameof(PublisherId))]
        public Publisher Publisher { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }

        [Required]
        [DataType(nameof(DataType))]
        public DateTime PublisheDate { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
