using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Enums;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;
using Yukidzaki_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Yukidzaki_Services.Implementations
{
    public class HomeService: IHomeService
    {
        private readonly IBaseRepository<Token> _tokenRepository;
        private readonly IBaseRepository<Email> _emailRepository;
        
        public HomeService(IBaseRepository<Token> tokenRepository, IBaseRepository<Email> emailRepository)
        {
            _tokenRepository = tokenRepository;
            _emailRepository = emailRepository;
            
        }

        public async Task<BaseResponse<HomeVM>> GetTokens()
        {
            var baseResponse = new BaseResponse<HomeVM>();
            try
            {
                var tokens = RandomizeTokes( await _tokenRepository.GetAll().ToListAsync()).Take(25);

                baseResponse.Data = new HomeVM { Tokens = tokens, Email = new Email() };

                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<HomeVM>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Email>> SaveEmail(Email _email)
        {
            var baseResponse = new BaseResponse<Email>();
            try
            {
                var email = await _emailRepository.GetAll().FirstOrDefaultAsync(x => x.Name == _email.Name);
                if(email != null)
                {
                    baseResponse.Data = email;
                    baseResponse.Description = "This Email already exists";
                    baseResponse.StatusCode = StatusCode.OK; 
                    
                }
                else
                {
                    await _emailRepository.Create(_email);
                    baseResponse.Data = _email;
                    baseResponse.StatusCode = StatusCode.OK;
                }
                return baseResponse;
                
            }
            catch (Exception ex)
            {
                return new BaseResponse<Email>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };

            }
        }

        private List<Token> RandomizeTokes(List<Token> tokens)
        {
            Random random = new Random();
            for (int i = tokens.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                // обменять значения data[j] и data[i]
                var temp = tokens[j];
                tokens[j] = tokens[i];
                tokens[i] = temp;
            }
            return tokens;
        }



    }
}
