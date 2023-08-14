namespace BooksForYou.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BooksForYou.Data;
    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Data.Repositories;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Data.Tests.Common;
    using BooksForYou.Web.ViewModels.Genres;
    using BooksForYou.Web.ViewModels.Publishers;
    using Moq;
    using Xunit;

    public class GenresServiceTests
    {

        private List<Genre> GetTestData()
        {
            return new List<Genre>()
            {
                new Genre()
                {
                     Id = 1,
                     Name = "Test1",
                     Description = "Test1"
                },
                new Genre()
                {
                    Id = 2,
                    Name = "Test2",
                    Description = "Test2"
                }
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task OnCreateGenreAsyncServiceShouldCreateNewGenre()
        {
            string name = "Fantasy";
            string description = "description";

            var testGenreModel = new GenreCreateViewModel()
            {
                Name = name,
                Description = description
            };

            var genreTest = new Genre()
            {
                Name = name,
                Description = description
            };

            var genreMoqRepo = new Mock<IDeletableEntityRepository<Genre>>();
            var service = new GenresService(genreMoqRepo.Object);

            var testGenre = await service.CreateGenreAsync(testGenreModel);

            Assert.NotNull(testGenre);
            Assert.IsAssignableFrom(genreTest.GetType(), testGenre);
        }

        [Fact]
        public void MethodGetGenresAsyncShouldReturnsCorrectCollections()
        {
            IQueryable<Genre> genres = new List<Genre>()
           {
               new Genre
               {
                    Id = 1,
                    Name = "Name",
                    Description = "Description",
               },
               new Genre
               {
                   Id = 2,
                   Name = "Name",
                   Description = "Description",
               }
           }.AsQueryable();

            var repo = new Mock<IDeletableEntityRepository<Genre>>();

            repo.Setup(g => g.All()).Returns(genres);
            var genreService = new GenresService(repo.Object);
            genreService.GetGenresAsync(1, 5);

            Assert.Equal(2, genres.Count());
        }

        [Fact]
        public async Task GetGenresAsyncShouldReturnCollectionOfGenres()
        {
            var context = InMemoryDatabase.GetDbContext();
            await SeedData(context);

            var methodResult = new List<GenresListViewModel>();
            var fromDb = GetTestData();

            var genreRepo = new EfDeletableEntityRepository<Genre>(context);

            var service = new GenresService(genreRepo);

            var actualResult = await service.GetGenresAsync(1, 5);

            Assert.Equal(fromDb.Count, actualResult.Genres.Count);
        }
    }
}
