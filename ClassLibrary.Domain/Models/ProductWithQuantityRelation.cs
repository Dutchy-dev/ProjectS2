using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class ProductWithQuantityRelation
    {
        public int OwnerId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public ProductWithQuantityRelation(int ownerId, int productId, int quantity)
        {
            OwnerId = ownerId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
