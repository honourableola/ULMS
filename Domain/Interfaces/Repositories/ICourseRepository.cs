using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetSelectedCourses(IList<Guid> ids);
        Task<IList<Course>> GetCoursesByLearner(Guid learnerId);
        Task<IList<Course>> GetCoursesByInstructor(Guid instructorId);
        Task<IEnumerable<Course>> SearchCoursesByName(string searchText);
        Task<LearnerCourse> LearnerCourseAssignment(LearnerCourse learnerCourse);
        Task<InstructorCourse> InstructorCourseAssignment(InstructorCourse instructorCourse);
        Task<CourseRequest> CreateCourseRequest(CourseRequest courseRequest);
        Task<IEnumerable<CourseRequest>> GetAllCourseRequestsUntreated();
        Task<IEnumerable<CourseRequest>> GetAllCourseRequestsRejected();
        Task<IEnumerable<CourseRequest>> GetAllCourseRequestsApproved();
        Task<IEnumerable<CourseRequest>> GetUntreatedCourseRequestsByLearner(Guid learnerId);
        Task<CourseRequest> GetCourseRequestById(Guid id);

    }
}
