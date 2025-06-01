using ClassLibrary.Domain.Models;

namespace WebApplication.Presentation.Models
{
    public class ShoppingListWithProductsViewModel
    {
        public int ShoppingListId { get; set; }
        public string Theme { get; set; }
        public List<ProductWithQuantityViewModel> Products { get; set; }
    }
}
