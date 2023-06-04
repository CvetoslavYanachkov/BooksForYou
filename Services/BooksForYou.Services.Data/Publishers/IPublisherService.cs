namespace BooksForYou.Services.Data.Publishers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Authors;
    using BooksForYou.Web.ViewModels.Administration.Publisher;
    using BooksForYou.Web.ViewModels.Administration.Publishers;

    public interface IPublisherService
    {
        Task<PublishersListViewModel> GetPublishersAsync(int pageNumber, int pageSize);

        Task<Publisher> CreatePublisherAsync(PublisherCreateViewModel model);

        Task<IEnumerable<Publisher>> GetPublishersToCreateAsync();

        Task<PublisherEditViewModel> GetPublisherForEditAsync(int id);

        Task UpdatePublisherAsync(int id, PublisherEditViewModel model);

        Task DeletePublisherAsync(int id);
    }
}
