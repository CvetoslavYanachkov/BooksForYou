namespace BooksForYou.Services.Data.Books
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class BooksService : IBooksService
    {
        private readonly IDeletableEntityRepository<Book> _bookRepository;
        private readonly IAzureImageService _azureImageService;

        public BooksService(
            IDeletableEntityRepository<Book> bookRepository,
            IAzureImageService azureImageService)
        {
            _bookRepository = bookRepository;
            _azureImageService = azureImageService;
        }

        public async Task<Book> CreateBookAsync(BookCreateViewModel model, IFormFile file)
        {
            string imageName = model.Title.ToString().Replace(' ', '-').Trim(' ');
            Uri blobImage = await _azureImageService.UploadImageToAzureAsync(file, imageName);
            string imageUrl = blobImage.ToString().Replace('"', ' ').Trim();
            var book = new Book()
            {
                ISBN = model.ISBN,
                Title = model.Title,
                Description = model.Description,
                AuthorId = model.AuthorId,
                GenreId = model.GenreId,
                PublisherId = model.PublisherId,
                Pages = model.Pages,
                ImageUrl = imageUrl,
                LanguageId = model.LanguageId,
                PublisheDate = model.PublisheDate
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.All().Where(b => b.Id == id).FirstOrDefaultAsync();

            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task<BooksListViewModel> GetBooksAsync(int pageNumber, int pageSize)
        {
            var books = await _bookRepository.All()
                .Select(b => new BookInListViewModel()
                {
                    Id = b.Id,
                    ISBN = b.ISBN,
                    Title = b.Title,
                    Description = b.Description,
                    Author = b.Author.Name,
                    Publisher = b.Publisher.Name,
                    Genre = b.Genre.Name,
                    Language = b.Language.Name,
                    Pages = b.Pages,
                    ImageUrl = b.ImageUrl,
                    PublisheDate = b.PublisheDate,
                })
                .ToListAsync();

            var result = new BooksListViewModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = books.Count()
            };

            result.Books = books
                .OrderByDescending(x => x.Id)
                .OrderByDescending(x => x.Title)
                .Skip((pageNumber * pageSize) - pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }
    }
}
