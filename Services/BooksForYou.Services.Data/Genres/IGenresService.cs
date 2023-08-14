namespace BooksForYou.Services.Data.Genres
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Genres;

    public interface IGenresService
    {
        Task<GenresListViewModel> GetGenresAsync(int pageNumber, int pageSize);

        Task<Genre> CreateGenreAsync(GenreCreateViewModel model);

        Task UpdateGenreAsync(int id, GenreEditViewModel model);

        Task<T> GetGenreForEditAsync<T>(int id);

        Task<T> GetGenreByIdAsync<T>(int id);

        Task<List<Genre>> GetGenresToCreateAsync();

        Task<List<string>> GetGenreNamesAsync();
    }
}
