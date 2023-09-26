using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Models;


namespace Yukidzaki_DAL.Repositories
{
    public class FilterRepository : IBaseRepository<Filter>
    {
        private readonly ApplicationDbContext _context;
        public FilterRepository(ApplicationDbContext context)
        {

            _context = context;

        }
        public async Task Create(Filter model)
        {
            await _context.Filters.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMultiple(IEnumerable<Filter> model)
        {
            await _context.AddRangeAsync(model);
        }

        public async Task Delete(Filter model)
        {
            _context.Filters.Remove(model);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Filter> GetAll()
        {
            return _context.Filters;
        }

        public async Task<Filter> Update(Filter model)
        {
            _context.Filters.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}
