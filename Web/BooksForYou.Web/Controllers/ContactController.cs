namespace BooksForYou.Web.Controllers
{
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Services.Messaging;
    using BooksForYou.Web.ViewModels.ContactUs;
    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public ContactController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            var model = new ContactUsViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _emailSender.SendEmailAsync("cyanachkov@gmail.com", model.Name, GlobalConstants.AdminMail, model.Subject, model.Message);

            return RedirectToAction("All", "Book");
        }
    }
}
