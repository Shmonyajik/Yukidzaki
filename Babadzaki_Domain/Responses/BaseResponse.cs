using Babadzaki_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_Domain.Responses
{
    internal class BaseResponse<T>: IBaseResponse<T>
    {
        public string Description { get; set; }

        public StatusCode StatusCode { get; set; }

        public T Data { get;}

    }

    internal interface IBaseResponse<T>
    { 
        T Data { get;}
    }
}
