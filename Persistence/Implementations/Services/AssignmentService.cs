using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Integrations.Email;
using Persistence.Integrations.MailKitModels.MailTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.AssignmentViewModel;

namespace Persistence.Implementations.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        public AssignmentService(IAssignmentRepository assignmentRepository, ICourseRepository courseRepository, IInstructorRepository instructorRepository, ILearnerRepository learnerRepository, IMailService mailService, IUserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _learnerRepository = learnerRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        //Upload of AssignmentPDF during creation

        //Upload of pdf during submission by learner
        public async Task<BaseResponse> AssignAssignmentToLearner(AssignAssignmentRequestModel model)
        {
            var learner = await _learnerRepository.GetAsync(model.LearnerId);
            if (learner == null)
            {
                throw new NotFoundException("Selected learner does not exist");
            }

            var selectedAssignments = await _assignmentRepository.GetSelectedAssignments(model.AssignmentIds);
            //var alreadyAssigned = await GetAssignmentsByLearner(learner.Id);
            if(selectedAssignments == null || selectedAssignments.Count == 0)
            {
                throw new NotFoundException("Selected assignemnts not found");
            }

            int noOfDuplicatedAssignments = 0;
            var successfullyAssigned = new List<Assignment>();
            foreach (var assignment in selectedAssignments)
            {               
                /*if (alreadyAssigned.Data != null)
                {
                    bool existing = alreadyAssigned.Data.Any(c => c.Id == assignment.Id);
                    if (existing)
                    {
                        continue;
                    }                   
                }*/
                var learnerAssignment = new LearnerAssignment
                {
                    Assignment = assignment,
                    AssignmentId = assignment.Id,
                    Learner = learner,
                    LearnerId = learner.Id,
                    Status = AssignmentStatus.Assigned
                };
                successfullyAssigned.Add(assignment);
                await _assignmentRepository.AddLearnerAssignment(learnerAssignment);
                /*if (alreadyAssigned.Data.Any(c => c.Id == assignment.Id))
                {
                    noOfDuplicatedAssignments++;
                }*/
            }
            await _assignmentRepository.SaveChangesAsync();
            var assignmentNames = SelectedAssignmentsNames(successfullyAssigned);

            var request = new AssignmentSuccessfullyAssigned
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                AssignmentNames = assignmentNames,
                ToEmail = learner.Email
            };

            await _mailService.SendAssignmentSuccessfullyAssignedToLearnerEmailAsync(request);
            return new BaseResponse
            {
                Message = $"{successfullyAssigned.Count} Assignments successfully assigned to {learner.FirstName} {learner.LastName} while {noOfDuplicatedAssignments} assignments were rejected because they were already assigned to the Learner",
                Status = true
            };
        }
        private string SelectedAssignmentsNames(IEnumerable<Assignment> assignments)
        {
            var names = "";
            foreach (var assignment in assignments)
            {
                names += assignment.Name + ", ";
            }
            return names;
        }
        public async Task<BaseResponse> CreateAssignment(Guid userId, CreateAssignmentRequestModel model)
        {
            var user = await _userRepository.GetAsync(userId);
            var instructor = await _instructorRepository.GetAsync(a => a.Email == user.Email);
            var courseExist = await _courseRepository.ExistsAsync(model.CourseId);
            if (!courseExist)
            {
                throw new BadRequestException($"Course with Id {model.CourseId} selected does not exist");
            }

            var assignmentExist = await _assignmentRepository.GetAssignmentByNameAndCourseId(model.Name, model.CourseId);
            if(assignmentExist != null)
            {
                throw new BadRequestException($"Assignment {model.Name} already exist and cannot be created");
            }

            var assignment = new Assignment
            {
                Name = model.Name,
                AssignmentContent = model.AssignmentContent,
                CourseId = model.CourseId,
                InstructorId = instructor.Id
            };
            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Assignment created successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> DeleteAssignment(Guid assignmentId)
        {
            var assignment = await _assignmentRepository.GetAsync(assignmentId);
            if(assignment == null)
            {
                throw new NotFoundException($"Assignment with Id {assignmentId} does not exist");
            }
            await _assignmentRepository.DeleteAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();
            return new BaseResponse
            {
                Message = $"Assignment deleted successfully",
                Status = true
            };
        }

        public async Task<AssignmentsResponseModel> FilterInstructorAssignmentsByCourse(Guid userId, Guid courseId)
        {
            var user = await _userRepository.GetAsync(userId);
            var instructor = await _instructorRepository.GetAsync(a => a.Email == user.Email);
            var course = await _courseRepository.GetAsync(courseId);    
            if(course == null)
            {
                throw new BadRequestException($"Selected course with id {courseId} does not exist");
            }
            var assigments = await _assignmentRepository.Query()
                .Include(a => a.Course)
                .Where(a => a.InstructorId == instructor.Id && a.CourseId == courseId).Select(a => new AssignmentDTO
                {
                    Id = a.Id,
                    AssignmentContent = a.AssignmentContent,
                    AssignmentPdfUpload = a.AssignmentPdfUpload,
                    CourseId = a.CourseId,
                    CourseName = a.Course.Name,
                    Name = a.Name,
                    ResponseContent = a.ResponseContent,
                    ResponsePdfUpload = a.ResponsePdfUpload,
                    LearnerScore = a.LearnerScore                   
                }).ToListAsync();

            if(assigments == null || assigments.Count == 0)
            {
                throw new BadRequestException($"Assignments not found");
            }
            return new AssignmentsResponseModel
            {
                Data = assigments,
                Message = $"{assigments.Count} assignments for {course.Name} retrieved successfully",
                Status = true
            };
        }

        public async Task<AssignmentsResponseModel> FilterLearnerAssignmentsByCourseAndStatus(Guid userId, FilterLearnerAssignmentsRequestModel model)
        {
            var user = await _userRepository.GetAsync(userId);
            var learner = await _learnerRepository.GetAsync(a => a.Email == user.Email);
            var course = await _courseRepository.GetAsync(model.CourseId);
            if (course == null)
            {
                throw new BadRequestException($"Selected course with id {model.CourseId} does not exist");
            }

            var assignments = await _assignmentRepository.GetLearnerAssignmentsByCourseAndStatus(learner.Id, model.CourseId, model.Status);
            if (assignments == null || assignments.Count == 0)
            {
                throw new BadRequestException($"Assignments not found");
            }
            var assignmentReturned = assignments.Select(a => new AssignmentDTO
            {
                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            if (assignments == null || assignments.Count == 0)
            {
                throw new BadRequestException($"Assignments not found");
            }
            return new AssignmentsResponseModel
            {
                Data = assignmentReturned,
                Message = $"{assignmentReturned.Count} assignments for {course.Name} retrieved successfully",
                Status = true
            };
        }

        public async Task<AssignmentResponseModel> GetAssignmentById(Guid assignmentId)
        {
            var assignment = await _assignmentRepository.Query()
                .Include(a => a.Course)
                .Include(a => a.Instructor)
                .SingleOrDefaultAsync(a => a.Id == assignmentId);
            if (assignment == null)
            {
                throw new NotFoundException($"Assignment with id {assignmentId} does not exist");
            }

            return new AssignmentResponseModel
            {
                Data = new AssignmentDTO
                {

                    Id = assignment.Id,
                    AssignmentContent = assignment.AssignmentContent,
                    AssignmentPdfUpload = assignment.AssignmentPdfUpload,
                    CourseId = assignment.CourseId,
                    CourseName = assignment.Course.Name,
                    Name = assignment.Name,
                    ResponseContent = assignment.ResponseContent,
                    ResponsePdfUpload = assignment.ResponsePdfUpload,
                    LearnerScore = assignment.LearnerScore
                },
                Message = $"Assignment retrieved successsfully",
                Status = true
            };      
        }

        public async Task<AssignmentsResponseModel> GetAssignmentsByCourse(Guid courseId)
        {
            var course = await _courseRepository.GetAsync(courseId);
            if (course == null)
            {
                throw new BadRequestException($"Selected course with id {courseId} does not exist");
            }

            var assignments = await _assignmentRepository.Query()
                .Include(a => a.Course)
                .Include(a => a.Instructor)
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
            if (assignments == null || assignments.Count == 0)
            {
                throw new BadRequestException($"Assignments not found");
            }
            var assignmentsReturned = assignments.Select(a => new AssignmentDTO
            {
                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            return new AssignmentsResponseModel
            {
                Data = assignmentsReturned,
                Message = $"{assignments.Count} assignments retrieved",
                Status = true
            };
        }

        public async Task<AssignmentsResponseModel> GetAssignmentsByInstructor(Guid instructorId)
        {
            var course = await _instructorRepository.GetAsync(instructorId);
            if (course == null)
            {
                throw new BadRequestException($"Selected instructor with id {instructorId} does not exist");
            }

            var assignments = await _assignmentRepository.Query()
                .Include(a => a.Course)
                .Include(a => a.Instructor)
                .Where(a => a.InstructorId == instructorId)
                .ToListAsync();
            if (assignments == null || assignments.Count == 0)
            {
                throw new BadRequestException($"Assignments not found");
            }
            var assignmentsReturned = assignments.Select(a => new AssignmentDTO
            {

                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            return new AssignmentsResponseModel
            {
                Data = assignmentsReturned,
                Message = $"{assignments.Count} assignments retrieved",
                Status = true
            };
        }

        public async Task<AssignmentsResponseModel> GetAssignmentsByLearner(Guid learnerId)
        {
            var assignments = await _assignmentRepository.GetLearnerAssignments(learnerId);
            if (assignments == null || assignments.Count == 0)
            {
                return new AssignmentsResponseModel
                {
                    Data = null,
                    Message = $"No Assignments currently found for learner {learnerId}",
                    Status = true
                };
            }
            var assignmentsReturned = assignments.Select(a => new AssignmentDTO
            {
                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            return new AssignmentsResponseModel
            {
                Data = assignmentsReturned,
                Message = $"{assignments.Count} assignments retrieved",
                Status = true
            };
        }

        public async Task<BaseResponse> GradeAssignment(AssessAssignmentRequestModel model)
        {
            var learnerAssignment = await _assignmentRepository.GetLearnerAssignmentById(model.learnerId, model.AssignmentId);
            if (learnerAssignment == null)
            {
                throw new NotFoundException($"Assignment with id {model.AssignmentId} and learner Id {model.learnerId} not found");
            }
            learnerAssignment.Assignment.LearnerScore = model.LearnerScore;
            learnerAssignment.Status = AssignmentStatus.Assessed;
            await _assignmentRepository.UpdateLearnerAssignment(learnerAssignment);
            await _assignmentRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = "Assignment Graded successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> SubmitAssignment(Guid userId, SubmitAssignmentRequestModel model)
        {
            var user = await _userRepository.GetAsync(userId);
            var learner = await _learnerRepository.GetAsync(a => a.Email == user.Email);
            var learnerAssignment = await _assignmentRepository.GetLearnerAssignmentById(learner.Id, model.AssignmentId);
            if(learnerAssignment == null)
            {
                throw new NotFoundException($"Assignment with id {model.AssignmentId} not found");
            }

            learnerAssignment.Assignment.ResponseContent = model.ResponseContent;
            learnerAssignment.Status = AssignmentStatus.Submitted;
            await _assignmentRepository.UpdateLearnerAssignment(learnerAssignment);
            await _assignmentRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = "Assignment submitted successfully",
                Status = true
            };          
        }

        public async Task<BaseResponse> UpdateAssignment(Guid assignmentId, UpdateAssignmentRequestModel model)
        {
            var assignment = await _assignmentRepository.GetAsync(assignmentId);
            if(assignment == null)
            {
                throw new NotFoundException($"Assignment with id {assignmentId} does not exist");
            }

            assignment.AssignmentContent = model.AssignmentContent;
            assignment.CourseId = model.CourseId;

            await _assignmentRepository.UpdateAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{assignment.Name} updated successfully",
                Status = true
            };           
        }

        public async Task<AssignmentsResponseModel> GetLearnerAssignmentsToBeSubmittedByCourse(Guid userId, Guid courseId)
        {
            var user = await _userRepository.GetAsync(userId);
            var learner = await _learnerRepository.GetAsync(a => a.Email == user.Email);
            var assignmentsToBeSubmitted = await _assignmentRepository.GetLearnerAssignmentsToBeSubmittedByCourse(learner.Id, courseId);
            if (assignmentsToBeSubmitted == null || assignmentsToBeSubmitted.Count == 0)
            {
                throw new BadRequestException($"No assignments due for submission found");
            }
            var assignmentsReturned = assignmentsToBeSubmitted.Select(a => new AssignmentDTO
            {
                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            return new AssignmentsResponseModel
            {
                Data = assignmentsReturned,
                Message = $"{assignmentsToBeSubmitted.Count} assignments retrieved",
                Status = true
            };
        }

        public async Task<AssignmentsResponseModel> GetInstructorAssignmentsToBeGradedByCourse(Guid userId, Guid courseId)
        {
            var user = await _userRepository.GetAsync(userId);
            var instructor = await _instructorRepository.GetAsync(a => a.Email == user.Email);
            var assignmentsToBeGraded = await _assignmentRepository.GetInstructorAssignmentsToBeGradedByCourse(courseId, instructor.Id);
            if (assignmentsToBeGraded == null || assignmentsToBeGraded.Count == 0)
            {
                throw new BadRequestException($"No assignments to be graded found for course id {courseId}");
            }
            var assignmentsReturned = assignmentsToBeGraded.Select(a => new AssignmentDTO
            {
                Id = a.Id,
                AssignmentContent = a.AssignmentContent,
                AssignmentPdfUpload = a.AssignmentPdfUpload,
                CourseId = a.CourseId,
                CourseName = a.Course.Name,
                Name = a.Name,
                ResponseContent = a.ResponseContent,
                ResponsePdfUpload = a.ResponsePdfUpload,
                LearnerScore = a.LearnerScore
            }).ToList();

            return new AssignmentsResponseModel
            {
                Data = assignmentsReturned,
                Message = $"{assignmentsToBeGraded.Count} assignments retrieved",
                Status = true
            };
        }
    }
}
