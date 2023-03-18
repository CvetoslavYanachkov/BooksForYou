namespace BooksForYou.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Common.Models;

    using static BooksForYou.Data.Common.DataConstants.Language;

    public class Language : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(MaxLanguageName)]
        public string Name { get; set; }
    }
}
