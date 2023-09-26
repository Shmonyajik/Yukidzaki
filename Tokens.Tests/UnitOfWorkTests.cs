using Yukidzaki.DAL.Tests.Infrastructure.Fixtures;
using Yukidzaki_Domain.Models;

namespace Yukidzaki.DAL.Tests
{
    public class UnitOfWorkTests : IClassFixture<UnitOfWorkFixture>
    {
        private readonly UnitOfWorkFixture _fixture;

        public UnitOfWorkTests(UnitOfWorkFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [Trait("UnitOfWorkTests", "UnderTesting")]
        public void ItShould_UnitOfWork_instance_created()
        {
            // arrange
            var sut = _fixture.Create();
            // act

            // assert
            Assert.NotNull(sut);
        }

        //[Fact]
        //[Trait("UnitOfWorkTests", "UnderTesting")]
        //public void ItShould_contains_token_1()
        //{
        //    // arrange
        //    const int expected = 1;
        //    var sut = _fixture.Create();
        //    // act
        //    var actual = sut.GetRepository<Token>().Count();
        //    // assert
        //    Assert.Equal(expected, actual);
        //}

        [Fact]
        [Trait("UnitOfWorkTests", "UnderTesting")]
        public void ItShould_contains_token_description_with_name()
        {
            // arrange
            const string expected = "Test Description";
            var sut = _fixture.Create();
            // act
            var actual =  sut.GetRepository<Token>().GetFirstOrDefault().Description;
            // assert
            Assert.Equal(expected, actual);
        }
    }


}
