﻿using System;
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
        public decimal? TotalPrice { get; private set; }

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
        
        public override bool Equals(object obj)
        {
            return obj is ShoppingList shoppingList &&
                   Id == shoppingList.Id &&
                   Theme == shoppingList.Theme &&
                   UserId == shoppingList.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Theme, UserId);
        }
        
    }
}
