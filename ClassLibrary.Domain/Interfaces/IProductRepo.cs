using ClassLibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Interfaces
{
    public interface IProductRepo
    {
        
        Product GetById(int id);//
        
    }
}
