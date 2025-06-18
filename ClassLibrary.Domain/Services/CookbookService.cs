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
    public class CookbookService
    {
        private readonly ICookbookRepo _cookbookRepo;

        public CookbookService(ICookbookRepo repo)
        {
            _cookbookRepo = repo;
        }

        public List<Cookbook> GetCookbooksForUser(int userId)
        {
            try
            {
                return _cookbookRepo.GetCookbooksByUserId(userId);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Kookboeken ophalen voor gebruiker met ID {userId}");
                throw;
            }
        }

        public void CreateCookbook(Cookbook cookbook)
        {
            try
            {
                _cookbookRepo.CreateCookbook(cookbook);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Kookboek aanmaken (naam: {cookbook.Name})");
            }
        }

        public Cookbook? GetCookbookById(int id)
        {
            try
            {
                var cookbook = _cookbookRepo.GetCookbookById(id);
                if (cookbook == null)
                    throw new ServicesException($"Geen kookboek gevonden met ID {id}", null!);

                return cookbook;
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Kookboek ophalen met ID {id}");
                throw;
            }
        }

        public void UpdateCookbook(Cookbook cookbook)
        {
            try
            {
                _cookbookRepo.UpdateCookbook(cookbook);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Kookboek updaten met ID {cookbook.Id}");
            }
        }

        public void DeleteCookbook(int id)
        {
            try
            {
                _cookbookRepo.DeleteCookbook(id);
            }
            catch (Exception ex)
            {
                ServiceExceptionHelper.HandleException(ex, $"Kookboek verwijderen met ID {id}");
            }
        }
    }
}
