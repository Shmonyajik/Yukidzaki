
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Yukidzaki.Services.Tests.Helpers;
using Yukidzaki_DAL;
using Yukidzaki_DAL.Interfaces;
using Yukidzaki_DAL.Repositories;
using Yukidzaki_Domain.Models;
using Yukidzaki_Domain.Responses;
using Yukidzaki_Domain.ViewModels;
using Yukidzaki_Services.Implementations;
using Yukidzaki_Services.Interfaces;

namespace Yukidzaki.Services.Tests
{
    
    public class GalleryServiceTest 
    {
        private IBaseRepository<Token> _tokenRepository;
        private IBaseRepository<SeasonCollection> _seasonCollectionRepository;
        private IBaseRepository<Filter> _filterRepository;
        private IBaseRepository<TokensFilters> _tokensFiltersRepository;
        private IGalleryService _galleryService;
        private IMemoryCache _cache;
        public GalleryServiceTest()
        {
            _tokenRepository = new TokenRepository(GetDbContext());
            _seasonCollectionRepository = new SeasonCollectionRepository(GetDbContext());
            _filterRepository = new FilterRepository(GetDbContext());
            _tokensFiltersRepository = new TokensFiltersRepository(GetDbContext());
            _cache = A.Fake<IMemoryCache>();

            _galleryService = new GalleryService(
                _tokenRepository,
                _seasonCollectionRepository,
                _tokensFiltersRepository,
                _filterRepository, _cache
                );
            var rand = new Random();
            var tokens = new List<Token>(10);
            for (int i = 1; i <= 10; i++)
            {
                tokens.Add(new Token
                {
                    Id = i,
                    edition = i,
                    Description = "Test",
                    Image = $"ipfs://bafybeiaml6ymmcdchcsnbrzlfz27uutcm5qceguavdyta74kzyeinukpwy/{i}.png",
                    SeasonCollectionId = 1
                });
                
            }
            var filters = new List<Filter>(20);
            for (int i = 1; i <= filters.Count; i++)
            {
                filters.Add( new Filter
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString()
                });
            }
            var tf = new List<TokensFilters>();
            int z = 1;
            for (int i = 1; i <= tokens.Count; i++, z++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    tf.Add(new TokensFilters
                    {
                        Id = z,
                        TokenId = i,
                        FilterId = rand.Next(1, 20),
                        Value = Guid.NewGuid().ToString()

                    });
                    
                }
               
            }
            _tokenRepository.CreateMultiple(tokens);
            _filterRepository.CreateMultiple(filters);
            _tokensFiltersRepository.CreateMultiple(tf);
        }
        
        public ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            ;
            return context;
        }

        [Fact]
        public async Task GetToken_Return_BaseResonseWithTokenModel()
        {
            //Arrange
            

            
           var result = await _galleryService.GetToken(1);
            //Assert
            Assert.IsType<BaseResponse<Token>>(result);
        }
        
       
        //[Fact]
        //public async Task SaveEmail_SaveEmail_IfNotAlredyExists()
        //{
        //    Arrange
        //    var testEmail = new Email
        //    {
        //        Id = 1,
        //        Name = "Test"
        //    };
        //    var testEmail1 = new Email
        //    {
        //        Id = 2,
        //        Name = "Test1"
        //    };
        //    var emailRepository = new EmailRepository(GetDbContext());
        //    var tokenRepository = new TokenRepository(GetDbContext());
        //    var homeService = new HomeService(tokenRepository, emailRepository);
        //    await emailRepository.Create(testEmail);

        //    Act
        //   var result = await homeService.SaveEmail(testEmail1);
        //    Assert
        //    Assert.True(result.Description.IsNullOrEmpty() && result.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK);
        //    Assert.Equal(2, await emailRepository.GetAll().CountAsync());
        //}
        //[Fact]
        //public async Task SaveEmail_ThrowInvalidOperationException_IfEmailWithIdAlreadyExists()
        //{
        //    Arrange
        //    var testEmail = new Email
        //    {
        //        Id = 1,
        //        Name = "Test"
        //    };
        //    var testEmail1 = new Email
        //    {
        //        Id = 1,
        //        Name = "Test1"
        //    };
        //    var emailRepository = new EmailRepository(GetDbContext());
        //    var tokenRepository = new TokenRepository(GetDbContext());
        //    var homeService = new HomeService(tokenRepository, emailRepository);
        //    await emailRepository.Create(testEmail);


        //    Act & Assert

        //    _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        //    {
        //        var result = await homeService.SaveEmail(testEmail1);
        //    });
        //}



    }
}
