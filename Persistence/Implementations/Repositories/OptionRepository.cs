using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class OptionRepository : BaseRepository<Option>, IOptionRepository
    {
        public OptionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Option> GetSelectedOptions(IList<Guid> ids)
        {
            return _context.Options
                .Include(a => a.Question)
                .Where(c => ids.Contains(c.Id)).ToList();
        }
    }
}
