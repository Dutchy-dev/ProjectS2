using ClassLibrary.Domain.Services;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ProductListService _productListService;

        public ProductListController(ProductListService productListService)
        {
            _productListService = productListService;

        }
        /*
        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Formulier tonen
        }

        [HttpPost]
        public IActionResult Add(int shoppingListId, int productId, int quantity)
        {
            _productListService.AddProductToList(shoppingListId, productId, quantity);
            return RedirectToAction("Add");
        }
        */

        [HttpGet]
        public IActionResult AddToList(int shoppingListId)
        {
            var model = new ProductFilterViewModel
            {
                ShoppingListId = shoppingListId
            };
            return View(model);
            
        }

        [HttpPost]
        public IActionResult AddToList(ProductFilterViewModel model)
        {
            var domainFilter = model.ToDomainModel();
            var products = _productListService.GetFilteredProducts(domainFilter);
            model.Products = products;
            return View(model);
            
        }

        [HttpPost]
        public IActionResult AddFilteredProductToList(ProductListViewModel model)
        {
            var items = new ProductList(model.ShoppingListId, model.ProductId, model.Quantity);

            _productListService.AddProductToList(items);
            return RedirectToAction("AddToList", new { shoppingListId = model.ShoppingListId });
        }

        [HttpPost]
        public IActionResult RemoveFromList(int shoppingListId, int productId)
        {
            _productListService.RemoveProductFromList(shoppingListId, productId);
            return RedirectToAction("Details", "ShoppingList", new { shoppingListId = shoppingListId });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int shoppingListId, int productId, int delta)
        {
            _productListService.ChangeQuantity(shoppingListId, productId, delta);
            return RedirectToAction("Details", "ShoppingList", new { shoppingListId = shoppingListId });
        }
    }
}
