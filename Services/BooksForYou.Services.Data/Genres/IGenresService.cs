﻿namespace BooksForYou.Services.Data.Genres
{
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Genres;

    public interface IGenresService
    {
        Task<GenresListViewModel> GetGenresAsync(int pageNumber, int pageSize);

        Task<Genre> CreateGenreAsync(GenreInputModel model);

        Task UpdateGenreAsync(int id, GenreEditViewModel model);

        Task<GenreEditViewModel> GetGenreForEditAsync(int id);

        Task<GenreDeleteViewModel> GetGenreForDeleteAsync(int id);

        Task DeleteGenreAsync(int id, GenreDeleteViewModel model);
    }
}
