using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }

        public User(int id, string name, string passwordHash)
        {
            this.Id = id;
            this.Name = name;
            this.PasswordHash = passwordHash;
        }
    }
}
