namespace BooksForYou.Web.Areas.Administration.Controllers
{
    using System.Globalization;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;
    using BooksForYou.Services.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : AdministrationController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IUserService _userService;

        public UserController
       (RoleManager<ApplicationRole> roleManager,
       IUserService userService)
        {
            _roleManager = roleManager;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateRole()
        {
            //await _roleManager.CreateAsync(new ApplicationRole()
            //{
            //    Name = "Publisher"
            //});

            return Ok();
        }

        public async Task<IActionResult> All()
        {
            var users = await _userService.GetUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserEditAsync(id);

            return View(user);
        }

        public async Task<IActionResult> ById(string id)
        {
            var user = await _userService.GetById(id);

            return View(user);
        }
    }
}
