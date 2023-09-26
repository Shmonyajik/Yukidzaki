
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    
    public class HomeServiceTest 
    {
        private  IBaseRepository<Token> _tokenRepository;
        private  IBaseRepository<Email> _emailRepository;
        private  IHomeService _homeService;
        public HomeServiceTest()
        {
                _tokenRepository = A.Fake<IBaseRepository<Token>>();
                _emailRepository = A.Fake<IBaseRepository<Email>>();

                _homeService = new HomeService(_tokenRepository, _emailRepository);
        }
        public ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            //context.Tokens.AddRange(GetTestTokens());
            //context.Emails.AddRange(GetTestEmails());
            //context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetTokens_Return_BaseResonseWithHomeVM()
        {
            //Arrange
            var tokens = A.Fake<IQueryable<Token>>();
            A.CallTo(() => _tokenRepository.GetAll()).Returns(tokens);

            // Act
            var result = await _homeService.GetTokens();
            // Assert
            Assert.IsType<BaseResponse<HomeVM>>(result);
        }
        [Fact]
        public async Task SaveEmail_NotSaveEmail_IfAlredyExists()
        {
            //Arrange
            var testEmail = new Email
            {
                Id = 1,
                Name = "Test"
            };
            var emailRepository = new EmailRepository(GetDbContext());
            var tokenRepository = new TokenRepository(GetDbContext());
            var homeService = new HomeService(tokenRepository, emailRepository);
            await emailRepository.Create(testEmail);

            // Act
            var result = await homeService.SaveEmail(testEmail);
            // Assert
            Assert.True(!result.Description.IsNullOrEmpty() && result.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK);
            Assert.Equal(1, await emailRepository.GetAll().CountAsync());
        }
        [Fact]
        public async Task SaveEmail_SaveEmail_IfNotAlredyExists()
        {
            //Arrange
            var testEmail = new Email
            {
                Id = 1,
                Name = "Test"
            };
            var testEmail1 = new Email
            {
                Id = 2,
                Name = "Test1"
            };
            var emailRepository = new EmailRepository(GetDbContext());
            var tokenRepository = new TokenRepository(GetDbContext());
            var homeService = new HomeService(tokenRepository, emailRepository);
            await emailRepository.Create(testEmail);

            // Act
            var result = await homeService.SaveEmail(testEmail1);
            // Assert
            Assert.True(result.Description.IsNullOrEmpty() && result.StatusCode == Yukidzaki_Domain.Enums.StatusCode.OK);
            Assert.Equal(2, await emailRepository.GetAll().CountAsync());
        }
        [Fact]
        public async Task SaveEmail_ThrowInvalidOperationException_IfEmailWithIdAlreadyExists()
        {
            //Arrange
            var testEmail = new Email
            {
                Id = 1,
                Name = "Test"
            };
            var testEmail1 = new Email
            {
                Id = 1,
                Name = "Test1"
            };
            var emailRepository = new EmailRepository(GetDbContext());
            var tokenRepository = new TokenRepository(GetDbContext());
            var homeService = new HomeService(tokenRepository, emailRepository);
            await emailRepository.Create(testEmail);


            // Act & Assert

            _ = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var result = await homeService.SaveEmail(testEmail1);
            });
        }



    }
}
