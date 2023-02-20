namespace BooksForYou.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    public class Genre : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
