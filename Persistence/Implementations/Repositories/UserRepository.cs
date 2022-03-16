using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;

namespace Persistence.Implementations.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
