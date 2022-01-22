using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
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
using static Domain.Models.CourseViewModel;

namespace Persistence.Implementations.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IInstructorRepository _instructorRepository;
        public CourseService(ICourseRepository courseRepository, ILearnerRepository learnerRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _learnerRepository = learnerRepository;
            _instructorRepository = instructorRepository;
        }
        public async Task<BaseResponse> AddCourse(CreateCourseRequestModel model)
        {
            var courseExist = await _courseRepository.ExistsAsync(a => a.Name == model.Name);

            if (courseExist)
            {
                throw new BadRequestException($"{model.Name} already exist and cannot be added");
            }

            var course = new Course
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Created = DateTime.UtcNow,
                AvailabilityStatus = CourseAvailabilityStatus.IsActive

            };

            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.Name} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteCourse(Guid id)
        {
            var courseExist = await _courseRepository.ExistsAsync(id);
            if (!courseExist)
            {
                throw new BadRequestException($"Course with id {id} does not exist");
            }

            var course = await _courseRepository.GetAsync(id);
            await _courseRepository.DeleteAsync(course);
            await _courseRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{course.Name} deleted successfully"
            };
        }

        public async Task<CoursesResponseModel> GetAllCourses()
        {
            var courses = await _courseRepository.Query()
                .Include(e => e.Category)
                .Include(a => a.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(d => d.LearnerCourses)
                .ThenInclude(o => o.Learner)
                .Select(n => new CourseDTO
                {
                    Id = n.Id,
                    Name = n.Name,
                    CategoryId = n.CategoryId,
                    CategoryName = n.Category.Name,
                    Description = n.Description,
                    AvailabilityStatus = n.AvailabilityStatus,
                    Modules = n.Modules.Select(m => new ModuleDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Content = m.Content,
                        ModuleImage1 = m.ModuleImage1,
                        ModuleImage2 = m.ModuleImage2,
                        ModulePDF1 = m.ModulePDF1,
                        ModulePDF2 = m.ModulePDF2,
                        ModuleVideo1 = m.ModuleVideo1,
                        ModuleVideo2 = m.ModuleVideo2,
                        CourseId = m.CourseId,
                        CourseName = m.Course.Name

                    }).ToList(),
                    InstructorCourses = n.InstructorCourses.Select(ic => new InstructorDTO
                    {
                        Id = ic.InstructorId,
                        FirstName = ic.Instructor.FirstName,
                        LastName = ic.Instructor.LastName,
                        Email = ic.Instructor.Email,
                        PhoneNumber = ic.Instructor.PhoneNumber,
                        InstructorPhoto = ic.Instructor.InstructorPhoto

                    }).ToList(),
                    LearnerCourses = n.LearnerCourses.Select(l => new LearnerDTO
                    {
                        Id = l.Learner.Id,
                        FirstName = l.Learner.FirstName,
                        LastName = l.Learner.LastName,
                        Email = l.Learner.Email,
                        LearnerPhoto = l.Learner.LearnerPhoto,
                        PhoneNumber = l.Learner.PhoneNumber,

                    }).ToList()

                }).ToListAsync();

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $"Courses retrieved successfully",
                Status = true
            };

        }

        public async Task<CoursesResponseModel> GetArchivedCourses()
        {
            var courses = await _courseRepository.Query()
                .Include(e => e.Category)
                .Include(a => a.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(d => d.LearnerCourses)
                .ThenInclude(o => o.Learner)
                .Where(a => a.AvailabilityStatus == CourseAvailabilityStatus.IsArchived)
                .Select(n => new CourseDTO
                {
                Id = n.Id,
                Name = n.Name,
                CategoryId = n.CategoryId,
                CategoryName = n.Category.Name,
                Description = n.Description,
                AvailabilityStatus = n.AvailabilityStatus,
                Modules = n.Modules.Select(m => new ModuleDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Content = m.Content,
                    ModuleImage1 = m.ModuleImage1,
                    ModuleImage2 = m.ModuleImage2,
                    ModulePDF1 = m.ModulePDF1,
                    ModulePDF2 = m.ModulePDF2,
                    ModuleVideo1 = m.ModuleVideo1,
                    ModuleVideo2 = m.ModuleVideo2,
                    CourseId = m.CourseId,
                    CourseName = m.Course.Name

                }).ToList(),
                InstructorCourses = n.InstructorCourses.Select(ic => new InstructorDTO
                {
                    Id = ic.InstructorId,
                    FirstName = ic.Instructor.FirstName,
                    LastName = ic.Instructor.LastName,
                    Email = ic.Instructor.Email,
                    PhoneNumber = ic.Instructor.PhoneNumber,
                    InstructorPhoto = ic.Instructor.InstructorPhoto

                }).ToList(),
                LearnerCourses = n.LearnerCourses.Select(l => new LearnerDTO
                {
                    Id = l.Learner.Id,
                    FirstName = l.Learner.FirstName,
                    LastName = l.Learner.LastName,
                    Email = l.Learner.Email,
                    LearnerPhoto = l.Learner.LearnerPhoto,
                    PhoneNumber = l.Learner.PhoneNumber,

                }).ToList()

            }).ToListAsync();

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $"Courses retrieved successfully",
                Status = true
            };

        }

        public async Task<CourseResponseModel> GetCourseById(Guid id)
        {
            var course = await _courseRepository.Query()
                .Include(e => e.Category)
                .Include(a => a.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(d => d.LearnerCourses)
                .ThenInclude(o => o.Learner)
                .SingleOrDefaultAsync(a => a.Id == id);

            return new CourseResponseModel
            {
                Data = new CourseDTO
                {
                    Id = course.Id,
                    Name = course.Name,
                    CategoryId = course.CategoryId,
                    CategoryName = course.Category.Name,
                    Description = course.Description,
                    AvailabilityStatus = course.AvailabilityStatus,
                    Modules = course.Modules.Select(m => new ModuleDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Content = m.Content,
                        ModuleImage1 = m.ModuleImage1,
                        ModuleImage2 = m.ModuleImage2,
                        ModulePDF1 = m.ModulePDF1,
                        ModulePDF2 = m.ModulePDF2,
                        ModuleVideo1 = m.ModuleVideo1,
                        ModuleVideo2 = m.ModuleVideo2,
                        CourseId = m.CourseId,
                        CourseName = m.Course.Name

                    }).ToList(),
                    InstructorCourses = course.InstructorCourses.Select(ic => new InstructorDTO
                    {
                        Id = ic.InstructorId,
                        FirstName = ic.Instructor.FirstName,
                        LastName = ic.Instructor.LastName,
                        Email = ic.Instructor.Email,
                        PhoneNumber = ic.Instructor.PhoneNumber,
                        InstructorPhoto = ic.Instructor.InstructorPhoto

                    }).ToList(),
                    LearnerCourses = course.LearnerCourses.Select(l => new LearnerDTO
                    {
                        Id = l.Learner.Id,
                        FirstName = l.Learner.FirstName,
                        LastName = l.Learner.LastName,
                        Email = l.Learner.Email,
                        LearnerPhoto = l.Learner.LearnerPhoto,
                        PhoneNumber = l.Learner.PhoneNumber,

                    }).ToList()

                },
                Message = $"Courses retrieved successfully",
                Status = true
            };
 
        }

        public async Task<CoursesResponseModel> GetCoursesByCategory(Guid categoryId)
        {
            var courses = await _courseRepository.Query()
                .Include(e => e.Category)
                .Include(a => a.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(d => d.LearnerCourses)
                .ThenInclude(o => o.Learner)
                .Where(c => c.CategoryId == categoryId)
                .Select(n => new CourseDTO
                {
                    Id = n.Id,
                    Name = n.Name,
                    CategoryId = n.CategoryId,
                    CategoryName = n.Category.Name,
                    Description = n.Description,
                    AvailabilityStatus = n.AvailabilityStatus,
                    Modules = n.Modules.Select(m => new ModuleDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Content = m.Content,
                        ModuleImage1 = m.ModuleImage1,
                        ModuleImage2 = m.ModuleImage2,
                        ModulePDF1 = m.ModulePDF1,
                        ModulePDF2 = m.ModulePDF2,
                        ModuleVideo1 = m.ModuleVideo1,
                        ModuleVideo2 = m.ModuleVideo2,
                        CourseId = m.CourseId,
                        CourseName = m.Course.Name

                    }).ToList(),
                    InstructorCourses = n.InstructorCourses.Select(ic => new InstructorDTO
                    {
                        Id = ic.InstructorId,
                        FirstName = ic.Instructor.FirstName,
                        LastName = ic.Instructor.LastName,
                        Email = ic.Instructor.Email,
                        PhoneNumber = ic.Instructor.PhoneNumber,
                        InstructorPhoto = ic.Instructor.InstructorPhoto

                    }).ToList(),
                    LearnerCourses = n.LearnerCourses.Select(l => new LearnerDTO
                    {
                        Id = l.Learner.Id,
                        FirstName = l.Learner.FirstName,
                        LastName = l.Learner.LastName,
                        Email = l.Learner.Email,
                        LearnerPhoto = l.Learner.LearnerPhoto,
                        PhoneNumber = l.Learner.PhoneNumber,

                    }).ToList()

                }).ToListAsync();

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $"Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<CoursesResponseModel> GetCoursesByInstructor(Guid instructorId)
        {
            var courses = await _courseRepository.GetCoursesByInstructor(instructorId);

            var coursesReturned = courses.Select(n => new CourseDTO
            {
                Id = n.Id,
                Name = n.Name,
                CategoryId = n.CategoryId,
                CategoryName = n.Category.Name,
                Description = n.Description,
                AvailabilityStatus = n.AvailabilityStatus,
                Modules = n.Modules.Select(m => new ModuleDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Content = m.Content,
                    ModuleImage1 = m.ModuleImage1,
                    ModuleImage2 = m.ModuleImage2,
                    ModulePDF1 = m.ModulePDF1,
                    ModulePDF2 = m.ModulePDF2,
                    ModuleVideo1 = m.ModuleVideo1,
                    ModuleVideo2 = m.ModuleVideo2,
                    CourseId = m.CourseId,
                    CourseName = m.Course.Name

                }).ToList(),
                InstructorCourses = n.InstructorCourses.Select(ic => new InstructorDTO
                {
                    Id = ic.InstructorId,
                    FirstName = ic.Instructor.FirstName,
                    LastName = ic.Instructor.LastName,
                    Email = ic.Instructor.Email,
                    PhoneNumber = ic.Instructor.PhoneNumber,
                    InstructorPhoto = ic.Instructor.InstructorPhoto

                }).ToList(),
                LearnerCourses = n.LearnerCourses.Select(l => new LearnerDTO
                {
                    Id = l.Learner.Id,
                    FirstName = l.Learner.FirstName,
                    LastName = l.Learner.LastName,
                    Email = l.Learner.Email,
                    LearnerPhoto = l.Learner.LearnerPhoto,
                    PhoneNumber = l.Learner.PhoneNumber,

                }).ToList()
            });
            return new CoursesResponseModel
            {
                Data = coursesReturned,
                Message = $"Instructor Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<CoursesResponseModel> GetCoursesByLearner(Guid learnerId)
        {
            var courses = await _courseRepository.GetCoursesByLearner(learnerId);

            var coursesReturned = courses.Select(n => new CourseDTO
            {
                Id = n.Id,
                Name = n.Name,
                CategoryId = n.CategoryId,
                CategoryName = n.Category.Name,
                Description = n.Description,
                AvailabilityStatus = n.AvailabilityStatus,
                Modules = n.Modules.Select(m => new ModuleDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Content = m.Content,
                    ModuleImage1 = m.ModuleImage1,
                    ModuleImage2 = m.ModuleImage2,
                    ModulePDF1 = m.ModulePDF1,
                    ModulePDF2 = m.ModulePDF2,
                    ModuleVideo1 = m.ModuleVideo1,
                    ModuleVideo2 = m.ModuleVideo2,
                    CourseId = m.CourseId,
                    CourseName = m.Course.Name

                }).ToList(),
                InstructorCourses = n.InstructorCourses.Select(ic => new InstructorDTO
                {
                    Id = ic.InstructorId,
                    FirstName = ic.Instructor.FirstName,
                    LastName = ic.Instructor.LastName,
                    Email = ic.Instructor.Email,
                    PhoneNumber = ic.Instructor.PhoneNumber,
                    InstructorPhoto = ic.Instructor.InstructorPhoto

                }).ToList(),
                LearnerCourses = n.LearnerCourses.Select(l => new LearnerDTO
                {
                    Id = l.Learner.Id,
                    FirstName = l.Learner.FirstName,
                    LastName = l.Learner.LastName,
                    Email = l.Learner.Email,
                    LearnerPhoto = l.Learner.LearnerPhoto,
                    PhoneNumber = l.Learner.PhoneNumber,

                }).ToList()
            });
            return new CoursesResponseModel
            {
                Data = coursesReturned,
                Message = $"Instructor Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<IEnumerable<Course>> SearchCoursesByName(string searchText)
        {
            return await _courseRepository.SearchCoursesByName(searchText);
        }

        public async Task<BaseResponse> UpdateCourse(Guid id, UpdateCourseRequestModel model)
        {
            var course = await _courseRepository.GetAsync(id);
            if(course == null)
            {
                throw new NotFoundException($"Course with id {id} not found");
            }

            course.Name = model.Name;
            course.Description = model.Description;
            course.Modified = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(course);
            await _courseRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{course.Name} updated successfully",
                Status = true
            };
        }
    }
}
