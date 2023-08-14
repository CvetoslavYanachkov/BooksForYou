
namespace BooksForYou.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BooksForYou.Common;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Web.ViewModels.Publishers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class PublisherController : BaseController
    {
        private readonly IPublishersService _publisherService;

        public PublisherController(IPublishersService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] int p = 1, [FromQuery] int s = 5)
        {
            var publishers = await _publisherService.GetPublishersAsync(p, s);

            return View(publishers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PublisherCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PublisherCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var book = await _publisherService.CreatePublisherAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _publisherService.GetPublisherForEditAsync<PublisherEditViewModel>(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PublisherEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _publisherService.UpdatePublisherAsync(id, model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _publisherService.GetPublisherForEditAsync<PublisherDetailsViewModel>(id);

            return View(model);
        }
    }
}
