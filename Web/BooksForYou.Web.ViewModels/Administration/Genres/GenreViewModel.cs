namespace BooksForYou.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
