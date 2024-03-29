﻿using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukidzaki_DAL.Repositories
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

        public async Task CreateMultiple(IEnumerable<Email> model)
        {
            await _context.AddRangeAsync(model);
            await _context.SaveChangesAsync();
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
