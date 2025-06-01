using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class ShoppingList
    {
        public int Id { get; private set; }
        public string Theme { get; private set; }
        public int UserId { get; private set; }

        public ShoppingList(string theme, int userId) // Constructor zonder Id (bij maken van nieuwe lijst)
        {
            Theme = theme;
            UserId = userId;
        }

        public ShoppingList(int id, string theme, int userId) // Constructor met Id (bij ophalen uit DB)
        {
            Id = id;
            Theme = theme;
            UserId = userId;
        }

    }
}
