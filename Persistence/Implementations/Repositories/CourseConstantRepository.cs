using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;

namespace Persistence.Implementations.Repositories
{
    public class CourseConstantRepository : BaseRepository<CourseConstant>, ICourseConstantRepository
    {
        public CourseConstantRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
