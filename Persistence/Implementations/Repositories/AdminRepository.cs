using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;

namespace Persistence.Implementations.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
