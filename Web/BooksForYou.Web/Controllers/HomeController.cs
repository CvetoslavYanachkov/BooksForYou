namespace BooksForYou.Web.Controllers
{
    using System.Diagnostics;

    using BooksForYou.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Book");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
