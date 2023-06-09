using AutoMapper;
using Babadzaki_DAL;
using Babadzaki_Domain.Models;
using Babadzaki_Domain.Responses;
using Babadzaki_Serivces.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Runtime.ConstrainedExecution;
using Babadzaki_DAL.Interfaces;
using Babadzaki_Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Babadzaki_Serivces.Implementations
{
    public class TokenService : ITokenService
    {
        
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Token> _tokenRepository;
        //private readonly IBaseRepository<SeasonCollection> _seasonCollectionRepository;

        public TokenService(IBaseRepository<Token> tokenRepository)//, IBaseRepository<SeasonCollection> seasonCollectionRepository
        {
            _tokenRepository = tokenRepository;
            //_seasonCollectionRepository = seasonCollectionRepository;
        }
        public async Task<BaseResponse<IEnumerable<Token>>> GetToken()
        {
            var baseResponse = new BaseResponse<IEnumerable<Token>>();
            try
            {
                var tokens =  _tokenRepository.GetAll().ToListAsync();//Include(t=>t.SeasonCollection).

                baseResponse.Data = (IEnumerable<Token>)tokens;

                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Token>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        

    }
}
