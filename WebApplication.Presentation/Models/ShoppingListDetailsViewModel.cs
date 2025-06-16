namespace WebApplication.Presentation.Models
{
    public class ShoppingListDetailsViewModel
    {
        public int ShoppingListId { get; set; }
        public string Theme { get; set; }
        public List<ProductWithQuantityViewModel> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
