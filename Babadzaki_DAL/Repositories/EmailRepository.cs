using Babadzaki_DAL.Interfaces;
using Babadzaki_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_DAL.Repositories
{
    public class EmailRepository: IBaseRepository<Email>
    {
        private readonly ApplicationDbContext _context;
        public EmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Email model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Email model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Email> GetAll()
        {
            return _context.Emails;
        }

        public async Task<Email> Update(Email model)
        {
            throw new NotImplementedException();
        }
    }
}
