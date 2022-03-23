using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<List<Assignment>> GetLearnerAssignmentsByCourseAndStatus(Guid learnerId, Guid courseId, AssignmentStatus status);
        Task<List<Assignment>> GetLearnerAssignments(Guid learnerId);
        Task<LearnerAssignment> GetLearnerAssignmentById(Guid learnerId, Guid assignmentId);
        Task<List<Assignment>> GetSelectedAssignments(List<Guid> ids);
        Task<LearnerAssignment> AddLearnerAssignment(LearnerAssignment learnerAssignment);
        Task<LearnerAssignment> UpdateLearnerAssignment(LearnerAssignment learnerAssignment);
        Task<List<Assignment>> GetLearnerAssignmentsToBeSubmittedByCourse(Guid learnerId, Guid courseId);
        public Task<List<Assignment>> GetInstructorAssignmentsToBeGradedByCourse(Guid courseId, Guid instructorId);
    }
}
