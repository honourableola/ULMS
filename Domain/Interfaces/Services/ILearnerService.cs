using Domain.Models;
using System;
using System.Threading.Tasks;
using static Domain.Models.LearnerViewModel;

namespace Domain.Interfaces.Services
{
    public interface ILearnerService
    {
        public Task<BaseResponse> AddLearner(CreateLearnerRequestModel model);
        public Task<BaseResponse> UpdateLearner(Guid id, UpdateLearnerRequestModel model);
        public Task<BaseResponse> DeleteLearner(Guid id);
        public Task<LearnerResponseModel> GetLearnerById(Guid id);
        public Task<LearnerResponseModel> GetLearnerByEmail(string email);
        public Task<LearnersResponseModel> SearchLearnersByName(string searchText);
        public Task<LearnersResponseModel> GetLearnersByCourse(Guid courseId);
        public Task<LearnersResponseModel> GetAllLearners();
    }
}
