using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class Cookbook
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public Cookbook(int id, int userId, string name, string? description = null)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
        }

        // Extra constructor voor nieuwe objecten (zonder ID)
        public Cookbook(int userId, string name, string? description = null)
        {
            UserId = userId;
            Name = name;
            Description = description;
        }

        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }

}
