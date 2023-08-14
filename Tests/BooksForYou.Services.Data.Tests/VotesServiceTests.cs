namespace BooksForYou.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data;
    using BooksForYou.Data.Models;
    using BooksForYou.Data.Repositories;
    using BooksForYou.Services.Data.Votes;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class VotesServiceTests
    {
        [Fact]
        public async Task WhenUserVotes2TimesOnly1VoteShouldBeCounted()
        {
            var list = new List<Vote>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repo = new EfRepository<Vote>(new ApplicationDbContext(options.Options));
            var service = new VotesService(repo);

            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            var votes = service.GetAverageVote(1);

            Assert.Equal(5, votes);
        }
    }
}
