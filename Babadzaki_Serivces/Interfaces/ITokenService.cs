using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Babadzaki_Domain.Responses;
using Babadzaki_Domain.Models;
using System.Runtime.ConstrainedExecution;

namespace Babadzaki_Serivces.Interfaces
{
    public interface ITokenService 
    {
        Task<BaseResponse<IEnumerable<Token>>> GetToken();
        


        //Task<IBaseResponse<TokenViewModel>> GetTokens(int id);

        //Task<IBaseResponse<TokenViewModel>> CreateToken(TokenViewModel TokenViewModel);

        //Task<IBaseResponse<bool>> DeleteToken(int id);

        //Task<IBaseResponse<Token>> GetTokenByName(string name);

        //Task<IBaseResponse<Token>> Edit(int id, TokenViewModel model);
    }
}
