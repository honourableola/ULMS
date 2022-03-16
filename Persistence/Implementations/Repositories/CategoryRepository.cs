using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;

namespace Persistence.Implementations.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
