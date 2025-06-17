using ClassLibrary.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly ShoppingListService _shoppingListService;

        public ShoppingListController(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string theme)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Je moet ingelogd zijn om een boodschappenlijst te maken.";
                return RedirectToAction("Login", "User");
            }
            _shoppingListService.CreateShoppingList(theme, userId.Value);
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Details(int shoppingListId)
        {
            var domainItems = _shoppingListService.GetShoppingListDetails(shoppingListId);
            var totalPrice = _shoppingListService.CalculateTotalPrice(shoppingListId);
            var shoppingList = _shoppingListService.GetShoppingListById(shoppingListId);

            var viewModel = new ShoppingListDetailsViewModel
            {
                ShoppingListId = shoppingListId,
                Theme = shoppingList.Theme,
                Products = domainItems.Select(i => new ProductWithQuantityViewModel
                {
                    Product = i.Product,
                    Quantity = i.Quantity
                }).ToList(),
                TotalPrice = totalPrice
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Je moet ingelogd zijn om je boodschappenlijsten te bekijken.";
                return RedirectToAction("Login", "User");
            }
            var domainData = _shoppingListService.GetShoppingListsWithProductsByUser(userId.Value);

            var shoppingListViewModel = domainData.Select(item => new ShoppingListWithProductsViewModel
            {
                ShoppingListId = item.shoppingList.Id,
                Theme = item.shoppingList.Theme,
                Products = item.products.Select(i => new ProductWithQuantityViewModel
                {
                    Product = i.Product,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();

            return View(shoppingListViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int shoppingListId)
        {
            _shoppingListService.DeleteList(shoppingListId);
            TempData["Message"] = "Boodschappenlijst succesvol verwijderd.";
            return RedirectToAction("Index");
        }


    }
}
