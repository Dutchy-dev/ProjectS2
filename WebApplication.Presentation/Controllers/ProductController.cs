using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;

namespace WebApplication.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            Product product = _productService.GetProductById(id);
            return View(product);
        }//
        
    }
}
