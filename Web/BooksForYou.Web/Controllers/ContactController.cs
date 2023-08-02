namespace BooksForYou.Web.Controllers
{
    using BooksForYou.Web.ViewModels.ContactUs;
    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        [HttpGet]
        public IActionResult ContactUs()
        {
            var model = new ContactUsViewModel();

            return View(model);
        }
    }
}
