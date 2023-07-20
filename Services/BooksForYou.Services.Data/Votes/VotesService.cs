namespace BooksForYou.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> _votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            _votesRepository = votesRepository;
        }

        public double GetAverageVote(int bookId)
        {
            double averageVote = _votesRepository.All()
                .Where(x => x.BookId == bookId)
                .Average(x => x.Value);

            return averageVote;
        }

        public async Task SetVoteAsync(int bookId, string userId, byte value)
        {
            var vote = await _votesRepository.All()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);

            if (vote == null)
            {
                vote = new Vote
                {
                    BookId = bookId,
                    UserId = userId
                };

                await _votesRepository.AddAsync(vote);
            }

            vote.Value = value;
            await _votesRepository.SaveChangesAsync();
        }
    }
}
