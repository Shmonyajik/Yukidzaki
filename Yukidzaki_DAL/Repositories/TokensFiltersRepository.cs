﻿using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukidzaki_DAL.Repositories
{
    public class TokensFiltersRepository : IBaseRepository<TokensFilters>
    {
        private readonly ApplicationDbContext _context;
        public TokensFiltersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(TokensFilters model)
        {
            await _context.TokensFilters.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMultiple(IEnumerable<TokensFilters> model)
        {
            await _context.AddRangeAsync(model);
        }

        public async Task Delete(TokensFilters model)
        {
            _context.TokensFilters.Remove(model);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TokensFilters> GetAll()
        {
            return _context.TokensFilters;
        }

        public async Task<TokensFilters> Update(TokensFilters model)
        {
            _context.TokensFilters.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
