namespace BooksForYou.Services.Data.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Services.Messaging;
    using BooksForYou.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AuthorService : IAuthorsService
    {
        private readonly IDeletableEntityRepository<Author> _authorRepository;

        private readonly IAzureImageService _azureImageService;

        private readonly IGenresService _genresService;

        private readonly IUsersService _usersService;

        public AuthorService(
          IDeletableEntityRepository<Author> authorRepository,
          IAzureImageService azureImageService,
          IGenresService genresService,
          IEmailSender emailSender,
          IUsersService usersService)
        {
            _authorRepository = authorRepository;
            _azureImageService = azureImageService;
            _genresService = genresService;
            _usersService = usersService;
        }

        public async Task<bool> ExistsById(string id)
        {
            return await _authorRepository.All()
                .AnyAsync(a => a.UserId == id);
        }

        public async Task<bool> UserWithWebsiteExists(string website)
        {
            return await _authorRepository.All()
                .AnyAsync(a => a.Website == website);
        }

        public async Task<Author> CreateAuthorAsync(string userId, AuthorCreateViewModel model, IFormFile file)
        {
            var user = await _usersService.GetUserByIdAsync(userId);
            string nameOfUser = user.FirstName + " " + user.LastName;

            string imageName = nameOfUser.ToString().Replace(' ', '-').Trim(' ');
            Uri blobImage = await _azureImageService.UploadImageToAzureAsync(file, imageName);
            string image = blobImage.ToString().Replace('"', ' ').Trim();
            var author = new Author()
            {
                Name = nameOfUser,
                Description = model.Description,
                Born = model.Born,
                Website = model.Website,
                GenreId = model.GenreId,
                ImageUrl = image,
                UserId = userId,
            };

            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveChangesAsync();

            return author;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _authorRepository.All().Where(a => a.Id == id).FirstOrDefaultAsync();

            if (author != null)
            {
                await _azureImageService.DeleteImageFromAzureAsync(author.ImageUrl);
            }

            _authorRepository.Delete(author);
            await _authorRepository.SaveChangesAsync();
        }

        public async Task<AuthorEditViewModel> GetAuthorForEditAsync(int id)
        {
            var author = await _authorRepository.All().Where(a => a.Id == id).FirstOrDefaultAsync();
            var genres = await _genresService.GetGenresToCreateAsync();

            return new AuthorEditViewModel()
            {
                Id = id,
                Description = author.Description,
                Born = author.Born,
                GenreId = author.GenreId,
                Genres = genres
            };
        }

        public async Task<AuthorsListViewModel> GetAuthorsAsync(int pageNumber, int pageSize)
        {
            var authors = await _authorRepository.All()
                .Select(a => new AuthorInListViewModel()
                {
                    Id = a.Id,
                    Name = a.User.FirstName + " " + a.User.LastName,
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

        public async Task<IEnumerable<Author>> GetAuthorsToCreateAsync()
        {
            return await _authorRepository.All().ToListAsync();
        }

        public async Task<T> GetAuthorByIdAsync<T>(int id)
        {
            var author = await _authorRepository.All().Where(g => g.Id == id).To<T>().FirstOrDefaultAsync();

            return author;
        }

        public async Task UpdateAuthorAsync(int id, AuthorEditViewModel model)
        {
            var author = await _authorRepository.All().Where(a => a.Id == id).FirstOrDefaultAsync();

            author.Id = id;
            author.Description = model.Description;
            author.Born = model.Born;
            author.GenreId = model.GenreId;

            await _authorRepository.SaveChangesAsync();
        }
    }
}
