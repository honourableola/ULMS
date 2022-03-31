using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CourseConstantViewModel;

namespace Persistence.Implementations.Services
{
    public class CourseConstantService : ICourseConstantService
    {
        private readonly ICourseConstantRepository _courseConstantRepository;

        public CourseConstantService(ICourseConstantRepository courseConstantRepository)
        {
            _courseConstantRepository = courseConstantRepository;
        }

        public async Task<BaseResponse> AddCourseConstant(CreateCourseConstantRequestModel model)
        {
            var courseConstants = await _courseConstantRepository.GetAllAsync(a => a.Id != null);

            if (courseConstants.Count >= 1)
            {
                throw new BadRequestException($"Another record of Course Constant cannot be created, Kindly update the existing record");
            }

            var courseConstant = new CourseConstant
            {
                MaximumNoOfAdditionalCourses = model.MaximumNoOfAdditionalCourses,
                MaximumNoOfMajorCourses = model.MaximumNoOfMajorCourses,
                NoOfAssessmentQuestions = model.NoOfAssessmentQuestions,
                DurationOfAssessment = model.DurationOfAssessment
            };

            await _courseConstantRepository.AddAsync(courseConstant);
            await _courseConstantRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Course Constant record created successfully",
                Status = true
            };
        }

        public CourseConstant GetCourseConstant()
        {
            var courseConstants =  _courseConstantRepository.Query().Where(a => a.Id != null).ToList();
            if(courseConstants == null)
            {
                throw new BadRequestException("No record exist on the constants table");
            }
            var courseConstant = courseConstants[0];

            return courseConstant;
        }

        public async Task<BaseResponse> UpdateCourseConstant(UpdateCourseConstantRequestModel model)
        {
            var courseConstants = await _courseConstantRepository.GetAllAsync(a => a.Id != null);
            var courseConstant = courseConstants[0];

            courseConstant.MaximumNoOfAdditionalCourses = model.MaximumNoOfAdditionalCourses;
            courseConstant.MaximumNoOfMajorCourses = model.MaximumNoOfMajorCourses;
            courseConstant.NoOfAssessmentQuestions = model.NoOfAssessmentQuestions;
            courseConstant.DurationOfAssessment = model.DurationOfAssessment;
            

            await _courseConstantRepository.UpdateAsync(courseConstant);
            await _courseConstantRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Course Constant Record updated successfully",
                Status = true
            };
        }
    }
}
