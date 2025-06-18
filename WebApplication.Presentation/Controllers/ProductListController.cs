using ClassLibrary.Domain.Services;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Presentation.Models;
using ClassLibrary.Domain.Domain_Exceptions;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Presentation.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ProductListService _productListService;
        private readonly RecipeService _recipeService;

        public ProductListController(ProductListService productListService, RecipeService recipeService)
        {
            _productListService = productListService;
            _recipeService = recipeService;
        }

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
            try
            {
                var domainFilter = model.ToDomainModel();
                var products = _productListService.GetFilteredProducts(domainFilter);
                model.Products = products;
            }
            catch (ServicesException)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het ophalen van de producten.";
            }

            return View(model);

        }

        [HttpPost]
        public IActionResult AddFilteredProductToList(ProductListViewModel model)
        {
            var items = new ProductList(model.ShoppingListId, model.ProductId, model.Quantity);

            try
            {
                _productListService.AddProductToList(items);
            }
            catch (ServicesException ex)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het toevoegen van het product.";
            }

            return RedirectToAction("AddToList", new { shoppingListId = model.ShoppingListId });
        }

        [HttpPost]
        public IActionResult RemoveFromList(int shoppingListId, int productId)
        {
            try
            {
                _productListService.RemoveProductFromList(shoppingListId, productId);
            }
            catch (ServicesException)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het verwijderen van het product.";
            }

            return RedirectToAction("Details", "ShoppingList", new { shoppingListId = shoppingListId });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int shoppingListId, int productId, int delta)
        {
            try
            {
                _productListService.ChangeQuantity(shoppingListId, productId, delta);
            }
            catch (ValidationException vex)
            {
                TempData["ErrorMessage"] = vex.Message;
            }
            catch (ServicesException)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het wijzigen van de hoeveelheid.";
            }

            return RedirectToAction("Details", "ShoppingList", new { shoppingListId = shoppingListId });
        }

        [HttpGet]
        public IActionResult AddToRecipe(int recipeId)
        {
            var model = new ProductFilterViewModel
            {
                RecipeId = recipeId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddToRecipe(ProductFilterViewModel model)
        {
            try
            {
                var domainFilter = model.ToDomainModel();
                var products = _productListService.GetFilteredProducts(domainFilter);
                model.Products = products;
            }
            catch (ServicesException)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het ophalen van de producten.";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddFilteredProductToRecipe(ProductListViewModel model)
        {
            try
            {
                var recipeProduct = new RecipeProduct(model.RecipeId, model.ProductId, model.Quantity);
                _recipeService.AddProductToRecipe(recipeProduct);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Er is iets misgegaan bij het toevoegen van het product.";
            }

            return RedirectToAction("AddToRecipe", new { recipeId = model.RecipeId });
        }
    }
}
