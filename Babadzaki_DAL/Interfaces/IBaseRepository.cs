using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T model);

        IQueryable<T> GetAll();

        Task Delete(T model);

        Task<T> Update(T model);
     
        
    }
}
