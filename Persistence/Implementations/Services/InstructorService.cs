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
using static Domain.Models.InstructorViewModel;

namespace Persistence.Implementations.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }
        public async Task<BaseResponse> AddInstructor(CreateInstructorRequestModel model)
        {
            var instructorExist = await _instructorRepository.ExistsAsync(a => a.Email == model.Email);

            if (instructorExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }

            var instructor = new Instructor
            {
               Id = Guid.NewGuid(),
               FirstName = model.FirstName,
               LastName = model.LastName,
               Email = model.Email,
               InstructorPhoto = model.InstructorPhoto,
               PhoneNumber = model.PhoneNumber,
               Created = DateTime.UtcNow
            };

            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.FirstName} {model.LastName} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteInstructor(Guid id)
        {
            var instructorExist = await _instructorRepository.ExistsAsync(id);
            if (!instructorExist)
            {
                throw new BadRequestException($"Instructor with id {id} does not exist");
            }

            var instructor = await _instructorRepository.GetAsync(id);
            await _instructorRepository.DeleteAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{instructor.FirstName} {instructor.LastName} deleted successfully"
            };
        }

        public async Task<InstructorsResponseModel> GetAllInstructors()
        {
            var instructors = await _instructorRepository.Query()
                .Include(u => u.InstructorCourses)
                .ThenInclude(a => a.Course)
               .Select(n => new InstructorDTO
               {
                   Id = n.Id,
                   FirstName = n.FirstName,
                   LastName = n.LastName,
                   Email = n.Email,
                   InstructorPhoto = n.InstructorPhoto,
                   PhoneNumber = n.PhoneNumber,
                   InstructorCourses = n.InstructorCourses.Select(o => new CourseDTO
                   {
                       Id = o.Id,
                       Name = o.Course.Name,
                       CategoryId = o.Course.CategoryId,
                       CategoryName = o.Course.Category.Name,
                       Description = o.Course.Description,
                       IsArchived = o.Course.IsArchived
                   }).ToList()
              
               }).ToListAsync();

            return new InstructorsResponseModel
            {
                Data = instructors,
                Message = $"Instructors retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorResponseModel> GetInstructorByEmail(string email)
        {
            var instructor = await _instructorRepository.Query()
                .Include(u => u.InstructorCourses)
                .ThenInclude(a => a.Course)
                .SingleOrDefaultAsync(a => a.Email == email);

            return new InstructorResponseModel
            {
                Data = new InstructorDTO
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    Email = instructor.Email,
                    InstructorPhoto = instructor.InstructorPhoto,
                    PhoneNumber = instructor.PhoneNumber,
                    InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        IsArchived = o.Course.IsArchived
                    }).ToList()
                },
                Message = $"Instructor retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorResponseModel> GetInstructorById(Guid id)
        {
            var instructor = await _instructorRepository.Query()
                .Include(u => u.InstructorCourses)
                .ThenInclude(a => a.Course)
                .SingleOrDefaultAsync(a => a.Id == id);

            return new InstructorResponseModel
            {
                Data = new InstructorDTO
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    Email = instructor.Email,
                    InstructorPhoto = instructor.InstructorPhoto,
                    PhoneNumber = instructor.PhoneNumber,
                    InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        IsArchived = o.Course.IsArchived
                    }).ToList()
                },
                Message = $"Instructor retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorsResponseModel> GetInstructorsByCourse(Guid courseId)
        {
            var instructors = await _instructorRepository.GetInstructorsByCourse(courseId);

            var instructorsReturned = instructors.Select(instructor => new InstructorDTO
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Email = instructor.Email,
                InstructorPhoto = instructor.InstructorPhoto,
                PhoneNumber = instructor.PhoneNumber,
                InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    IsArchived = o.Course.IsArchived
                }).ToList()
            });
            return new InstructorsResponseModel
            {
                Data = instructorsReturned,
                Message = $"Instructors offering course with id {courseId} retrieved successfully",
                Status = true
            };
        }

        public async Task<IEnumerable<Instructor>> SearchInstructorsByName(string searchText)
        {
            return await _instructorRepository.SearchInstructorsByName(searchText);
        }

        public async Task<BaseResponse> UpdateInstructor(Guid id, UpdateInstructorRequestModel model)
        {
            var instructor = await _instructorRepository.GetAsync(id);
            if (instructor == null)
            {
                throw new NotFoundException($"Instructor with id {id} not found");
            }

            instructor.PhoneNumber = model.PhoneNumber;
            instructor.InstructorPhoto = model.InstructorPhoto;
            instructor.Modified = DateTime.UtcNow;
            await _instructorRepository.UpdateAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{instructor.FirstName} {instructor.LastName} updated successfully",
                Status = true
            };
        }
    }
}
