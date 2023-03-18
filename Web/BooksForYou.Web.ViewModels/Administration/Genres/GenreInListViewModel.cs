namespace BooksForYou.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreInListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int BooksCount { get; set; }
    }
}
