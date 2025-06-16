using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface IProductRelationRepo
    {
        void AddProduct(ProductWithQuantityRelation relation);
        void RemoveProduct(int ownerId, int productId);
        void UpdateQuantity(int ownerId, int productId, int delta);
        List<ProductWithQuantity> GetProductsByOwnerId(int ownerId);
        List<Product> GetFilteredProducts(ProductFilter filter);
    }
}
