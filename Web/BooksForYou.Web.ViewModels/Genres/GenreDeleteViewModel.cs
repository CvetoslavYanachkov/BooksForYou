namespace BooksForYou.Web.ViewModels.Genres
{
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;

    public class GenreDeleteViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
