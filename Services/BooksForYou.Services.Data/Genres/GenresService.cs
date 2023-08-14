namespace BooksForYou.Services.Data.Genres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Web.ViewModels.Genres;
    using Microsoft.EntityFrameworkCore;

    public class GenresService : IGenresService
    {
        private readonly IDeletableEntityRepository<Genre> _genreRepository;

        public GenresService(IDeletableEntityRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<Genre> CreateGenreAsync(GenreCreateViewModel model)
        {
            var genre = new Genre()
            {
                Name = model.Name,
                Description = model.Description
            };

            await _genreRepository.AddAsync(genre);
            await _genreRepository.SaveChangesAsync();

            return genre;
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _genreRepository.All().Where(g => g.Id == id).FirstOrDefaultAsync();

            _genreRepository.Delete(genre);
            await _genreRepository.SaveChangesAsync();
        }

        public async Task<T> GetGenreByIdAsync<T>(int id)
        {
            var genre = await _genreRepository.All().Where(g => g.Id == id).To<T>().FirstOrDefaultAsync();

            return genre;
        }

        public async Task<T> GetGenreForEditAsync<T>(int id)
        {
            var genre = await _genreRepository.All().Where(g => g.Id == id).To<T>().FirstOrDefaultAsync();

            return genre;
        }

        public async Task<GenresListViewModel> GetGenresAsync(int pageNumber, int pageSize)
        {
            var genres = await _genreRepository.All()
               .Select(x => new GenreInListViewModel()
               {
                   Id = x.Id,
                   Name = x.Name,
                   Description = x.Description,
                   BooksCount = x.Books.Count()
               }).ToListAsync();

            var result = new GenresListViewModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = genres.Count()
            };

            result.Genres = genres
                .OrderByDescending(x => x.Id)
                .OrderByDescending(x => x.Name)
                .Skip((pageNumber * pageSize) - pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }

        public async Task UpdateGenreAsync(int id, GenreEditViewModel model)
        {
            var genre = await _genreRepository.All().Where(g => g.Id == id).FirstOrDefaultAsync();

            genre.Name = model.Name;
            genre.Description = model.Description;

            await _genreRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenresToCreateAsync()
        {
            return await _genreRepository.All().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetGenreNamesAsync()
        {
            return await _genreRepository.AllAsNoTracking()
                .Select(g => g.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
