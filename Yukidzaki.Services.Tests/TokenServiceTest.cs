using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yukidzaki.Services.Tests.Helpers;
using FakeItEasy;
using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Models;
using Yukidzaki_Services.Implementations;
using Yukidzaki_Services.Interfaces;
using AutoMapper;
using Yukidzaki_Domain.Responses;

namespace Yukidzaki.Services.Tests
{
    public class TokenServiceTest 
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Token> _tokenRepository;
        private readonly IBaseRepository<SeasonCollection> _seasonCollectionRepository;
        private readonly IBaseRepository<Filter> _filterRepository;
        private readonly ITokenService _tokenService;

        public TokenServiceTest() 
        {
            _tokenRepository = A.Fake<IBaseRepository<Token>>();
            _seasonCollectionRepository = A.Fake<IBaseRepository<SeasonCollection>>();
            _filterRepository = A.Fake<IBaseRepository<Filter>>();
            _mapper = A.Fake<IMapper>();

            _tokenService = new TokenService(_tokenRepository, _mapper, _seasonCollectionRepository,_filterRepository);
        }
        [Fact]
        public async Task GetToken_Return_BaseResonseWithTokenModel()
        {
            // Arrange         
            var tokens = A.Fake<IQueryable<Token>>();
            A.CallTo(() => _tokenRepository.GetAll()).Returns(tokens);

            // Act
            var result = await _tokenService.GetToken();
            // Assert
            Assert.IsType<BaseResponse<IEnumerable<Token>>>(result);
        }
    }
}
