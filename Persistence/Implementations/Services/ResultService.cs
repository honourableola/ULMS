using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.ResultViewModel;

namespace Persistence.Implementations.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;

        public ResultService(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public Task<BaseResponse> DeleteResult(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GenerateResult(CreateResultRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsResponseModel> GetAllResults()
        {
            throw new NotImplementedException();
        }

        public Task<ResultsResponseModel> GetResultsByCourse(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsResponseModel> GetResultsByLearner(Guid learnerId)
        {
            throw new NotImplementedException();
        }
    }
}
