using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.ResultViewModel;

namespace Persistence.Implementations.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILearnerRepository _learnerRepository;

        public ResultService(IResultRepository resultRepository, IOptionRepository optionRepository, IQuestionRepository questionRepository, IAssessmentRepository assessmentRepository, ILearnerRepository learnerRepository)
        {
            _resultRepository = resultRepository;
            _optionRepository = optionRepository;
            _questionRepository = questionRepository;
            _assessmentRepository = assessmentRepository;
            _learnerRepository = learnerRepository;
        }


        public async Task<BaseResponse> GenerateResult(MarkAssessmentRequestModel model)
        {
            var questions = new List<Question>();
            var obtainedPoints= 0;
            var totalPoints = 0;
            var options = _optionRepository.GetSelectedOptions(model.OptionIds).ToList();
            var assessment = await _assessmentRepository.GetAsync(model.AssessmentId);
            var learner = await _learnerRepository.GetAsync(model.LearnerId);

            foreach (var option in options)
            {
                var question = _questionRepository.GetQuestionsById(option.QuestionId);
                questions.Add(question);
            }

            for(int i = 0; i < questions.Count; i++)
            {
                totalPoints += questions[i].Points;
                if(options[i].Status == OptionStatus.Correct)
                {
                    obtainedPoints += options[i].Question.Points;
                }
            }

            var result = new Result
            {
                Assessment = assessment,
                AssessmentId = model.AssessmentId,
                Learner = learner,
                LearnerId = model.LearnerId,
                ObtainedMarks = (obtainedPoints / totalPoints) * 100,
                TotalMarks = 100
            };
            await _resultRepository.AddAsync(result);
            await _resultRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{assessment.Title} result for {learner.FirstName} {learner.LastName} generated successfully",
                Status = true
            };
        }

        public async Task<ResultsResponseModel> GetAllResults()
        {
            var results = await _resultRepository.GetAllAsync(a => a.Id.ToString().Length != 0);
            var resultsReturned = results.Select(u => new ResultDTO
            {
               AssessmentName = u.Assessment.Title, 
               LearnerName = $"{u.Learner.FirstName} {u.Learner.LastName}",
               ObtainedMarks = u.ObtainedMarks,
               TotalMarks = u.TotalMarks               
            });

            if(results.Count == 0)
            {
                return new ResultsResponseModel
                {
                    Data = null,
                    Message = "No results found",
                    Status = false
                };
            }

            return new ResultsResponseModel
            {
                Data = resultsReturned,
                Message = $"{results.Count} results retrieved",
                Status = true
            };

        }

        public async Task<ResultsResponseModel> GetResultsByCourse(Guid courseId)
        {
            var results = await _resultRepository.Query().Include(o => o.Assessment)
                .ThenInclude(s => s.Module).Include(a => a.Learner)
                .Where(d => d.Assessment.Module.CourseId == courseId)
                .OrderByDescending(a => a.ObtainedMarks)
                .Select(c => new ResultDTO
                {
                    AssessmentName = c.Assessment.Title,
                    LearnerName = $"{c.Learner.FirstName} {c.Learner.LastName}",
                    ObtainedMarks = c.ObtainedMarks,
                    TotalMarks = c.TotalMarks
                }).ToListAsync();

            if (results.Count == 0)
            {
                return new ResultsResponseModel
                {
                    Data = null,
                    Message = "No results found",
                    Status = false
                };
            }

            return new ResultsResponseModel
            {
                Data = results,
                Message = $"{results.Count} results retrieved",
                Status = true
            };
        }

        public async Task<ResultsResponseModel> GetResultsByLearner(Guid learnerId)
        {
            var results = await _resultRepository.Query().Include(o => o.Assessment)
                .Include(a => a.Learner)
                .Where(d => d.LearnerId == learnerId).OrderByDescending(n => n.Created)
                .Select(c => new ResultDTO
                {
                    AssessmentName = c.Assessment.Title,
                    LearnerName = $"{c.Learner.FirstName} {c.Learner.LastName}",
                    ObtainedMarks = c.ObtainedMarks,
                    TotalMarks = c.TotalMarks
                }).ToListAsync();

            if (results.Count == 0)
            {
                return new ResultsResponseModel
                {
                    Data = null,
                    Message = "No results found",
                    Status = false
                };
            }

            return new ResultsResponseModel
            {
                Data = results,
                Message = $"{results.Count} results retrieved",
                Status = true
            };
        }
    }
}
