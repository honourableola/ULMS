using Domain.Entities;
using Domain.Interfaces.Repositories;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
