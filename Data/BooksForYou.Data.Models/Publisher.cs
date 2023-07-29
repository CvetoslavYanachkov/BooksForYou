namespace BooksForYou.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Publisher;

    public class Publisher : BaseDeletableModel<int>
    {
        public Publisher()
        {
            this.Books = new HashSet<Book>();
        }

        [Required]
        [StringLength(MaxPublisherName)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxPublisherDescription)]
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
