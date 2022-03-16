using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Module>> SearchModuleByName(string searchText)
        {           
                return await _context.Modules.Include(v => v.Course).Where(module => EF.Functions.Like(module.Name, $"%{searchText}%")).ToListAsync();         
        }
    }
}
