using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yukidzaki_Domain.Models;

namespace Yukidzaki_DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T model);
        Task CreateMultiple(IEnumerable<T> model);

        IQueryable<T> GetAll();

        //Task Delete(T model);

        Task<T> Update(T model);
     
        
    }
}
