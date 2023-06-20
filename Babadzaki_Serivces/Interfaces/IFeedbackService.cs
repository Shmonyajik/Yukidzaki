using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_Serivces.Interfaces
{
    public interface IHomeService
    {
        Task<BaseResponse<HomeVM>> GetTokens();

        Task<BaseResponse<Email>> SaveEmail(Email email);
    }
}
