using Babadzaki_DAL.Interfaces;
using Babadzaki_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Babadzaki_DAL.Repositories
{
    public class SeasonCollectionRepository : IBaseRepository<SeasonCollection>
    {
        private readonly ApplicationDbContext _context;
        public SeasonCollectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(SeasonCollection model)
        {
            await _context.SeasonCollections.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(SeasonCollection model)
        {
            _context.SeasonCollections.Remove(model);
            await _context.SaveChangesAsync();
        }

        public IQueryable<SeasonCollection> GetAll()
        {
            return _context.SeasonCollections;
        }

        public async Task<SeasonCollection> Update(SeasonCollection model)
        {
            _context.SeasonCollections.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
