using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;

namespace ClassLibrary.Domain.Services
{
    public class ProductService
    {
        private readonly IProductRepo _productRepo;
        
        public ProductService(IProductRepo productRepository)
        {
            _productRepo = productRepository;
        }//

        public Product GetProductById(int id)
        {
            return _productRepo.GetById(id);
        }//
        
    }
}
