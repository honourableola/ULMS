using Domain.Models;
using System;
using System.Threading.Tasks;
using static Domain.Models.TopicViewModel;

namespace Domain.Interfaces.Services
{
    public interface ITopicService
    {
        public Task<BaseResponse> AddTopic(CreateTopicRequestModel model);
        public Task<BaseResponse> UpdateTopic(Guid id, UpdateTopicRequestModel model);
        public Task<BaseResponse> DeleteTopic(Guid id);
        public Task<TopicResponseModel> GetTopic(Guid id);
        public Task<TopicsResponseModel> GetAllTopics();
        public Task<TopicsResponseModel> GetTopicsByModule(Guid moduleId);
        public Task<TopicsResponseModel> GetTakenTopicsByModule(Guid moduleId);
        public Task<TopicsResponseModel> GetNotTakenTopicsByModule(Guid moduleId);
        public Task<TopicsResponseModel> SearchTopicsByTitle(string searchText);
        public bool IsAllModuleTopicsTaken(Guid moduleId);
    }
}
