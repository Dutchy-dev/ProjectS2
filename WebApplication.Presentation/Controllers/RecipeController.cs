using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService;
        private readonly ShoppingListService _shoppingListService;


        public RecipeController(RecipeService recipeService, ShoppingListService shoppingListService)
        {
            _recipeService = recipeService;
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public IActionResult Index(int cookbookId)
        {
            var recipes = _recipeService.GetAllRecipesForCookbook(cookbookId);
            var viewModel = new RecipeIndexViewModel
            {
                CookbookId = cookbookId,
                Recipes = recipes.Select(r => new RecipeOverviewViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    ShortDescription = r.Description.Length > 50 ? r.Description[..50] + "..." : r.Description
                }).ToList(),
                NewRecipe = new RecipeCreateViewModel
                {
                    CookbookId = cookbookId
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound();

            var products = _recipeService.GetProductsForRecipe(id);

            // Gebruiker ophalen uit sessie
            int? userId = HttpContext.Session.GetInt32("UserId");
            List<RecipeDetailsViewModel.ShoppingListSummaryViewModel> shoppingLists = new();

            if (userId.HasValue)
            {
                var listsWithProducts = _shoppingListService.GetShoppingListsWithProductsByUser(userId.Value);

                shoppingLists = listsWithProducts.Select(tuple => new RecipeDetailsViewModel.ShoppingListSummaryViewModel
                {
                    Id = tuple.shoppingList.Id,
                    Name = tuple.shoppingList.Theme 
                }).ToList();
            }

            var viewModel = new RecipeDetailsViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookbookId = recipe.CookbookId,
                Products = products.Select(p => new ProductWithQuantityEditViewModel
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Quantity = p.Quantity
                }).ToList(),
                AvailableShoppingLists = shoppingLists
            };

            return View(viewModel);
        }

        /*
        [HttpGet]
        public IActionResult Details(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound();

            var products = _recipeService.GetProductsForRecipe(id);

            var viewModel = new RecipeDetailsViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookbookId = recipe.CookbookId,
                Products = products.Select(p => new ProductWithQuantityEditViewModel
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Quantity = p.Quantity
                }).ToList()
            };

            return View(viewModel);
        }*/

        [HttpGet]
        public IActionResult Edit(int id, [Bind(Prefix = "ProductFilter")] ProductFilterViewModel? productFilter = null)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound();
            var viewModel = new RecipeDetailsViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookbookId = recipe.CookbookId,
                Products = recipe.Products.Select(p => new ProductWithQuantityEditViewModel
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Quantity = p.Quantity
                }).ToList(),
                ProductFilter = productFilter ?? new ProductFilterViewModel()
            };

            if (productFilter != null)
            {
                viewModel.ProductFilter.Products = _recipeService.GetFilteredProducts(productFilter.ToDomainModel());
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(RecipeDetailsViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine($"ModelState error: {error}");
                }

                ViewModel.Products = _recipeService.GetProductsForRecipe(ViewModel.Id)
                    .Select(p => new ProductWithQuantityEditViewModel
                    {
                        ProductId = p.Product.Id,
                        ProductName = p.Product.Name,
                        Quantity = p.Quantity
                    }).ToList();

                //var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                //ViewBag.Errors = errors;
                return View(ViewModel);
            }

            // update receptgegevens
            _recipeService.UpdateRecipe(ViewModel.Id, ViewModel.Name, ViewModel.Description);

            // verwijder producten indien opgegeven
            if (!string.IsNullOrEmpty(ViewModel.RemovedProductIds))
            {
                var idsToRemove = ViewModel.RemovedProductIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                foreach (var id in idsToRemove)
                {
                    _recipeService.RemoveProductFromRecipe(ViewModel.Id, id);
                }
            }

            // update de overgebleven producten
            foreach (var p in ViewModel.Products)
            {
                _recipeService.UpdateProductQuantity(ViewModel.Id, p.ProductId, p.Quantity);
            }

            return RedirectToAction("Details", new { id = ViewModel.Id });
        }


        [HttpPost]
        public IActionResult Create(RecipeIndexViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewModel.Recipes = _recipeService.GetAllRecipesForCookbook(ViewModel.CookbookId)
                    .Select(r => new RecipeOverviewViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ShortDescription = r.Description.Length > 50 ? r.Description[..50] + "..." : r.Description
                    }).ToList();

                return View("Create", ViewModel);
            }

            var recipe = new Recipe(0, ViewModel.NewRecipe.Name, ViewModel.NewRecipe.Description, ViewModel.NewRecipe.CookbookId);
            _recipeService.Create(recipe);
            return RedirectToAction("Create", new { cookbookId = ViewModel.NewRecipe.CookbookId });
        }

        [HttpGet]
        public IActionResult Create(int cookbookId)
        {
            var viewModel = new RecipeIndexViewModel
            {
                CookbookId = cookbookId,
                NewRecipe = new RecipeCreateViewModel
                {
                    CookbookId = cookbookId
                },
                Recipes = new List<RecipeOverviewViewModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RemoveProduct(int recipeId, int productId)
        {
            _recipeService.RemoveProductFromRecipe(recipeId, productId);
            return RedirectToAction("Edit", new { id = recipeId });
        }

        [HttpPost]
        public IActionResult AddProductToRecipe(int recipeId, int productId, int quantity)
        {
            var recipeProduct = new RecipeProduct(recipeId, productId, quantity);
            _recipeService.AddProductToRecipe(recipeProduct);

            return RedirectToAction("Edit", new { id = recipeId });
        }

        [HttpPost]
        public IActionResult Delete(int id, int cookbookId)
        {
            _recipeService.DeleteRecipe(id);
            return RedirectToAction("Index", new { cookbookId });
        }

        [HttpPost]
        public IActionResult AddRecipeToShoppingList(int recipeId, int shoppingListId)
        {
            _recipeService.AddRecipeToShoppingList(recipeId, shoppingListId);
            return RedirectToAction("Details", new { id = recipeId });
        }

    }
}
