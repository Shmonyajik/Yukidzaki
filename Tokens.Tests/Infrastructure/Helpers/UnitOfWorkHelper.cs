using Calabonga.UnitOfWork;
using Moq;
using Yukidzaki_DAL;
using Yukidzaki_Domain.Models;

namespace Yukidzaki.DAL.Tests.Infrastructure.Helpers
{
    public static class UnitOfWorkHelper
    {
        public static Mock<IUnitOfWork<ApplicationDbContext>> GetMock()
        {
            var context = new DbContextHelper().Context;
            var unitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();

            unitOfWork.Setup(x => x.GetRepository<Token>(false)).Returns(new Repository<Token>(context));

            return unitOfWork;
        }
    }
}
