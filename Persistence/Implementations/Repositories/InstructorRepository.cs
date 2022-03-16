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
    public class InstructorRepository : BaseRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IList<Instructor>> GetInstructorsByCourse(Guid courseId)
        {
            return await _context.Instructors
                .Include(s => s.InstructorCourses)
                .ThenInclude(f => f.Course)
                .Where(g => g.InstructorCourses.Any(h => h.CourseId == courseId)).ToListAsync();
        }

        public async Task<IEnumerable<Instructor>> GetSelectedInstructors(IList<Guid> ids)
        {
            return await _context.Instructors.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText)
        {
            return await _context.Instructors.Where(instructor => EF.Functions.Like(instructor.FirstName, $"%{searchText}%")).ToListAsync();
        }
    }
}
