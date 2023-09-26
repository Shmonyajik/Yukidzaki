

using Yukidzaki_Domain.Models;

namespace Yukidzaki.DAL.Tests.Infrastructure.Helpers
{
    public static class TokenHelper
    {
        private static Random rnd = new Random();
        
        public static Token GetOne() 
        {
            var tempEdition = rnd.Next(1, 1000000);
            return new Token
            {
                Id = rnd.Next(1, 1000000),
                edition = tempEdition,
                Image = $"ipfs://bafybeiaml6ymmcdchcsnbrzlfz27uutcm5qceguavdyta74kzyeinukpwy/{tempEdition}.png",
                SeasonCollectionId = 1,
                Description = "Test Description"


            };
        }
        public static IEnumerable<Token> GetMany()
        {
            yield return GetOne();
        }
    }
}
