namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    public class Author : BaseDeletableModel<int>
    {
        public Author()
        {
            this.Genres = new List<Genre>();
            this.Books = new List<Book>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(3000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Born { get; set; }

        [Required]
        public string Website { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
