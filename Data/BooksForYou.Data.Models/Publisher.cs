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
            this.Books = new List<Book>();
        }

        [Required]
        [StringLength(MaxPublisherName)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxPublisherDescription)]
        public string Description { get; set; }

        [Required]
        [StringLength(PublisherPhoneNumber)]
        public string PhoneNumber { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
