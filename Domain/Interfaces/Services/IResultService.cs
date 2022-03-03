using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.ResultViewModel;

namespace Domain.Interfaces.Services
{
    public interface IResultService
    {
        public Task<BaseResponse> GenerateResult(CreateResultRequestModel model);
/*        public Task<BaseResponse> UpdateAssessment(Guid id, UpdateAssessmentRequestModel model);*/
        public Task<BaseResponse> DeleteResult(Guid id);
        public Task<ResultsResponseModel> GetResultsByLearner(Guid learnerId);
        public Task<ResultsResponseModel> GetAllResults();
        public Task<ResultsResponseModel> GetResultsByCourse(Guid courseId);
    }
}
