using Babadzaki_DAL.Interfaces;
using Babadzaki_Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_DAL.Repositories
{
    public class TokenRepository : IBaseRepository<Token>
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Token model)
        {
            await _context.Tokens.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Token> GetAll()
        {
            return _context.Tokens;
        }

        public async Task Delete(Token model)
        {
            _context.Tokens.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Token> Update(Token model)
        {
            _context.Tokens.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
        
    }
}
