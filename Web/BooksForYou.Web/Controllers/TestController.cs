namespace BooksForYou.Web.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using BooksForYou.Services.Messaging;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public TestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Test()
        {
            var html = new StringBuilder();
            html.AppendLine($"<h1>{"dsa"}</h1>");
            html.AppendLine($"<h3>{"dsa"}</h3>");
            html.AppendLine($"<img src=\"{"dsa"}\" />");
            _emailSender.SendEmailAsync("cyanachkov@gmail.com", "Test", "ceno1902@gmail.com", "Test", html.ToString());

            return View();
        }
    }
}
