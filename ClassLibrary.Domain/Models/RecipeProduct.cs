using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class RecipeProduct
    {
        public int RecipeId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public RecipeProduct(int recipeId, int productId, int quantity)
        {
            RecipeId = recipeId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
