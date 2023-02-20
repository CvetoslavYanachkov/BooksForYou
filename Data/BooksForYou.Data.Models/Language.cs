namespace BooksForYou.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    public class Language : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
