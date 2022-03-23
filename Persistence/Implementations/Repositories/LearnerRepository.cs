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
    public class LearnerRepository : BaseRepository<Learner>, ILearnerRepository
    {
        public LearnerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Learner> GetLearnerByEmail(string email)
        {
            return await _context.Learners.SingleOrDefaultAsync(l => l.Email == email);
        }

        public async Task<IList<Learner>> GetLearnersByCourse(Guid courseId)
        {
            return await _context.Learners
                .Include(n => n.LearnerCourses)
                .ThenInclude(o => o.Course)
                .Where(c => c.LearnerCourses.Any(o => o.CourseId == courseId)).ToListAsync();
        }

        public async  Task<IEnumerable<Learner>> GetSelectedLearners(IList<Guid> ids)
        {
            return await _context.Learners.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Learner>> SearchLearnersByName(string searchText)
        {
            return await _context.Learners.Where(learner => EF.Functions.Like(learner.FirstName, $"%{searchText}%")).ToListAsync();
        }
    }
}
