namespace BooksForYou.Web.ViewModels.Genres
{
    using System.ComponentModel.DataAnnotations;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;

    public class GenreEditViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
