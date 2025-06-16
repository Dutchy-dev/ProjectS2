using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    public class CookbookController : Controller
    {
        private readonly CookbookService _cookbookService;

        public CookbookController(CookbookService cookbookService)
        {
            _cookbookService = cookbookService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Je moet ingelogd zijn om je kookboeken te bekijken.";
                return RedirectToAction("Login", "User");
            }

            var cookbooks = _cookbookService.GetCookbooksForUser(userId.Value);

            return View(cookbooks);
        }

        [HttpPost]
        public IActionResult Create(CreateCookbookViewModel ViewModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var newCookbook = new Cookbook(userId.Value, ViewModel.Name, ViewModel.Description);

            _cookbookService.CreateCookbook(newCookbook);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cookbook = _cookbookService.GetCookbookById(id);
            if (cookbook == null) return NotFound();

            var model = new EditCookbookViewModel
            {
                Id = cookbook.Id,
                Name = cookbook.Name,
                Description = cookbook.Description,
                UserId = cookbook.UserId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditCookbookViewModel ViewModel)
        {
            var cookbook = _cookbookService.GetCookbookById(ViewModel.Id);
            if (cookbook == null) return NotFound();

            cookbook.Update(ViewModel.Name, ViewModel.Description);
            _cookbookService.UpdateCookbook(cookbook);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _cookbookService.DeleteCookbook(id);
            return RedirectToAction("Index");
        }
    }
}
