namespace BooksForYou.Services.Data.Authors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Web.ViewModels.Administration.Authors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class AuthorService : IAuthorsService
    {
        private readonly IRepository<Author> _authorRepository;

        private readonly IAzureImageService _azureImageService;

        public AuthorService(
            IRepository<Author> authorRepository,
            IAzureImageService azureImageService)
        {
            _authorRepository = authorRepository;
            _azureImageService = azureImageService;
        }

        public async Task<Author> CreateAuthorAsync(AuthorCreateViewModel model, IFormFile file)
        {
            Uri blobImage = await _azureImageService.UploadImageToAzureAsync(file);
            string image = blobImage.ToString().Replace('"', ' ').Trim();
            var author = new Author()
            {
                Name = model.Name,
                Description = model.Description,
                Born = model.Born,
                Website = model.Website,
                GenreId = model.GenreId,
                ImageUrl = image,
            };

            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveChangesAsync();

            return author;
        }

        public async Task<AuthorsListViewModel> GetAuthorsAsync(int pageNumber, int pageSize)
        {
            var authors = await _authorRepository.All()
                .Select(a => new AuthorInListViewModel()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Born = a.Born,
                    Website = a.Website,
                    Genre = a.Genre.Name,
                    ImageUrl = a.ImageUrl,
                }).ToListAsync();

            var result = new AuthorsListViewModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = authors.Count()
            };

            result.Authors = authors
                .OrderByDescending(x => x.Id)
                .OrderByDescending(x => x.Name)
                .Skip((pageNumber * pageSize) - pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }
    }
}
