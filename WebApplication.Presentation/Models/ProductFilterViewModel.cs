using ClassLibrary.Domain.Models;

namespace WebApplication.Presentation.Models
{
    public class ProductFilterViewModel
    {
        public string? Name { get; set; }
        public string? Store { get; set; }
        public string? Category { get; set; }
        public List<Product> Products { get; set; } = new();
        public int ShoppingListId { get; set; }

        public ProductFilter ToDomainModel()
        {
            return new ProductFilter(Name, Store, Category);
        }
    }
}
