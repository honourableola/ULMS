using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.AssignmentViewModel;

namespace Domain.Interfaces.Services
{
    public interface IAssignmentService
    {
        public Task<BaseResponse> CreateAssignment(Guid userId, CreateAssignmentRequestModel model);
        public Task<BaseResponse> SubmitAssignment(Guid userId, SubmitAssignmentRequestModel model);
        public Task<BaseResponse> GradeAssignment(AssessAssignmentRequestModel model);
        public Task<BaseResponse> UpdateAssignment(Guid assignmentId, UpdateAssignmentRequestModel model);
        public Task<BaseResponse> AssignAssignmentToLearner(AssignAssignmentRequestModel model);
        public Task<BaseResponse> DeleteAssignment(Guid assignmentId);
        public Task<AssignmentResponseModel> GetAssignmentById(Guid assignmentId);
        public Task<AssignmentsResponseModel> GetAssignmentsByInstructor(Guid instructorId);
        public Task<AssignmentsResponseModel> GetAssignmentsByLearner(Guid learnerId);
        public Task<AssignmentsResponseModel> GetAssignmentsByCourse(Guid courseId);
        public Task<AssignmentsResponseModel> GetLearnerAssignmentsToBeSubmittedByCourse(Guid userId, Guid courseId);
        public Task<AssignmentsResponseModel> GetInstructorAssignmentsToBeGradedByCourse(Guid userId, Guid courseId);
        public Task<AssignmentsResponseModel> FilterLearnerAssignmentsByCourseAndStatus(Guid userId, FilterLearnerAssignmentsRequestModel model);
        public Task<AssignmentsResponseModel> FilterInstructorAssignmentsByCourse(Guid userId, Guid courseId);
       





    }
}
