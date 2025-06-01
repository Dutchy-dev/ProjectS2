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
            int userId = 1; // tijdelijk hardcoded
            _shoppingListService.CreateShoppingList(theme, userId);
            return RedirectToAction("Create");
        }
        
        [HttpGet]
        public IActionResult Details(int shoppingListId)
        {
            var domainItems = _shoppingListService.GetShoppingListDetails(shoppingListId);

            var productViewModel = domainItems.Select(i => new ProductWithQuantityViewModel()
            {
                Product = i.Product,
                Quantity = i.Quantity
            }).ToList();

            ViewBag.ShoppingListId = shoppingListId;
            return View(productViewModel);
        }
        /*
        [HttpGet]
        public IActionResult SelectListToAddProduct()
        {
            return RedirectToAction("AllShoppingLists", new { selectingList = true });
        }
        */

        [HttpGet]
        public IActionResult AllShoppingLists()
        {
            int userId = 1; // tijdelijk hardcoded
            var domainData = _shoppingListService.GetShoppingListsWithProductsByUser(userId);

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
            return RedirectToAction("AllShoppingLists");
        }


    }
}
