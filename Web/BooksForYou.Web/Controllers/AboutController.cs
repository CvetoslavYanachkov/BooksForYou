namespace BooksForYou.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AboutController : BaseController
    {
        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
