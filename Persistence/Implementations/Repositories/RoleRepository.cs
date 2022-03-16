using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;

namespace Persistence.Implementations.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
