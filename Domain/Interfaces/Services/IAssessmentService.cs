using Domain.Models;
using System;
using System.Threading.Tasks;
using static Domain.Models.AssessmentViewModel;

namespace Domain.Interfaces.Services
{
    public interface IAssessmentService
    {
        public Task<BaseResponse> GenerateAssessment(Guid learnerId);
        public Task<BaseResponse> DeleteAssessment(Guid id);
        public Task<AssessmentResponseModel> GetAssessment(Guid id);
        public Task<AssessmentsResponseModel> GetAllAssessments();
        public Task<AssessmentsResponseModel> GetAssessmentsByCourse(Guid courseId);
    }
}
