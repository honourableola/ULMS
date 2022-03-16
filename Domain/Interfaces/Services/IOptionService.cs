using Domain.Models;
using System;
using System.Threading.Tasks;
using static Domain.Models.OptionViewModel;

namespace Domain.Interfaces.Services
{
    public interface IOptionService
    {
        public Task<BaseResponse> AddOption(CreateOptionRequestModel model);
        public Task<BaseResponse> UpdateOption(Guid id, UpdateOptionRequestModel model);
        public Task<BaseResponse> DeleteOption(Guid id);
        public Task<OptionResponseModel> GetOption(Guid id);
        public Task<OptionsResponseModel> GetOptionsByQuestion(Guid questionId);
    }
}
