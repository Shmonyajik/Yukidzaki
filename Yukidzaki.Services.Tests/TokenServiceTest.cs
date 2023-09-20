using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yukidzaki.Services.Tests.Helpers;
using Yukidzaki_DAL.Repositories;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;
using Yukidzaki_Services.Implementations;

namespace Yukidzaki.Services.Tests
{
    public class TokenServiceTest : TestBase
    {

        [Fact]
        public async Task GetTokens_Return_BaseResonseWithHomeVM()
        {
            // Arrange         
            var _tokenRepository = new TokenRepository(context);
            await _tokenRepository.CreateMultiple(GetTestTokens(10));
            var _emailRepository = new EmailRepository(context);
            await _emailRepository.CreateMultiple(GetTestEmails(10));

            var _tokenService = new HomeService(_tokenRepository, _emailRepository);
            // Act
            var result = await _tokenService.GetTokens();
            // Assert
            Assert.IsType<BaseResponse<HomeVM>>(result);
        }
    }
}
