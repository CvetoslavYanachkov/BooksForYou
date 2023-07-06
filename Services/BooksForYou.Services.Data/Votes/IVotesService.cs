namespace BooksForYou.Services.Data.Votes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task SetVoteAsync(int bookId, string userId, byte value);

        double GetAverageVote(int bookId);
    }
}
