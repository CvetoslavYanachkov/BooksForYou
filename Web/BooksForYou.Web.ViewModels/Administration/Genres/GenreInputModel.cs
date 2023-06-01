namespace BooksForYou.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
