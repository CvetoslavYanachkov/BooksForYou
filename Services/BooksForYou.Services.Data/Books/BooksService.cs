namespace BooksForYou.Services.Data.Books
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Languages;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class BooksService : IBooksService
    {
        private readonly IDeletableEntityRepository<Book> _bookRepository;
        private readonly IAzureImageService _azureImageService;
        private readonly IGenresService _genresService;
        private readonly ILanguagesService _languagesService;
        private readonly IPublishersService _publishersService;
        private readonly IUsersService _userService;

        public BooksService(
            IDeletableEntityRepository<Book> bookRepository,
            IAzureImageService azureImageService,
            IGenresService genresService,
            ILanguagesService languagesService,
            IPublishersService publishersService,
            IUsersService userService)
        {
            _bookRepository = bookRepository;
            _azureImageService = azureImageService;
            _genresService = genresService;
            _languagesService = languagesService;
            _publishersService = publishersService;
            _userService = userService;
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
                UserId = model.UserAuthorId,
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

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.AllAsNoTracking().Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<BookEditViewModel> GetBookForEditAsync(int id)
        {
            var book = await _bookRepository.All().Where(b => b.Id == id).FirstOrDefaultAsync();

            var usersAuthors = await _userService.GetUsersAuthorsToCreateAsync();
            var genres = await _genresService.GetGenresToCreateAsync();
            var languages = await _languagesService.GetLanguagesToCreateAsync();
            var publishers = await _publishersService.GetPublishersToCreateAsync();

            return new BookEditViewModel()
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Description = book.Description,
                UserAuthorId = book.UserId,
                GenreId = book.GenreId,
                PublisherId = book.PublisherId,
                Pages = book.Pages,
                LanguageId = book.LanguageId,
                PublisheDate = book.PublisheDate,
                UsersAuthors = usersAuthors,
                Genres = genres,
                Languages = languages,
                Publishers = publishers
            };
        }

        public async Task<BooksQueryModel> GetBooksAsync(BookSorting sorting, string searchTerm = null, string genre = null, string publisher = null, int currentPage = 1, int booksPerPage = 1)
        {
            var result = new BooksQueryModel();
            var booksQuery = _bookRepository.All().AsQueryable();

            if (string.IsNullOrWhiteSpace(genre) == false)
            {
                booksQuery = booksQuery
                    .Where(b => b.Genre.Name == genre);
            }

            if (string.IsNullOrWhiteSpace(publisher) == false)
            {
                booksQuery = booksQuery
                    .Where(b => b.Publisher.Name == publisher);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                booksQuery = booksQuery
                    .Where(b => EF.Functions.Like(b.Title.ToLower(), searchTerm) ||
                    EF.Functions.Like(b.User.FirstName.ToLower(), searchTerm) ||
                    EF.Functions.Like(b.User.LastName.ToLower(), searchTerm));
            }

            booksQuery = sorting switch
            {
                BookSorting.ISBN => booksQuery
                .OrderBy(b => b.ISBN),
                BookSorting.PublishDate => booksQuery
                .OrderBy(b => b.PublisheDate),
                BookSorting.Pages => booksQuery
                .OrderBy(b => b.Pages),
                _ => booksQuery.OrderByDescending(b => b.Id)
            };

            result.Books = await booksQuery
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage)
               .Select(b => new BookInListViewModel()
               {
                   Id = b.Id,
                   Title = b.Title,
                   UserAuthor = b.User.FirstName + " " + b.User.LastName,
                   Genre = b.Genre.Name,
                   ImageUrl = b.ImageUrl
               })
                .ToListAsync();

            result.TotalBooksRecords = await booksQuery.CountAsync();

            return result;
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
            UserAuthor = b.User.FirstName + " " + b.User.LastName,
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
            book.UserId = model.UserAuthorId;
            book.PublisherId = model.PublisherId;
            book.GenreId = model.GenreId;
            book.LanguageId = model.LanguageId;
            book.PublisherId = model.PublisherId;
            book.Pages = model.Pages;
            book.PublisheDate = model.PublisheDate;

            await _bookRepository.SaveChangesAsync();
        }

        public async Task AddBookToMyBooksAsync(string userId, int bookId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var book = await _bookRepository.AllAsNoTracking().Where(b => b.Id == bookId).FirstOrDefaultAsync();

            if (book == null)
            {
                throw new ArgumentException("Invalid Movie ID");
            }

            if (!user.UsersBooks.Any(x => x.BookId == bookId))
            {
                user.UsersBooks.Add(new UserBook()
                {
                    BookId = book.Id,
                    UserId = user.Id,
                    Book = book,
                    User = user
                });

                await _userService.UpdateUserAsync(user);
            }
        }

        public async Task RemoveBookFromMyBooksAsync(int bookId, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }



            var book = user.UsersBooks.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                user.UsersBooks.Remove(book);

                await _userService.UpdateUserAsync(user);
            }
        }

        public async Task<BooksListViewModel> GetMyBooksAsync(int pageNumber, int pageSize, string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var books = _bookRepository.AllAsNoTracking().Where(b => b.UsersBooks.Any(x => x.UserId == userId))
                .Select(b => new BookInListViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    UserAuthor = b.User.FirstName + " " + b.User.LastName,
                    Genre = b.Genre.Name,
                    ImageUrl = b.ImageUrl
                }).ToList();

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
