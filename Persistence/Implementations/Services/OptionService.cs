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
using static Domain.Models.OptionViewModel;

namespace Persistence.Implementations.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;
        public OptionService(IOptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        public async Task<BaseResponse> AddOption(CreateOptionRequestModel model)
        {
            var optionExist = await _optionRepository.GetAsync(o => o.Label == model.Label && o.OptionText == model.OptionText);
            if(optionExist != null)
            {
                throw new BadRequestException("Option already exist");
            }

            var option = new Option
            {
                Id = Guid.NewGuid(),
                Label = model.Label,
                OptionText = model.OptionText,
                Status = model.Status,
                QuestionId = model.QuestionId
            };

            await _optionRepository.AddAsync(option);
            await _optionRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = "Option created successfully",
                Status = true

            };
        }

        public async Task<BaseResponse> DeleteOption(Guid id)
        {
            var option = await _optionRepository.GetAsync(id);
            if(option == null)
            {
                throw new NotFoundException($"Option with id {id} does not exist");
            }

            await _optionRepository.DeleteAsync(option);
            await _optionRepository.SaveChangesAsync();
            return new BaseResponse
            {
                Message = "Option deleted succcessfully",
                Status = true
            };

        }


        public async Task<OptionResponseModel> GetOption(Guid id)
        {
            var option = await _optionRepository.Query()
                .Include(o => o.Question)
                .SingleOrDefaultAsync(o => o.Id == id);
            if (option == null)
            {
                throw new NotFoundException($"Option with id {id} does not exist");
            }

            return new OptionResponseModel
            {
                Data = new OptionDTO
                {
                    Label = option.Label,
                    OptionText = option.OptionText,
                    Status = option.Status,
                    QuestionId = option.QuestionId
                },
                Message = "Option added successfully",
                Status = true
            };

        }

        public async Task<OptionsResponseModel> GetOptionsByQuestion(Guid questionId)
        {
            var options = await _optionRepository.Query()
                .Include(o => o.Question)
                .Where(o => o.QuestionId == questionId)
                .Select(o => new OptionDTO 
                { 
                    Id = o.Id,
                    Label = o.Label,
                    OptionText = o.OptionText,
                    Status = o.Status,
                    QuestionId = o.QuestionId                  
                }).ToListAsync();

            if (options == null)
            {
                throw new BadRequestException($"Options not found");
            }
            else if (options.Count == 0)
            {
                return new OptionsResponseModel
                {
                    Message = $" No Option Found",
                    Status = true
                };
            }

            return new OptionsResponseModel
            {
                Data = options,
                Message = $"{options.Count} Options retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> UpdateOption(Guid id, UpdateOptionRequestModel model)
        {
            var option = await _optionRepository.GetAsync(id);
            if (option == null)
            {
                throw new NotFoundException($"Option with id {id} does not exist");
            }

            option.OptionText = model.OptionText;
            option.Label = model.Label;
            option.Status = model.Status;
            await _optionRepository.UpdateAsync(option);
            await _optionRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = "Option updated successfully",
                Status = true
            };

        }
    }
}
