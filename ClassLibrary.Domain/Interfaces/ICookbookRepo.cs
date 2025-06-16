using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface ICookbookRepo
    {
        List<Cookbook> GetCookbooksByUserId(int userId);

        void CreateCookbook(Cookbook cookbook);

        Cookbook? GetCookbookById(int id);

        void UpdateCookbook(Cookbook cookbook);

        void DeleteCookbook(int id);
    }
}
