
using Microsoft.EntityFrameworkCore;
using Yukidzaki_DAL;
using Yukidzaki_Domain.Models;

namespace Yukidzaki.Services.Tests.Helpers
{
    public class DbContextHelper
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Unit_TestingDB")
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            //context.Tokens.AddRange(GetTestTokens());
            //context.Emails.AddRange(GetTestEmails());
            //context.SaveChanges();
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

       
    }
}
