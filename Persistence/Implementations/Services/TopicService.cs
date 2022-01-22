using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.TopicViewModel;

namespace Persistence.Implementations.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<BaseResponse> AddTopic(CreateTopicRequestModel model)
        {
            var topicExist = await _topicRepository.ExistsAsync(a => a.Title == model.Title);

            if (topicExist)
            {
                throw new BadRequestException($"{model.Title} already exist and cannot be added");
            }

            var topic = new Topic
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Content = model.Content,
                ModuleId = model.ModuleId,
                Created = DateTime.UtcNow
            };

            await _topicRepository.AddAsync(topic);
            await _topicRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.Title} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteTopic(Guid id)
        {
            var topicExist = await _topicRepository.ExistsAsync(id);
            if (!topicExist)
            {
                throw new BadRequestException($"Topic with id {id} does not exist");
            }

            var topic = await _topicRepository.GetAsync(id);
            await _topicRepository.DeleteAsync(topic);
            await _topicRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{topic.Title} deleted successfully"
            };
        }
  

        public async  Task<TopicsResponseModel> GetAllTopics()
        {
        var topics = await _topicRepository.Query()
           .Select(n => new TopicDTO
           {
               Id = n.Id,
               Title = n.Title,
               Content = n.Content,
               ModuleName = n.Module.Name
             
           }).ToListAsync();

        return new TopicsResponseModel
        {
            Data = topics,
            Message = $"Topics retrieved successfully",
            Status = true
        };
    }

        public async Task<TopicResponseModel> GetTopic(Guid id)
        {
            var topic = await _topicRepository.Query()
                .SingleOrDefaultAsync(a => a.Id == id);

            return new TopicResponseModel
            {
                Data = new TopicDTO
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Content = topic.Content,
                    ModuleName = topic.Module.Name
                
                },
                Message = $"Topic retrieved successfully",
                Status = true
            };
        }

        public async Task<TopicsResponseModel> GetTopicsByModule(Guid moduleId)
        {
            var topics = await _topicRepository.Query()
                .Where(c => c.ModuleId == moduleId)
                .Select(n => new TopicDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ModuleName = n.Module.Name
                }).ToListAsync();


            return new TopicsResponseModel
            {
                Data = topics,
                Message = $"Topics retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> UpdateTopic(Guid id, UpdateTopicRequestModel model)
        {
            var topic = await _topicRepository.GetAsync(id);
            if (topic == null)
            {
                throw new NotFoundException($"Topic with id {id} not found");
            }

            topic.Title = model.Title;
            topic.Content = model.Content;
            topic.Modified = DateTime.UtcNow;
            await _topicRepository.UpdateAsync(topic);
            await _topicRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{topic.Title} updated successfully",
                Status = true
            };
        }
    }
}
