namespace BooksForYou.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
