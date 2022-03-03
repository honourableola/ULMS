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
using static Domain.Models.ModuleViewModel;

namespace Persistence.Implementations.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        public async Task<BaseResponse> AddModule(CreateModuleRequestModel model)
        {
            var moduleExist = await _moduleRepository.ExistsAsync(a => a.Name == model.Name);

            if (moduleExist)
            {
                throw new BadRequestException($"{model.Name} already exist and cannot be added");
            }

            var module = new Module
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Content = model.Content,
                CourseId = model.CourseId,
                Description = model.Description,
                ModuleImage1 = model.ModuleImage1,
                ModuleImage2 = model.ModuleImage2,
                ModulePDF1 = model.ModulePDF1,
                ModulePDF2 = model.ModulePDF2,
                ModuleVideo1 = model.ModuleVideo1,
                ModuleVideo2 = model.ModuleVideo2,
                Created = DateTime.UtcNow

            };

            await _moduleRepository.AddAsync(module);
            await _moduleRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.Name} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteModule(Guid id)
        {
            var moduleExist = await _moduleRepository.ExistsAsync(id);
            if (!moduleExist)
            {
                throw new BadRequestException($"Module with id {id} does not exist");
            }

            var module = await _moduleRepository.GetAsync(id);
            await _moduleRepository.DeleteAsync(module);
            await _moduleRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{module.Name} deleted successfully"
            };
        }

        public async Task<ModulesResponseModel> GetAllModules()
        {
            var modules = await _moduleRepository.Query()
               .Select(n => new ModuleDTO
               {
                   Id = n.Id,
                   Name = n.Name,
                   Content = n.Content,
                   CourseId = n.CourseId,
                   CourseName = n.Course.Name,
                   Description = n.Description,
                   ModuleImage1 = n.ModuleImage1,
                   ModuleImage2 = n.ModuleImage2,
                   ModulePDF1 = n.ModulePDF1,
                   ModulePDF2 = n.ModulePDF2,
                   ModuleVideo1 = n.ModuleVideo1,
                   ModuleVideo2 = n.ModuleVideo2,                  
                   Topics = n.Topics.Select(o => new TopicDTO
                   {
                       Id = o.Id,
                       Content = o.Content,
                       ModuleId = o.ModuleId,
                       ModuleName = o.Module.Name,
                       Title = o.Title                      
                   }).ToList()
               }).ToListAsync();

            if (modules == null)
            {
                throw new BadRequestException($"Modules not found");
            }
            else if (modules.Count == 0)
            {
                return new ModulesResponseModel
                {                
                    Message = $" No Module Found",
                    Status = true
                };
            }

            return new ModulesResponseModel
            {
                Data = modules,
                Message = $"{modules.Count} Modules retrieved successfully",
                Status = true
            };
        }

        public async Task<ModuleResponseModel> GetModule(Guid id)
        {
            var module = await _moduleRepository.Query()
                .Include(d => d.Course)
                .SingleOrDefaultAsync(a => a.Id == id);

            if(module == null)
            {
                throw new BadRequestException($"Module with id {id} does not exist");
            }

            return new ModuleResponseModel
            {
                Data = new ModuleDTO
                {
                    Id = module.Id,
                    Name = module.Name,
                    Content = module.Content,
                    CourseId = module.CourseId,
                    CourseName = module.Course.Name,
                    Description = module.Description,
                    ModuleImage1 = module.ModuleImage1,
                    ModuleImage2 = module.ModuleImage2,
                    ModulePDF1 = module.ModulePDF1,
                    ModulePDF2 = module.ModulePDF2,
                    ModuleVideo1 = module.ModuleVideo1,
                    ModuleVideo2 = module.ModuleVideo2,
                    Topics = module.Topics.Select(o => new TopicDTO
                    {
                        Id = o.Id,
                        Content = o.Content,
                        ModuleId = o.ModuleId,
                        ModuleName = o.Module.Name,
                        Title = o.Title
                    }).ToList()
                
                },
                Message = $"Module retrieved successfully",
                Status = true
            };
        }

        public async Task<ModulesResponseModel> GetModulesByCourse(Guid courseId)
        {
            var modules = await _moduleRepository.Query()
                .Include(e => e.Course)
                .Where(c => c.CourseId == courseId)
                .Select(n => new ModuleDTO
                {
                    Id = n.Id,
                    Name = n.Name,
                    Content = n.Content,
                    CourseId = n.CourseId,
                    CourseName = n.Course.Name,
                    Description = n.Description,
                    ModuleImage1 = n.ModuleImage1,
                    ModuleImage2 = n.ModuleImage2,
                    ModulePDF1 = n.ModulePDF1,
                    ModulePDF2 = n.ModulePDF2,
                    ModuleVideo1 = n.ModuleVideo1,
                    ModuleVideo2 = n.ModuleVideo2,
                    Topics = n.Topics.Select(o => new TopicDTO
                    {
                        Id = o.Id,
                        Content = o.Content,
                        ModuleId = o.ModuleId,
                        ModuleName = o.Module.Name,
                        Title = o.Title
                    }).ToList()

                }).ToListAsync();

            if (modules == null)
            {
                throw new BadRequestException($"Modules not found");
            }
            else if (modules.Count == 0)
            {
                return new ModulesResponseModel
                {                  
                    Message = $" No Module Found",
                    Status = true
                };
            }


            return new ModulesResponseModel
            {
                Data = modules,
                Message = $"{modules.Count} Modules retrieved successfully",
                Status = true
            };
        }

        public async Task<ModulesResponseModel> SearchModulesByName(string searchText)
        {
            var modules = await _moduleRepository.SearchModuleByName(searchText);
            if (modules == null)
            {
                throw new BadRequestException($"Modules not found");
            }
           

            var modulesReturned = modules.Select(n => new ModuleDTO
            {
                Id = n.Id,
                Name = n.Name,
                Content = n.Content,
                CourseId = n.CourseId,
                CourseName = n.Course.Name,
                Description = n.Description,
                ModuleImage1 = n.ModuleImage1,
                ModuleImage2 = n.ModuleImage2,
                ModulePDF1 = n.ModulePDF1,
                ModulePDF2 = n.ModulePDF2,
                ModuleVideo1 = n.ModuleVideo1,
                ModuleVideo2 = n.ModuleVideo2,
                Topics = n.Topics.Select(o => new TopicDTO
                {
                    Id = o.Id,
                    Content = o.Content,
                    ModuleId = o.ModuleId,
                    ModuleName = o.Module.Name,
                    Title = o.Title
                }).ToList()

            }).ToList();


            return new ModulesResponseModel
            {
                Data = modulesReturned,
                Message = $"{modules.Count()} Modules retrieved successfully",
                Status = true
            };
       
        }

        public async Task<BaseResponse> UpdateModule(Guid id, UpdateModuleRequestModel model)
        {
            var module = await _moduleRepository.GetAsync(id);
            if (module == null)
            {
                throw new NotFoundException($"Module with id {id} not found");
            }

            module.Name = model.Name;
            module.Content = model.Content;
            module.Description = model.Description;
            module.ModuleImage1 = model.ModuleImage1;
            module.ModuleImage2 = model.ModuleImage2;
            module.ModulePDF1 = model.ModulePDF1;
            module.ModulePDF2 = model.ModulePDF2;
            module.ModuleVideo1 = model.ModuleVideo1;
            module.ModuleVideo2 = model.ModuleVideo2;
            module.Modified = DateTime.UtcNow;

            await _moduleRepository.UpdateAsync(module);
            await _moduleRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{module.Name} updated successfully",
                Status = true
            };
        }

    }
}
