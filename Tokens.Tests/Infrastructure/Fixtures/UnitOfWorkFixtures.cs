using Calabonga.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yukidzaki.DAL.Tests.Infrastructure.Helpers;
using Yukidzaki_DAL;

namespace Yukidzaki.DAL.Tests.Infrastructure.Fixtures
{
    public class UnitOfWorkFixture
    {
        public IUnitOfWork<ApplicationDbContext> Create()
        {
            var mock = UnitOfWorkHelper.GetMock();
            return mock.Object;
        }
    }
}
