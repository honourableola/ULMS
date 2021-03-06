using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Identity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Domain.Models.AssessmentViewModel;

namespace Persistence.Implementations.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IQuestionRepository _questionRepository;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICourseConstantService _courseConstantService;
        private readonly IModuleService _moduleService;
        //private readonly IIdentityService _identityService;

        public AssessmentService(IAssessmentRepository assessmentRepository,/* IHttpContextAccessor httpContextAccessor,*/ ICourseRepository courseRepository, IQuestionRepository questionRepository, ICourseConstantService courseConstantService, IModuleService moduleService /*IIdentityService identityService*/)
        {
            _assessmentRepository = assessmentRepository;
            _courseRepository = courseRepository;
            _questionRepository = questionRepository;
            //_httpContextAccessor = httpContextAccessor;
            _courseConstantService = courseConstantService;
            _moduleService = moduleService;
           // _moduleRepository = moduleRepository;
            //_identityService = identityService;
        }

        public async Task<BaseResponse> GenerateAssessment(Guid learnerId)
        {
            var modules = await _moduleService.GetTakenModulesByLearnerWithNoAssessment(learnerId);
            var courseConstant =  _courseConstantService.GetCourseConstant();
            //var signedInUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
 
            foreach(var module in modules)
            {
                var assessment = new Assessment
                {
                    Id = Guid.NewGuid(),
                    Title = "Assessment for " + module.Name,
                    Description = $"This assessment tests the understanding of learner on {module.Name}",
                    DurationInMinutes = courseConstant.DurationOfAssessment,
                    ModuleId = module.Id
                };
                var questions = GenerateAssessmentQuestions(module.Id);
                assessment.Questions = questions;

                await _assessmentRepository.AddAsync(assessment);
                await _assessmentRepository.SaveChangesAsync();
                module.AssessmentGenerated = true;
                await _assessmentRepository.SaveChangesAsync();
            }
                     
            return new BaseResponse
            {
                Message = $"assessments generated successfully",
                Status = true
            }; 
        }

        public async Task<BaseResponse> DeleteAssessment(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            if(assessment == null)
            {
                throw new NotFoundException($"Assessment with id {id} does not exist");
            }

            await _assessmentRepository.DeleteAsync(assessment);
            await _assessmentRepository.SaveChangesAsync();
            return new BaseResponse
            {
                Message = $"{assessment.Title} deleted successfully",
                Status = true
            };
        }

        public async Task<AssessmentsResponseModel> GetAllAssessments()
        {
            var assessments = await _assessmentRepository
                .Query()
                .Select(a => new AssessmentDTO
                {
                    Id = a.Id,
                    Description = a.Description,
                    ModeuleId = a.ModuleId,
                    Title = a.Title,
                    DurationInMinutes = a.DurationInMinutes                   
                }).ToListAsync();

            if (assessments.Count == 0)
            {
                return new AssessmentsResponseModel
                {
                    Data = null,
                    Message = $"No assessment found",
                    Status = false
                };
            }
            return new AssessmentsResponseModel
            {
                Data = assessments,
                Message = $"{assessments.Count} Assessments retrieved successfully",
                Status = true
            };
        }

        public async Task<AssessmentResponseModel> GetAssessment(Guid id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            if (assessment == null)
            {
                throw new NotFoundException($"assessment with id {id} does not exist");
            }

            return new AssessmentResponseModel
            {
                Data = new AssessmentDTO
                {
                    Id = assessment.Id,
                    Description = assessment.Description,
                    ModeuleId = assessment.ModuleId,
                    Title = assessment.Title,
                    DurationInMinutes = assessment.DurationInMinutes
                },
                Message = $"Assessment retrieved successfully",
                Status = true
            };
        }

        public async Task<AssessmentsResponseModel> GetAssessmentsByCourse(Guid courseId)
        {
            var assessments = await _assessmentRepository.Query()
                .Include(a => a.Module)
                .Where(a => a.Module.CourseId == courseId)
                .Select(assessment => new AssessmentDTO
                {
                    Id = assessment.Id,
                    Description = assessment.Description,
                    ModeuleId = assessment.ModuleId,
                    Title = assessment.Title,
                    DurationInMinutes = assessment.DurationInMinutes
                }).ToListAsync();

            var course = await _courseRepository.GetAsync(courseId);
            return new AssessmentsResponseModel
            {
                Data = assessments,
                Message = $"Assessments for {course.Name} retrieved successfully",
                Status = true
            };
        }

        /*public async Task<BaseResponse> UpdateAssessment(Guid id, UpdateAssessmentRequestModel model)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            if (assessment == null)
            {
                throw new NotFoundException($"Assessment with id {id} not found");
            }

            assessment.DurationInMinutes = model.DurationInMinutes;

            await _assessmentRepository.UpdateAsync(assessment);
            await _assessmentRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{assessment.Title} updated successfully",
                Status = true
            };
        }*/

        private List<Question> GenerateAssessmentQuestions(Guid moduleId)
        {
            List<Question> assessmentQuestions = new List<Question>();
            List<int> ids = new List<int>();
            var courseConstant =   _courseConstantService.GetCourseConstant();
            var questions =  _questionRepository.GetQuestionsByModule(moduleId);
            if(courseConstant.NoOfAssessmentQuestions > questions.Count)
            {
                throw new BadRequestException($"Questions available in the question bank is not enough to generate the assessment. Kindly add more questions to the module or reduce the assessment questions");
            }
            Random random = new Random();
            int randomId = 0;
            for(int i = 1; i < courseConstant.NoOfAssessmentQuestions; i++)
            {
                randomId = random.Next(0, questions.Count() - 1);
                while (ids.Contains(randomId))
                {
                    randomId = random.Next(1, questions.Count() - 1);
                }
                ids.Add(randomId);
            }

            for(int i = 0; i < courseConstant.NoOfAssessmentQuestions; i++)
            {
                questions[ids[i]].QuestionNumber = i + 1;
                assessmentQuestions.Add(questions[ids[i]]);
            }
            return assessmentQuestions;
        }
    }
}
