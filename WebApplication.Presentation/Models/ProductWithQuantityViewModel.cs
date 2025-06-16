using ClassLibrary.Domain.Models;

namespace WebApplication.Presentation.Models
{
    public class ProductWithQuantityViewModel
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

    }
}
