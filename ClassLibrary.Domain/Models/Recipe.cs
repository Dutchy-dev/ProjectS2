using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class Recipe
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CookbookId { get; private set; }
        public List<ProductWithQuantity> Products { get; private set; } = new();

        public Recipe(int id, string name, string description, int cookbookId)
        {
            Id = id;
            Name = name;
            Description = description;
            CookbookId = cookbookId;
        }

        public Recipe(int id, string name, string description, int cookbookId, List<ProductWithQuantity> products)
            : this(id, name, description, cookbookId)
        {
            Products = products;
        }
    }
}
