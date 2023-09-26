
using Yukidzaki_DAL;
using Yukidzaki_Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yukidzaki.Services.Tests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        protected readonly ApplicationDbContext context;
        public TestBase()
        {
            context =  DbContextHelper.Create();
        }
        public void Dispose()
        {
           DbContextHelper.Destroy(context);
        }

        public static IEnumerable<Token> GetTestTokens(int number)
        {
            var tokens = new List<Token>(number);
            for (var i = 1; i <= number; i++)
            {
                tokens.Add(new Token
                {
                    Id = i,
                    edition = i,
                    Description = Guid.NewGuid().ToString(),
                    Image = $"ipfs://bafybeiaml6ymmcdchcsnbrzlfz27uutcm5qceguavdyta74kzyeinukpwy/{i}.png",
                    SeasonCollectionId = 1
                });
            }

            return tokens;
        }
        public static IEnumerable<Email> GetTestEmails(int number)
        {
            var emails = new List<Email>(number);
            for (var i = 1; i <= number; i++)
            {
                emails.Add(new Email
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString()
                });
            }

            return emails;
        }

        public static IEnumerable<Filter> GetTestFilters(int number)
        {
            var filters = new List<Filter>(number);
            for (var i = 1; i <= number; i++)
            {
                filters.Add(new Filter
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString()

                    
                });
            }

            return filters;
        }

        public IEnumerable<TokensFilters> GetTestTokensFilters()
        {
            var tokensAmount = context.Tokens.Count();
            var filtersAmount = context.Filters.Count();
            var tfList = new List<TokensFilters>();
            var tokensFilter = new TokensFilters();
            int z = 1;
            //var tokensFilters = new List<TokensFilters>(number);
            for (int i = 1 ; i <= tokensAmount; i++)
            {
                for (int j = 1; j <= filtersAmount; j++, z++)
                {
                    if(i%2 == 0)
                    {
                        if (j % 2 != 0)
                        {
                            continue;
                        }
                        tokensFilter = new TokensFilters
                        {
                            Id = z,
                            Value = Guid.NewGuid().ToString()
                        };
                        tokensFilter.TokenId = context.Tokens.FirstOrDefault(t => t.Id == i).Id;
                        tokensFilter.FilterId = context.Filters.FirstOrDefault(f => f.Id == j).Id;

                    }
                    else
                    {
                        if(j%2 == 0)
                        {
                            continue;
                        }
                        tokensFilter = new TokensFilters
                        {
                            Id = z,
                            Value = Guid.NewGuid().ToString()
                        };
                        tokensFilter.TokenId = context.Tokens.FirstOrDefault(t => t.Id == i).Id;
                        tokensFilter.FilterId = context.Filters.FirstOrDefault(f => f.Id == j).Id;
                    }
                    tfList.Add(tokensFilter);

                    
                }
            }

            return tfList;
        }

    }
}
