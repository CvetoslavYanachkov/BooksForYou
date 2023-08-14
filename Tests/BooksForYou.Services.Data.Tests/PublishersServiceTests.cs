namespace BooksForYou.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using BooksForYou.Data;
    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Data.Repositories;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Data.Tests.Common;
    using BooksForYou.Web.ViewModels.Publishers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyModel.Resolution;
    using Moq;
    using Xunit;

    public class PublishersServiceTests
    {
        private List<Publisher> GetTestData()
        {
            return new List<Publisher>()
            {
                new Publisher()
                {
                     Id = 1,
                     Name = "Test1",
                     Description = "Test1"
                },
                new Publisher()
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
        public async Task CreatePublisherAsyncServiceShouldCreateNewPublisher()
        {
            string name = "Ciela";
            string description = "description";
            var publisherMoqRepo = new Mock<IDeletableEntityRepository<Publisher>>();

            var service = new PublishersService(publisherMoqRepo.Object);

            var testPublisherModel = new PublisherCreateViewModel()
            {
                Name = name,
                Description = description
            };

            var publsherTest = new Publisher()
            {
                Name = name,
                Description = description
            };

            var testPublisher = await service.CreatePublisherAsync(testPublisherModel);

            Assert.NotNull(testPublisher);
            Assert.IsAssignableFrom(publsherTest.GetType(), testPublisher);
        }

        [Fact]
        public async Task GetPublishersAsyncShouldReturnCollectionOfPublishers()
        {
            var context = InMemoryDatabase.GetDbContext();
            await SeedData(context);

            var fromDb = GetTestData();

            var publisherRepo = new EfDeletableEntityRepository<Publisher>(context);

            var service = new PublishersService(publisherRepo);

            var actualResult = await service.GetPublishersAsync(1, 5);

            Assert.Equal(fromDb.Count, actualResult.Publishers.Count);
        }
    }
}
