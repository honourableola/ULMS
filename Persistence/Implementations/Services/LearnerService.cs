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
using static Domain.Models.LearnerViewModel;

namespace Persistence.Implementations.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;

        public LearnerService(ILearnerRepository learnerRepository)
        {
            _learnerRepository = learnerRepository;
        }
        public async Task<BaseResponse> AddLearner(CreateLearnerRequestModel model)
        {
            var learnerExist = await _learnerRepository.ExistsAsync(a => a.Email == model.Email);

            if (learnerExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }

            var learner = new Learner
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                LearnerPhoto = model.LearnerPhoto,
                PhoneNumber = model.PhoneNumber,
                Created = DateTime.UtcNow
            };

            await _learnerRepository.AddAsync(learner);
            await _learnerRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.FirstName} {model.LastName} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteLearner(Guid id)
        {
            var learnerExist = await _learnerRepository.ExistsAsync(id);
            if (!learnerExist)
            {
                throw new BadRequestException($"Learner with id {id} does not exist");
            }

            var learner = await _learnerRepository.GetAsync(id);
            await _learnerRepository.DeleteAsync(learner);
            await _learnerRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{learner.FirstName} {learner.LastName} deleted successfully"
            };
        }

        public async Task<LearnersResponseModel> GetAllLearners()
        {
            var learners = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
               .Select(n => new LearnerDTO
               {
                   Id = n.Id,
                   FirstName = n.FirstName,
                   LastName = n.LastName,
                   Email = n.Email,
                   LearnerPhoto = n.LearnerPhoto,
                   PhoneNumber = n.PhoneNumber,
                   LearnerCourses = n.LearnerCourses.Select(o => new CourseDTO
                   {
                       Id = o.Id,
                       Name = o.Course.Name,
                       CategoryId = o.Course.CategoryId,
                       CategoryName = o.Course.Category.Name,
                       Description = o.Course.Description,
                       AvailabilityStatus = o.Course.AvailabilityStatus
                   }).ToList()

               }).ToListAsync();

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }
            else if (learners.Count == 0)
            {
                return new LearnersResponseModel
                {                   
                    Message = $" No Learner Found",
                    Status = true
                };
            }


            return new LearnersResponseModel
            {
                Data = learners,
                Message = $" {learners.Count} Learners retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnerResponseModel> GetLearnerByEmail(string email)
        {
            var learner = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
                .SingleOrDefaultAsync(a => a.Email == email);
            if(learner == null)
            {
                throw new BadRequestException($"Learner with eamil {email} does not exist");
            }

            return new LearnerResponseModel
            {
                Data = new LearnerDTO
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Email = learner.Email,
                    LearnerPhoto = learner.LearnerPhoto,
                    PhoneNumber = learner.PhoneNumber,
                    LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                    }).ToList()
                },
                Message = $"Learner retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnerResponseModel> GetLearnerById(Guid id)
        {
            var learner = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
                .ThenInclude(d => d.Category)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (learner == null)
            {
                throw new BadRequestException($"Learner with id {id} does not exist");
            }
            return new LearnerResponseModel
            {
                Data = new LearnerDTO
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Email = learner.Email,
                    LearnerPhoto = learner.LearnerPhoto,
                    PhoneNumber = learner.PhoneNumber,
                    LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                        
                    }).ToList()
                },
                Message = $"Learner retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnersResponseModel> GetLearnersByCourse(Guid courseId)
        {
            var learners = await _learnerRepository.GetLearnersByCourse(courseId);

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }
            else if (learners.Count == 0)
            {
                return new LearnersResponseModel
                {
                  
                    Message = $" No Learner Found",
                    Status = true
                };
            }


            var learnersReturned = learners.Select(learner => new LearnerDTO
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Email = learner.Email,
                LearnerPhoto = learner.LearnerPhoto,
                PhoneNumber = learner.PhoneNumber,
                LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });
            return new LearnersResponseModel
            {
                Data = learnersReturned,
                Message = $"{learners.Count} Learners offering course with id {courseId} retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnersResponseModel> SearchLearnersByName(string searchText)
        {
            var learners = await _learnerRepository.SearchLearnersByName(searchText);

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }

            var learnersReturned = learners.Select(learner => new LearnerDTO
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Email = learner.Email,
                LearnerPhoto = learner.LearnerPhoto,
                PhoneNumber = learner.PhoneNumber,
                LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });
            return new LearnersResponseModel
            {
                Data = learnersReturned,
                Message = $"{learners.Count()} Learners retrieved successfully",
                Status = true
            };
       
        }

        public async Task<BaseResponse> UpdateLearner(Guid id, UpdateLearnerRequestModel model)
        {
            var learner = await _learnerRepository.GetAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Learner with id {id} not found");
            }

            learner.PhoneNumber = model.PhoneNumber;
            learner.LearnerPhoto = model.LearnerPhoto;
            learner.Modified = DateTime.UtcNow;
            await _learnerRepository.UpdateAsync(learner);
            await _learnerRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{learner.FirstName} {learner.LastName} updated successfully",
                Status = true
            };
        }
    }
}
