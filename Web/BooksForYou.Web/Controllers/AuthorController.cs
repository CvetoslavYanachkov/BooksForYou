namespace BooksForYou.Web.Controllers
{
    using System.Threading.Tasks;

    using BooksForYou.Services.Data.Authors;
    using BooksForYou.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Mvc;

    public class AuthorController : Controller
    {
        private readonly IAuthorsService _authorsService;

        public AuthorController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet]
        public async Task<IActionResult> AuthorById(int id)
        {
            var authorModel = await _authorsService.GetAuthorByIdAsync<AuthorSingleViewModel>(id);

            return View(authorModel);
        }
    }
}
