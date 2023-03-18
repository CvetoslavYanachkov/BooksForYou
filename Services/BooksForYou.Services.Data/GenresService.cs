namespace BooksForYou.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Genres;
    using Microsoft.EntityFrameworkCore;

    public class GenresService : IGenresService
    {
        private readonly IDeletableEntityRepository<Genre> _genreRepo;

        public GenresService(IDeletableEntityRepository<Genre> genreRepo)
        {
            _genreRepo = genreRepo;
        }

        public async Task<Genre> CreateGenreAsync(GenreViewModel model)
        {
            var genre = new Genre()
            {
                Name = model.Name,
                Description = model.Description
            };

            await _genreRepo.AddAsync(genre);
            await _genreRepo.SaveChangesAsync();

            return genre;
        }

        public async Task<GenreEditViewModel> GetGenreForEditAsync(int id)
        {
            var genre = await _genreRepo.All().Where(g => g.Id == id).FirstOrDefaultAsync();

            return new GenreEditViewModel()
            {
                Id = id,
                Name = genre.Name,
                Description = genre.Description
            };
        }

        public async Task<GenresListViewModel> GetGenresAsync(int pageNumber, int pageSize)
        {
            var genres = await _genreRepo.All()
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
            var genre = await _genreRepo.All().Where(g => g.Id == id).FirstOrDefaultAsync();

            genre.Name = model.Name;
            genre.Description = model.Description;

            await _genreRepo.SaveChangesAsync();
        }
    }
}
