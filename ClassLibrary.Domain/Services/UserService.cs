using BCrypt.Net;
using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Services
{
    public class UserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public User? GetByUsername(string username)
        {
            return _userRepo.GetByUsername(username);
        }

        public bool UserExists(string username)
        {
            return _userRepo.GetByUsername(username) != null;
        }

        public void CreateUser(string username, string plainPassword)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            _userRepo.Create(username, hash);
        }
    }
}
