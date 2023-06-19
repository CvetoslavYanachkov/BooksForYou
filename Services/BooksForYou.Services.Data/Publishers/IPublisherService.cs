namespace BooksForYou.Services.Data.Publishers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Publishers;

    public interface IPublisherService
    {
        Task<PublishersListViewModel> GetPublishersAsync(int pageNumber, int pageSize);

        Task<Publisher> CreatePublisherAsync(PublisherCreateViewModel model);

        Task<IEnumerable<Publisher>> GetPublishersToCreateAsync();

        Task<T> GetPublisherForEditAsync<T>(int id);

        Task UpdatePublisherAsync(int id, PublisherEditViewModel model);

        Task<T> GetPublisherByIdAsync<T>(int id);

        Task DeletePublisherAsync(int id);
    }
}
