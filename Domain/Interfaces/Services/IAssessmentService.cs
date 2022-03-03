using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.AssessmentViewModel;

namespace Domain.Interfaces.Services
{
    public interface IAssessmentService
    {
        public Task<BaseResponse> GenerateAssessment(CreateAssessmentRequestModel model);
        public Task<BaseResponse> UpdateAssessment(Guid id, UpdateAssessmentRequestModel model);
        public Task<BaseResponse> DeleteAssessment(Guid id);
        public Task<AssessmentResponseModel> GetAssessment(Guid id);
        public Task<AssessmentsResponseModel> GetAllAssessments();
        public Task<AssessmentsResponseModel> GetAssessmentsByCourse(Guid courseId);
    }
}
