using ClassLibrary.Domain.Interfaces;
using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Services
{
    public class CookbookService
    {
        private readonly ICookbookRepo _cookbookRepo;

        public CookbookService(ICookbookRepo repo)
        {
            _cookbookRepo = repo;
        }

        public List<Cookbook> GetCookbooksForUser(int userId)
        {
            return _cookbookRepo.GetCookbooksByUserId(userId);
        }

        public void CreateCookbook(Cookbook cookbook)
        {
            _cookbookRepo.CreateCookbook(cookbook);
        }

        public Cookbook? GetCookbookById(int id)
        {
            return _cookbookRepo.GetCookbookById(id);
        }

        public void UpdateCookbook(Cookbook cookbook)
        {
            _cookbookRepo.UpdateCookbook(cookbook);
        }

        public void DeleteCookbook(int id)
        {
            _cookbookRepo.DeleteCookbook(id);
        }
    }
}
