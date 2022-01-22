using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Implementations.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IList<Course>> GetCoursesByInstructor(Guid instructorId)
        {
            return await _context.Courses
                .Include(c => c.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(s => s.LearnerCourses)
                .ThenInclude(t => t.Learner)
                .Where(f => f.InstructorCourses
                .Any(c => c.InstructorId == instructorId))
                .ToListAsync();
        }

        public async Task<IList<Course>> GetCoursesByLearner(Guid learnerId)
        {
            return await _context.Courses
                .Include(o => o.Category)
                .Include(m => m.LearnerCourses)
                .ThenInclude(n => n.Learner)
                .Include(f => f.InstructorCourses)
                .ThenInclude(d => d.Instructor)
                .Where(d => d.LearnerCourses.Any(a => a.LearnerId == learnerId)).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetSelectedCourses(IList<Guid> ids)
        {
            return await _context.Courses
                .Include(o => o.Category)
                .Include(m => m.LearnerCourses)
                .ThenInclude(n => n.Learner)
                .Include(f => f.InstructorCourses)
                .ThenInclude(d => d.Instructor)
                .Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Course>> SearchCoursesByName(string searchText)
        {
            return await _context.Courses.Where(course => EF.Functions.Like(course.Name, $"%{searchText}%")).ToListAsync();
        }
    }
}
