using BCrypt.Net;
using ClassLibrary.Domain.Domain_Exceptions;
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
            try
            {
                return _userRepo.GetByUsername(username);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"GetByUsername({username})");
                throw;
            }
        }

        public bool UserExists(string username)
        {
            try
            {
                return _userRepo.GetByUsername(username) != null;
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"UserExists({username})");
                throw;
            }
        }

        public void CreateUser(string username, string plainPassword)
        {
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                _userRepo.Create(username, hash);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"CreateUser(username: {username})");
            }
        }
    }
}
