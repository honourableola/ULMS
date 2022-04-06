using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.QuestionViewModel;

namespace Persistence.Implementations.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IModuleRepository _moduleRepository;

        public QuestionService(IQuestionRepository questionRepository, IModuleRepository moduleRepository)
        {
            _questionRepository = questionRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<BaseResponse> AddQuestion(CreateQuestionRequestModel model)
        {
            var questionExist = await _questionRepository.GetAsync(q => q.QuestionText == model.QuestionText);

            if (questionExist != null)
            {
                throw new BadRequestException($"Question cannot be added as it already exist");
            }
            //Write query for correct answer

            var question = new Question
            {
                Id = Guid.NewGuid(),
                Points = model.Points,
                QuestionText = model.QuestionText,
                ModuleId = model.ModuleId,
                Created = DateTime.Now                
            };

            await _questionRepository.AddAsync(question);
            await _questionRepository.SaveChangesAsync();
            return new BaseResponse
            {
                Message = $"Question created successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> DeleteQuestion(Guid id)
        {
            var question = await _questionRepository.GetAsync(id);
            if (question == null)
            {
                throw new NotFoundException($"Question with id {id} does not exist");
            }

            await _questionRepository.DeleteAsync(question);
            await _questionRepository.SaveChangesAsync();
            return new BaseResponse
            {
                Message = $"Question deleted successfully",
                Status = true
            };
        }

        public async Task<QuestionsResponseModel> GetAllQuestions()
        {
            var questions = await _questionRepository.Query().Include(q => q.Module).ToListAsync();

                var questionsReturned = questions.Select(question => new QuestionDTO
               {
                   Id = question.Id,
                   QuestionText = question.QuestionText,
                   Points = question.Points,
                   ModuleName = question.Module.Name,
                   ModuleId = question.ModuleId,
                   Options = question.Options.Select(o => new OptionDTO
                   {
                       Id = o.Id,
                       OptionText = o.OptionText,
                       Label = o.Label,
                       QuestionId = o.QuestionId,
                       Status = o.Status,
                   }).ToList()
               }).ToList();

            if (questions.Count() == 0)
            {
                return new QuestionsResponseModel
                {
                    Data = null,
                    Message = $"No question found",
                    Status = false
                };
            }
            return new QuestionsResponseModel
            {
                Data = questionsReturned,
                Message = $"{questions.Count} Questions retrieved successfully",
                Status = true
            };
        }

        public async Task<QuestionResponseModel> GetQuestionById(Guid id)
        {
            var question = await _questionRepository.Query().Include(q => q.Module).SingleOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new NotFoundException($"question with id {id} does not exist");
            }

            return new QuestionResponseModel
            {
                Data = new QuestionDTO
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    Points = question.Points,
                    ModuleId = question.ModuleId,
                    ModuleName = question.Module.Name,
                    Options = question.Options.Select(o => new OptionDTO
                    {
                        Id = o.Id,
                        OptionText = o.OptionText,
                        Label = o.Label,
                        QuestionId = o.QuestionId,
                        Status = o.Status,
                    }).ToList()
                },
                Message = $"Question retrieved successfully",
                Status = true
            };
        }

        public async Task<QuestionsResponseModel> GetQuestionsByModule(Guid moduleId)
        {
            var questions =  _questionRepository.Query().Include(q => q.Module).Where(q => q.ModuleId == moduleId);
                var req = questions.Select(question => new QuestionDTO
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    Points = question.Points,
                    ModuleId = question.ModuleId,
                    ModuleName = question.Module.Name,
                    Options = question.Options.Select(o => new OptionDTO
                    {
                        Id = o.Id,
                        OptionText = o.OptionText,
                        Label = o.Label,
                        QuestionId = o.QuestionId,
                        Status = o.Status,
                    }).ToList()
                }).ToList();
           /* var questions = await _questionRepository.GetAllAsync(a => a.ModuleId == moduleId);

               var req = questions.Select(question => new QuestionDTO
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    Points = question.Points,
                    ModuleId = question.ModuleId
                }).ToList();*/

            if (questions.Count() == 0)
            {
                return new QuestionsResponseModel
                {
                    Data = null,
                    Message = $"No question found",
                    Status = false
                };
            }

            var module = await _moduleRepository.GetAsync(moduleId);
            return new QuestionsResponseModel
            {
                Data = req,
                Message = $"{req.Count} Questions for {module.Name} retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> UpdateQuestion(Guid id, UpdateQuestionRequestModel model)
        {
            var question = await _questionRepository.GetAsync(id);
            if (question == null)
            {
                throw new NotFoundException($"Question with id {id} not found");
            }

            question.QuestionText = model.QuestionText;
            question.Points = model.Points;

            await _questionRepository.UpdateAsync(question);
            await _questionRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Question updated successfully",
                Status = true
            };
        }
    }
}
