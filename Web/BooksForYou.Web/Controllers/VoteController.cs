namespace BooksForYou.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Votes;
    using BooksForYou.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : BaseController
    {
        private readonly IVotesService _votesService;

        public VoteController(IVotesService votesService)
        {
            _votesService = votesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _votesService.SetVoteAsync(model.BookId, userId, model.Value);
            var averageVote = _votesService.GetAverageVote(model.BookId);

            return new PostVoteResponseModel { AverageVote = averageVote };
        }
    }
}
