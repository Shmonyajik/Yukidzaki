using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukidzaki_Services.Interfaces
{
    public interface IHomeService
    {
        Task<BaseResponse<HomeVM>> GetTokens();

        Task<BaseResponse<Email>> SaveEmail(Email email);
    }
}
