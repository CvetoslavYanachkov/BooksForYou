namespace BooksForYou.Services.Data.Books
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Languages;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using BooksForYou.Web.ViewModels.Authors;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class BooksService : IBooksService
    {
        private readonly IDeletableEntityRepository<Book> _bookRepository;
        private readonly IAzureImageService _azureImageService;
        private readonly IGenresService _genresService;
        private readonly IAuthorsService _authorService;
        private readonly ILanguagesService _languagesService;
        private readonly IPublisherService _publishersService;

        public BooksService(
            IDeletableEntityRepository<Book> bookRepository,
            IAzureImageService azureImageService,
            IGenresService genresService,
            IAuthorsService authorService,
            ILanguagesService languagesService,
            IPublisherService publishersService)
        {
            _bookRepository = bookRepository;
            _azureImageService = azureImageService;
            _genresService = genresService;
            _authorService = authorService;
            _languagesService = languagesService;
            _publishersService = publishersService;
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

            if (book != null)
            {
                await _azureImageService.DeleteImageFromAzureAsync(book.ImageUrl);
            }

            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task<T> GetBookByIdAsync<T>(int id)
        {
            var book = await _bookRepository.All().Where(b => b.Id == id).To<T>().FirstOrDefaultAsync();

            return book;
        }

        public async Task<BookEditViewModel> GetBookForEditAsync(int id)
        {
            var book = await _bookRepository.All().Where(b => b.Id == id).FirstOrDefaultAsync();

            var authors = await _authorService.GetAuthorsToCreateAsync();
            var genres = await _genresService.GetGenresToCreateAsync();
            var languages = await _languagesService.GetLanguagesToCreateAsync();
            var publishers = await _publishersService.GetPublishersToCreateAsync();

            return new BookEditViewModel()
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Description = book.Description,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                PublisherId = book.PublisherId,
                Pages = book.Pages,
                LanguageId = book.LanguageId,
                PublisheDate = book.PublisheDate,
                Authors = authors,
                Genres = genres,
                Languages = languages,
                Publishers = publishers
            };
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
                   PublisheDate = b.PublisheDate,
                   ImageUrl = b.ImageUrl
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

        public async Task UpdateBookAsync(int id, BookEditViewModel model)
        {
            var book = await _bookRepository.All().Where(b => b.Id == id).FirstOrDefaultAsync();

            book.Id = model.Id;
            book.ISBN = model.ISBN;
            book.Title = model.Title;
            book.Description = model.Description;
            book.AuthorId = model.AuthorId;
            book.PublisherId = model.PublisherId;
            book.GenreId = model.GenreId;
            book.LanguageId = model.LanguageId;
            book.PublisherId = model.PublisherId;
            book.Pages = model.Pages;
            book.PublisheDate = model.PublisheDate;

            await _bookRepository.SaveChangesAsync();
        }
    }
}
