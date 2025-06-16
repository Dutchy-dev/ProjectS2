using ClassLibrary.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Presentation.Models
{
    public class RecipeDetailsViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public List<ProductWithQuantityEditViewModel> Products { get; set; } = new List<ProductWithQuantityEditViewModel>();
        public int CookbookId { get; set; }
        public string? RemovedProductIds { get; set; }
        public ProductFilterViewModel ProductFilter { get; set; } = new ProductFilterViewModel();

    }
}
