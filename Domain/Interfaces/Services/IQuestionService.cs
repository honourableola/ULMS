using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.QuestionViewModel;

namespace Domain.Interfaces.Services
{
    public interface IQuestionService
    {
        public Task<BaseResponse> AddQuestion(CreateQuestionRequestModel model);
        public Task<BaseResponse> UpdateQuestion(Guid id, UpdateQuestionRequestModel model);
        public Task<BaseResponse> DeleteQuestion(Guid id);
        public Task<QuestionResponseModel> GetQuestionById(Guid id);
        public Task<QuestionsResponseModel> GetQuestionsByModule(Guid moduleId);
        public Task<QuestionsResponseModel> GetAllQuestions();
    }
}
