using Microsoft.EntityFrameworkCore;
using Yukidzaki_DAL;
namespace Yukidzaki.DAL.Tests.Infrastructure.Helpers
{
    public class DbContextHelper
    {
        public ApplicationDbContext Context { get; set; }
        public DbContextHelper()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("Unit_TestingDB");

            var options = builder.Options;
            Context = new ApplicationDbContext(options);

            Context.AddRange(TokenHelper.GetMany());

            Context.SaveChanges();
        }
    }
}
