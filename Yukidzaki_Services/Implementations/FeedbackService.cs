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
    public class FeedbackService: IFeedbackService
    {
        
        private readonly IBaseRepository<Email> _emailRepository;
        public FeedbackService( IBaseRepository<Email> emailRepository)
        {
            _emailRepository = emailRepository;
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


        
    }
}
