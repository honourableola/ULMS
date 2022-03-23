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
    public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<LearnerAssignment> AddLearnerAssignment(LearnerAssignment learnerAssignment)
        {
            await _context.LearnerAssignments.AddAsync(learnerAssignment);
            await _context.SaveChangesAsync();
            return learnerAssignment;
        }

       
        public async Task<List<Assignment>> GetInstructorAssignmentsToBeGradedByCourse(Guid courseId, Guid instructorId)
        {
            var assignments = await _context.LearnerAssignments
                .Include(a => a.Learner)
                .Include(a => a.Assignment)
                .Where(a => a.Assignment.CreatedBy == instructorId.ToString()
                && a.Assignment.CourseId == courseId
                && a.Status == AssignmentStatus.Submitted)
                .Select(a => a.Assignment)
                .ToListAsync();
            return assignments;
        }

        public async Task<LearnerAssignment> GetLearnerAssignmentById(Guid learnerId, Guid assignmentId)
        {
            var learnerAssignment = _context.LearnerAssignments.Include(a => a.Learner).Include(a => a.Assignment)
                .Where(a => a.LearnerId == learnerId && a.AssignmentId == assignmentId)
                .SingleOrDefault();
            return learnerAssignment;
        }

        public async Task<List<Assignment>> GetLearnerAssignments(Guid learnerId)
        {
            var assignments = await _context.LearnerAssignments
                .Include(a => a.Learner)
                .Include(a => a.Assignment)
                .Select(a => a.Assignment)
                .ToListAsync();
            return assignments;
        }

        public async Task<List<Assignment>> GetLearnerAssignmentsByCourseAndStatus(Guid learnerId, Guid courseId, AssignmentStatus status)
        {
            var assignments = await _context.LearnerAssignments
                .Include(a => a.Learner)
                .Include(a => a.Assignment)
                .Where(a => a.LearnerId == learnerId 
                && a.Assignment.CourseId == courseId 
                && a.Status == status)
                .Select(a => a.Assignment)
                .ToListAsync();
            return assignments;
        }

        public async Task<List<Assignment>> GetLearnerAssignmentsToBeSubmittedByCourse(Guid learnerId, Guid courseId)
        {
            var assignments = await _context.LearnerAssignments
                .Include(a => a.Learner)
                .Include(a => a.Assignment)
                .Where(a => a.LearnerId == learnerId 
                && a.Assignment.CourseId == courseId 
                && a.Status == AssignmentStatus.Assigned)
                .Select(a => a.Assignment)
                .ToListAsync();
            return assignments;
        }

        public async Task<List<Assignment>> GetSelectedAssignments(List<Guid> ids)
        {
            var assignments = await _context.Assignments
                .Include(a => a.Course)
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
            return assignments;
        }

        public async Task<LearnerAssignment> UpdateLearnerAssignment(LearnerAssignment learnerAssignment)
        {
             _context.LearnerAssignments.Update(learnerAssignment);
            await _context.SaveChangesAsync();
            return learnerAssignment;
        }
    }
}
