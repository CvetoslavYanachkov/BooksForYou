namespace BooksForYou.Web.ViewModels.Publishers
{
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;

    public class PublisherDetailsViewModel : IMapFrom<Publisher>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
