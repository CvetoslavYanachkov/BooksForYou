namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    public class Publisher : BaseDeletableModel<int>
    {
        public Publisher()
        {
            this.Books = new List<Book>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(3000)]
        public string Description { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
