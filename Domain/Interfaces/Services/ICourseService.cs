using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.CategoryViewModel;
using static Domain.Models.CourseViewModel;

namespace Domain.Interfaces.Services
{
    public interface ICourseService
    {
        public Task<BaseResponse> AddCourse(CreateCourseRequestModel model);
        public Task<BaseResponse> UpdateCourse(Guid id, UpdateCourseRequestModel model);
        public Task<BaseResponse> DeleteCourse(Guid id);
        public Task<CourseResponseModel> GetCourseById(Guid id);
        public Task<CoursesResponseModel> GetCoursesByCategory(Guid categoryId);
        public Task<CoursesResponseModel> GetCoursesByInstructor(Guid instructorId);
        public Task<CoursesResponseModel> GetCoursesByLearner(Guid learnerId);
        public Task<CoursesResponseModel> GetArchivedCourses();
        public Task<CoursesResponseModel> GetActiveCourses();
        public Task<CoursesResponseModel> SearchCoursesByName(string searchText);
        public Task<CoursesResponseModel> GetAllCourses();
        public Task<BaseResponse> RequestForCourse(CourseRequestRequestModel model);
        public Task<BaseResponse> AssignCoursesToLearner(LearnerCourseAssignmentRequestModel model);
        public Task<BaseResponse> AssignCoursesToInstructor(InstructorCourseAssignmentRequestModel model);
        public Task<BaseResponse> ApproveCourseRequest(Guid id);
        public Task<BaseResponse> RejectCourseRequest(Guid id);
        Task<CourseRequestsResponseModel> GetAllCourseRequestsUntreated();
        Task<CourseRequestsResponseModel> GetAllCourseRequestsRejected();
        Task<CourseRequestsResponseModel> GetAllCourseRequestsApproved();
        Task<CourseRequestsResponseModel> GetUntreatedCourseRequestsByLearner(Guid learnerId);

    }
}
