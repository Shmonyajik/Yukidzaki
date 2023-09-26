using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.Models;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http;

namespace Yukidzaki_Services.Interfaces
{
    public interface ITokenService 
    {
        Task<BaseResponse<IEnumerable<Token>>> GetToken();
        Task<BaseResponse<bool>> LoadTokens(IFormFileCollection files);
        


        //Task<IBaseResponse<TokenViewModel>> GetTokens(int id);

        //Task<IBaseResponse<TokenViewModel>> CreateToken(TokenViewModel TokenViewModel);

        //Task<IBaseResponse<bool>> DeleteToken(int id);

        //Task<IBaseResponse<Token>> GetTokenByName(string name);

        //Task<IBaseResponse<Token>> Edit(int id, TokenViewModel model);
    }
}
