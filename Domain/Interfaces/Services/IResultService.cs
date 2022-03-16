using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Domain.Models.ResultViewModel;

namespace Domain.Interfaces.Services
{
    public interface IResultService
    {
        public Task<BaseResponse> GenerateResult(MarkAssessmentRequestModel model);
        public Task<ResultsResponseModel> GetResultsByLearner(Guid learnerId);
        public Task<ResultsResponseModel> GetAllResults();
        public Task<ResultsResponseModel> GetResultsByCourse(Guid courseId);
    }
}
