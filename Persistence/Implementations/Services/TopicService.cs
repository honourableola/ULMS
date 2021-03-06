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
               ModuleName = n.Module.Name,
               ModuleId = n.ModuleId,
               IsTaken = n.IsTaken
             
           }).ToListAsync();

            if (topics == null)
            {
                throw new BadRequestException($"Topics not found");
            }
            else if (topics.Count == 0)
            {
                return new TopicsResponseModel
                {                   
                    Message = $" No Topic Found",
                    Status = true
                };
            }

            return new TopicsResponseModel
        {
            Data = topics,
            Message = $"{topics.Count} Topics retrieved successfully",
            Status = true
        };
    }

        public async Task<TopicsResponseModel> GetNotTakenTopicsByModule(Guid moduleId)
        {
            var topics = await _topicRepository.Query()
                .Where(c => c.ModuleId == moduleId && c.IsTaken == false)
                .Select(n => new TopicDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ModuleName = n.Module.Name,
                    ModuleId = n.ModuleId,
                    IsTaken = n.IsTaken

                }).ToListAsync();

            if (topics == null)
            {
                throw new BadRequestException($"Topics not found");
            }
            else if (topics.Count == 0)
            {
                return new TopicsResponseModel
                {
                    Message = $" No Topic Found",
                    Status = true
                };
            }
            return new TopicsResponseModel
            {
                Data = topics,
                Message = $"{topics.Count} Topics not yet completed retrieved successfully",
                Status = true
            };
        }

        public async Task<TopicsResponseModel> GetTakenTopicsByModule(Guid moduleId)
        {
            var topics = await _topicRepository.Query()
                .Where(c => c.ModuleId == moduleId && c.IsTaken == true)
                .Select(n => new TopicDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ModuleName = n.Module.Name,
                    ModuleId = n.ModuleId,
                    IsTaken = n.IsTaken

                }).ToListAsync();

            if (topics == null)
            {
                throw new BadRequestException($"Topics not found");
            }
            else if (topics.Count == 0)
            {
                return new TopicsResponseModel
                {
                    Message = $" No Topic Found",
                    Status = true
                };

            }
            return new TopicsResponseModel
            {
                Data = topics,
                Message = $"{topics.Count} Completed Topics retrieved successfully",
                Status = true
            };
        }

        public async Task<TopicResponseModel> GetTopic(Guid id)
        {
            var topic = await _topicRepository.Query()
                .Include(h => h.Module)
                .SingleOrDefaultAsync(a => a.Id == id);
            if(topic == null)
            {
                throw new BadRequestException($"Topic with id {id} does not exist");
            }

            return new TopicResponseModel
            {
                Data = new TopicDTO
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Content = topic.Content,
                    ModuleName = topic.Module.Name,
                    ModuleId = topic.ModuleId,
                    IsTaken = topic.IsTaken

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
                    ModuleName = n.Module.Name,
                    ModuleId = n.ModuleId,
                    IsTaken = n.IsTaken
                    
                }).ToListAsync();

            if (topics == null)
            {
                throw new BadRequestException($"Topics not found");
            }
            else if (topics.Count == 0)
            {
                return new TopicsResponseModel
                {              
                    Message = $" No Topic Found",
                    Status = true
                };          

        }
            return new TopicsResponseModel
            {
                Data = topics,
                Message = $"{topics.Count} Topics retrieved successfully",
                Status = true
            };
        }

        public bool IsAllModuleTopicsTaken(Guid moduleId)
        {
            var isTaken = true;
            var topics = _topicRepository.GetTopicsByModule(moduleId);
            foreach(var topic in topics)
            {
                if (!topic.IsTaken) { isTaken = false; }
            }
            return isTaken;
        }

        public async Task<TopicsResponseModel> SearchTopicsByTitle(string searchText)
        {
            var topics = await _topicRepository.SearchTopicsByTitle(searchText);

            if (topics == null)
            {
                throw new BadRequestException($"Topics not found");
            }

            var topicsReturned = topics.Select(n => new TopicDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                ModuleName = n.Module.Name,
                ModuleId = n.ModuleId,
                IsTaken = n.IsTaken
            }).ToList();

            return new TopicsResponseModel
            {
                Data = topicsReturned,
                Message = $"{topics.Count()} Topics retrieved successfully",
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
