namespace BooksForYou.Services.Data.Books
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Web.ViewModels.Administration.Books;
    using Microsoft.EntityFrameworkCore;

    public class BooksService : IBooksService
    {
        private readonly IDeletableEntityRepository<Book> _bookRepository;

        public BooksService(
            IDeletableEntityRepository<Book> bookRepository,
            IDeletableEntityRepository<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> CreateBookAsync(BookCreateViewModel model)
        {
            var book = new Book()
            {
                Title = model.Title,
                Description = model.Description,
                AuthorId = model.AuthorId,
                GenreId = model.GenreId,
                PublisherId = model.PublisherId,
                Pages = model.Pages,
                ImageUrl = model.ImageUrl,
                PublisheDate = model.PublisheDate
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
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
