using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_DAL.Interfaces
{
    internal interface IBaseRepository<T>
    {
        Task Create(T model);

        IQueryable<T> GetALL();

        Task<T> Update();
        
        Task Delete(T model);


    }
}
