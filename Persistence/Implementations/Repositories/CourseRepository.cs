using Domain.Entities;
using Domain.Enums;
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

        public async Task<CourseRequest> CreateCourseRequest(CourseRequest courseRequest)
        {
             await _context.CourseRequests.AddAsync(courseRequest);
            await _context.SaveChangesAsync();
            return courseRequest;
        }
        public async Task<CourseRequest> GetCourseRequestById(Guid id)
        {
            return await _context.CourseRequests.Include(c => c.Course).Include(o => o.Learner).SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<CourseRequest>> GetAllCourseRequestsApproved()
        {
            return await _context.CourseRequests.Where(d => d.RequestStatus == CourseRequestStatus.Approved).ToListAsync();
        }

        public async Task<IEnumerable<CourseRequest>> GetAllCourseRequestsRejected()
        {
            return await _context.CourseRequests.Where(d => d.RequestStatus == CourseRequestStatus.Rejected).ToListAsync();
        }

        public async Task<IEnumerable<CourseRequest>> GetAllCourseRequestsUntreated()
        {
            return await _context.CourseRequests.Where(d => d.RequestStatus == CourseRequestStatus.Requested).ToListAsync();
        }

        public async Task<IEnumerable<CourseRequest>> GetUntreatedCourseRequestsByLearner(Guid learnerId)
        {
            return await _context.CourseRequests.Where(c => c.LearnerId == learnerId && c.RequestStatus == CourseRequestStatus.Requested).ToListAsync();
        }

        public async Task<IList<Course>> GetCoursesByInstructor(Guid instructorId)
        {
            return await _context.Courses
                .Include(o => o.Category)
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

        public async  Task<InstructorCourse> InstructorCourseAssignment(InstructorCourse instructorCourse)
        {
            await _context.InstructorCourses.AddAsync(instructorCourse);
            await _context.SaveChangesAsync();
            return instructorCourse;
        }

        public async Task<LearnerCourse> LearnerCourseAssignment(LearnerCourse learnerCourse)
        {
            await _context.LearnerCourses.AddAsync(learnerCourse);
            await _context.SaveChangesAsync();
            return learnerCourse;
        }

        public async Task<IEnumerable<Course>> SearchCoursesByName(string searchText)
        {
            return await _context.Courses.Include(v => v.Category).Where(course => EF.Functions.Like(course.Name, $"%{searchText}%")).ToListAsync();
        }

        
    }
}
