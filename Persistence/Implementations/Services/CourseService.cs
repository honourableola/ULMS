using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Integrations.Email;
using Persistence.Integrations.MailKitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CourseViewModel;

namespace Persistence.Implementations.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseConstantService _courseConstantService;
        private readonly IMailService _mailService;
        //private readonly IMailSender _mailSender;
        public CourseService(ICourseRepository courseRepository, ILearnerRepository learnerRepository, IInstructorRepository instructorRepository, ICourseConstantService courseConstantService,  IMailService mailService)
        {
            _courseRepository = courseRepository;
            _learnerRepository = learnerRepository;
            _instructorRepository = instructorRepository;
            _courseConstantService = courseConstantService;
            _mailService = mailService;
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

            if (courses == null)
            {
                throw new BadRequestException($"Courses not found");
            }
            else if (courses.Count == 0)
            {
                return new CoursesResponseModel
                {
                    Data = courses,
                    Message = $" No Course Found",
                    Status = true
                };
            }

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $" {courses.Count} Courses retrieved successfully",
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

            if (courses == null)
            {
                throw new BadRequestException($"Courses Not found");
            }
            else if (courses.Count == 0)
            {
                return new CoursesResponseModel
                {
                    Data = courses,
                    Message = $" No Course Found",
                    Status = true
                };
            }


            return new CoursesResponseModel
            {
                Data = courses,
                Message = $" {courses.Count} Courses retrieved successfully",
                Status = true
            };

        }

        public async Task<CoursesResponseModel> GetActiveCourses()
        {
            var courses = await _courseRepository.Query()
                .Include(e => e.Category)
                .Include(a => a.InstructorCourses)
                .ThenInclude(c => c.Instructor)
                .Include(d => d.LearnerCourses)
                .ThenInclude(o => o.Learner)
                .Where(a => a.AvailabilityStatus == CourseAvailabilityStatus.IsActive)
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

            if (courses == null)
            {
                throw new BadRequestException($"Courses not found");
            }
            else if (courses.Count == 0)
            {
                return new CoursesResponseModel
                {
                    Data = courses,
                    Message = $" No Course Found",
                    Status = true
                };
            }

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $" {courses.Count} Courses retrieved successfully",
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

            if(course == null)
            {
                throw new BadRequestException($"Course with id {id} does not exist");
            }
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
                Message = $"Course retrieved successfully",
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
            if(courses == null)
            {
                throw new BadRequestException($"Courses with categoryId {categoryId} not found");
            }
            else if(courses.Count == 0)
            {
                return new CoursesResponseModel
                {
                    Data = courses,
                    Message = $" No Course Found",
                    Status = true
                };
            }

            return new CoursesResponseModel
            {
                Data = courses,
                Message = $"{courses.Count} Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<CoursesResponseModel> GetCoursesByInstructor(Guid instructorId)
        {
            var courses = await _courseRepository.GetCoursesByInstructor(instructorId);

            if (courses == null)
            {
                throw new BadRequestException($"Courses not found");
            }
            else if (courses.Count == 0)
            {
                return new CoursesResponseModel
                {                    
                    Message = $"No Course Found",
                    Status = true
                };
            }

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
                Message = $" {courses.Count} Instructor Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnerCoursesResponseModel> GetCoursesByLearner(Guid learnerId)
        {
            var courses = await _courseRepository.GetCoursesByLearner(learnerId);

            if (courses == null)
            {
                throw new BadRequestException($"Courses not found");
            }
            else if (courses.Count == 0)
            {
                return new LearnerCoursesResponseModel
                {                  
                    Message = $"No Course Found",
                    Status = true
                };
            }

            var coursesReturned = courses.Select(n => new LearnerCourseDTO
            {
                Id = n.Id,
                CourseName = n.Course.Name,
                CategoryName = n.Course.Category.Name,
                Description = n.Course.Description,
                AvailabilityStatus = n.Course.AvailabilityStatus,
                CourseType = n.CourseType,
                CourseId = n.CourseId          
            }).ToList();
            return new LearnerCoursesResponseModel
            {
                Data = coursesReturned,
                Message = $"{courses.Count} Learner Courses retrieved successfully",
                Status = true
            };
        }

        public async Task<CoursesResponseModel> SearchCoursesByName(string searchText)
        {
            var courses = await _courseRepository.SearchCoursesByName(searchText);

            if (courses == null)
            {
                throw new BadRequestException($"Courses not found");
            }
   
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
                Message = $"{coursesReturned.Count()} Searched Courses retrieved successfully",
                Status = true
            };         
       
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

        public async Task<BaseResponse> RequestForCourse(CourseRequestRequestModel model)
        {
            //Check if learner has a pending course request for the new course request
            //if existing request is approved, deny request
            //if existing request is initialized deny request
            //if existing request is rejected grant request

           /* var existingCourseRequests = await */
            var learner = await _learnerRepository.GetAsync(model.LearnerId);
            if (learner == null)
            {
                throw new NotFoundException($"Learner not found");
            }
            var course = await _courseRepository.GetAsync(model.courseId);

            if (course == null)
            {
                throw new NotFoundException($"Selected course not found");
            }

            var existingCourseRequests = await _courseRepository.GetUntreatedCourseRequestsByLearner(model.LearnerId);
            /*if (existingCourseRequests.Where(c => course.Id == c.CourseId) != null)
            {
                throw new BadRequestException($"Course request for {course.Name} not successful because there is an active requests for the same course");
            }*/
          
            if (existingCourseRequests.Any(c => c.CourseId == course.Id))
            {
                throw new BadRequestException($"Course request for {course.Name} not successful because there is an active requests for the same course");
            }

            var learnerAssignedCourses = await _courseRepository.GetCoursesByLearner(model.LearnerId);

            if (learnerAssignedCourses.Any(c => c.CourseId == course.Id))
            {
                throw new BadRequestException($"Course request not successful because {course.Name} already exist among learner's courses");
            }
            var courseRequest = new CourseRequest
            {
                Course = course,
                CourseId = course.Id,
                Learner = learner,
                LearnerId = learner.Id,
                RequestMessage = model.RequestMessage,
                RequestStatus = CourseRequestStatus.Requested
            };

            await _courseRepository.CreateCourseRequest(courseRequest);
            await _courseRepository.SaveChangesAsync();
            //var mailBody = $"😊😊Hello {learner.FirstName.ToUpper()} {learner.LastName.ToUpper()}. \nYour course request for {courseRequest.Course.Name} has been received by the Administrator. You will be notified of either approval or rejection of the request. Kindly refrain from making another request till you get a response. Warm Regards...✌✌✌✌";
            // await _mailSender.SendWelcomeMail(learner.User.Email, "Course Request Received".ToUpper(), mailBody, $"{learner.FirstName} {learner.LastName}");
            var request = new SuccessfulCourseRequestMail
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                CourseName = courseRequest.Course.Name,
                ToEmail = learner.Email
            };

            await _mailService.SendSuccessfulCourseRequestEmailAsync(request);
            return new BaseResponse
            {
                Message = $"Course Request for {course.Name} submitted by {learner.FirstName} {learner.LastName} to the Admin Successfully",
                Status = true
            };
           
        }

        public async Task<BaseResponse> AssignCoursesToLearner(LearnerCourseAssignmentRequestModel model)
        {
            //Check if course is already assigned to learner
            int countOfDuplicatedCourses = 0;
            var learner = await _learnerRepository.GetAsync(model.LearnerId);

            if (learner == null)
            {
                throw new NotFoundException($"Learner with id {model.LearnerId} does not exist");
            }

            var learnerAssignedCourses = await _courseRepository.GetCoursesByLearner(model.LearnerId);
            var selectedCourses = await _courseRepository.GetSelectedCourses(model.Ids);
            
            if (selectedCourses == null)
            {
                throw new NotFoundException($"Courses not found");
            }

            var noOfExistingMajorCourses = await _courseRepository.GetNoOfLearnerMajorCourses(model.LearnerId);
            var courseConstant = _courseConstantService.GetCourseConstant();

            if ((noOfExistingMajorCourses + selectedCourses.Count()) > courseConstant.MaximumNoOfMajorCourses)
            {
                throw new BadRequestException($"Only {courseConstant.MaximumNoOfMajorCourses - noOfExistingMajorCourses } additional Courses can be assigned to this learner due to the restriction maximum number of Major Courses possible i.e {courseConstant.MaximumNoOfMajorCourses} courses ");
            }

            foreach (var course in selectedCourses)
            {
                if (learnerAssignedCourses.Any(c => c.CourseId == course.Id))
                {
                    countOfDuplicatedCourses++;
                    continue;
                    //throw new BadRequestException($"{course.Name} already assigned to this learner");
                }
                var learnerCourse = new LearnerCourse
                {
                    Course = course,
                    CourseId = course.Id,
                    Learner = learner,
                    LearnerId = model.LearnerId,
                    CourseType = LearnerCourseType.Major
                };

                await _courseRepository.LearnerCourseAssignment(learnerCourse);
            }
            await _courseRepository.SaveChangesAsync();
            var courseNames = SelectedCoursesNames(selectedCourses);
            //var mailBody = $"😊😊Hello {learner.FirstName.ToUpper()} {learner.LastName.ToUpper()}. \nThe following courses has been assigned to you by the Administrator: {courseNames}👏👏. \nLog in to your account to start learning.    happy Learning...✌✌✌✌";
            //await _mailSender.SendWelcomeMail(learner.User.Email, "Notification of Courses Assigned".ToUpper(), mailBody, $"{learner.FirstName} {learner.LastName}");
            var request = new SuccessfulCourseAssignmentToLearner
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                CourseNames = courseNames,
                ToEmail = learner.Email
            };

            await _mailService.SendSuccessfulCourseAssignmentToLearnerEmailAsync(request);
            return new BaseResponse
            {
                Message = $"{selectedCourses.Count() - countOfDuplicatedCourses} Courses successfully assigned to {learner.FirstName} {learner.LastName} while {countOfDuplicatedCourses} courses were rejected because they were already assigned to the Learner",
                Status = true
            };
                     
        }

        private string SelectedCoursesNames(IEnumerable<Course> courses)
        {
            var names = "";
            foreach (var course in courses)
            {
                names += (course.Name + " ");
            }
            return names;
        }
        public async Task<BaseResponse> AssignCoursesToInstructor(InstructorCourseAssignmentRequestModel model)
        {
            int countOfDuplicatedCourses = 0;
            //Check if course is already assigned to Instructor (Improve the check)
            var instructor = await _instructorRepository.GetAsync(model.InstructorId);

            if (instructor == null)
            {
                throw new NotFoundException($"Instructor with id {model.InstructorId} does not exist");
            }

            var courses = await _courseRepository.GetSelectedCourses(model.Ids);

            if (courses == null)
            {
                throw new NotFoundException($"Courses not found");
            }

            var instructorAssignedCourses = await _courseRepository.GetCoursesByInstructor(model.InstructorId);
            foreach (var course in courses)
            {

                if (instructorAssignedCourses.Any(c => c.Id == course.Id))
                {
                    countOfDuplicatedCourses++;
                    continue;
                }
                var instructorCourse = new InstructorCourse
                {
                    Course = course,
                    CourseId = course.Id,
                    Instructor = instructor,
                    InstructorId = model.InstructorId
                };

                await _courseRepository.InstructorCourseAssignment(instructorCourse);
            }
            await _courseRepository.SaveChangesAsync();
            var courseNames = SelectedCoursesNames(courses);
            //var mailBody = $"😊😊Hello {instructor.FirstName.ToUpper()} {instructor.LastName.ToUpper()}. \nThe following courses has been assigned to you by the Administrator: {courseNames}👏👏. \nLog in to your account to start learning.    happy Learning...✌✌✌✌";
            //await _mailSender.SendWelcomeMail(instructor.User.Email, "Notification of Courses Assigned".ToUpper(), mailBody, $"{instructor.FirstName} {instructor.LastName}");
            var request = new SuccessfulCourseAssignmentToInstructor
            {
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                CourseNames = courseNames,
                ToEmail = instructor.Email
            };

            await _mailService.SendSuccessfulCourseAssignmentToInstructorEmailAsync(request);
            return new BaseResponse
            {
                Message = $"{courses.Count() - countOfDuplicatedCourses} Courses successfully assigned to {instructor.FirstName} {instructor.LastName} while {countOfDuplicatedCourses} courses were rejected because the courses were already assigned to the instructor",
                Status = true
            };
        }

        public async Task<BaseResponse> ApproveCourseRequest(Guid id)
        {
            var courseRequest = await _courseRepository.GetCourseRequestById(id);
            var learner = await _learnerRepository.GetAsync(courseRequest.LearnerId);

            if (courseRequest == null)
            {
                throw new NotFoundException($"Course request with Id {id} does not exist");
            }

            var learnerAssignedCourses = await _courseRepository.GetCoursesByLearner(courseRequest.LearnerId);

            var noOfExistingAdditionalCourses = await _courseRepository.GetNoOfLearnerAdditionalCourses(courseRequest.LearnerId);
            var courseConstant =  _courseConstantService.GetCourseConstant();

            if ((noOfExistingAdditionalCourses + 1) > courseConstant.MaximumNoOfAdditionalCourses)
            {
                throw new BadRequestException($"Course assignment Not successful because learner already reached the maximum number of Additional Courses possible i.e {courseConstant.MaximumNoOfAdditionalCourses} courses ");
            }


            if (learnerAssignedCourses.Any(c => c.CourseId == courseRequest.CourseId))
            {             
                throw new BadRequestException($"{courseRequest.Course.Name} approval not successful because it is already assigned to this learner");
            }
            var learnerCourse = new LearnerCourse
            {
                Course = courseRequest.Course,
                CourseId = courseRequest.CourseId,
                Learner = courseRequest.Learner,
                LearnerId = courseRequest.LearnerId,
                CourseType = LearnerCourseType.Requested

            };
            courseRequest.RequestStatus = CourseRequestStatus.Approved;
            await _courseRepository.LearnerCourseAssignment(learnerCourse);
            await _courseRepository.SaveChangesAsync();
            //var mailBody = $"😊😊Hello {learner.FirstName.ToUpper()} {learner.LastName.ToUpper()}. \nYour request for {courseRequest.Course.Name} has been approved and thus added to your list of courses👏👏. Log in to your account to start learning.    happy Learning...✌✌✌✌";
            //await _mailSender.SendWelcomeMail(learner.User.Email, "Notification of Approval of Course Request".ToUpper(), mailBody, $"{learner.FirstName} {learner.LastName}");

            var request = new CourseRequestApproval
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                CourseName = courseRequest.Course.Name,
                ToEmail = learner.Email
            };

            await _mailService.SendSuccessfulCourseRequestApprovalEmailAsync(request);
            return new BaseResponse
            {
                Message = $"{courseRequest.Course.Name} approved for {courseRequest.Learner.FirstName} {courseRequest.Learner.LastName} successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> RejectCourseRequest(Guid id)
        {
            var courseRequest = await _courseRepository.GetCourseRequestById(id);
            var learner = await _learnerRepository.GetAsync(courseRequest.LearnerId);

            if(courseRequest == null)
            {
                throw new NotFoundException($"Course Request with id {id} does not exist");
            }

            courseRequest.RequestStatus = CourseRequestStatus.Rejected;
            await _courseRepository.UpdateAsync(courseRequest);
            await _courseRepository.SaveChangesAsync();

            //var mailBody = $"😊😊Hello {learner.FirstName.ToUpper()} {learner.LastName.ToUpper()}. \nYour request for {courseRequest.Course.Name} has been rejected by the Administrator. Another request can be made if you are still interested in taking the course.   happy Learning...✌✌✌✌";
            //await _mailSender.SendWelcomeMail(learner.User.Email, "Notification of Rejection of Course Request".ToUpper(), mailBody, $"{learner.FirstName} {learner.LastName}");
            var request = new CourseRequestRejection
            {
                FirstName = learner.FirstName.ToUpper(),
                LastName = learner.LastName.ToUpper(),
                CourseName = courseRequest.Course.Name,
                ToEmail = learner.Email
            };

            await _mailService.SendCourseRequestRejectionEmailAsync(request);
            return new BaseResponse
            {
                Message = $"{courseRequest.Course.Name} request by {courseRequest.Learner.FirstName} {courseRequest.Learner.LastName} rejected successfully",
                Status = true
            };
        }

        public async Task<CourseRequestsResponseModel> GetAllCourseRequestsUntreated()
        {
            var courseRequests = await _courseRepository.GetAllCourseRequestsUntreated();

            if (courseRequests == null)
            {
                throw new NotFoundException($"No Untreated Course requests found");
            }
            else if (courseRequests.Count() == 0)
            {
                return new CourseRequestsResponseModel
                {
                    Message = $"No Untreated Course Requests available",
                    Status = true
                };
            }

            var courseRequestsReturned = courseRequests.Select(c => new CourseRequestDTO
            {
                Id = c.Id,
                CourseId = c.CourseId,
                Course = c.Course,
                LearnerId = c.LearnerId,
                Learner = c.Learner,
                RequestMessage = c.RequestMessage
            }).ToList();

            return new CourseRequestsResponseModel
            {
                Data = courseRequestsReturned,
                Message = $"{courseRequests.Count()} Untreated Course requests retrieved successfully",
                Status = true
            };
        }

        public async Task<CourseRequestsResponseModel> GetAllCourseRequestsRejected()
        {
            var courseRequests = await _courseRepository.GetAllCourseRequestsRejected();

            if (courseRequests == null)
            {
                throw new NotFoundException($"No Rejected course requets found");
            }
            else if (courseRequests.Count() == 0)
            {
                return new CourseRequestsResponseModel
                {
                    Message = $"No rejected course Requests available",
                    Status = true
                };
            }

            var courseRequestsReturned = courseRequests.Select(c => new CourseRequestDTO
            {
                Id = c.Id,
                CourseId = c.CourseId,
                Course = c.Course,
                LearnerId = c.LearnerId,
                Learner = c.Learner,
                RequestMessage = c.RequestMessage
            }).ToList();

            return new CourseRequestsResponseModel
            {
                Data = courseRequestsReturned,
                Message = $"{courseRequests.Count()} Rejected Course requests retrieved successfully",
                Status = true
            };
        }

        public async Task<CourseRequestsResponseModel> GetAllCourseRequestsApproved()
        {
            var courseRequests = await _courseRepository.GetAllCourseRequestsApproved();

            if (courseRequests == null)
            {
                throw new NotFoundException($"No Approved course requests found");
            }
            else if (courseRequests.Count() == 0)
            {
                return new CourseRequestsResponseModel
                {
                    Message = $"No Approved course Requests found",
                    Status = true
                };
            }

            var courseRequestsReturned = courseRequests.Select(c => new CourseRequestDTO
            {
                Id = c.Id,
                CourseId = c.CourseId,
                Course = c.Course,
                LearnerId = c.LearnerId,
                Learner = c.Learner,
                RequestMessage = c.RequestMessage
            }).ToList();

            return new CourseRequestsResponseModel
            {
                Data = courseRequestsReturned,
                Message = $"{courseRequests.Count()} Approved Course requests retrieved successfully",
                Status = true
            };
        }

        public async Task<CourseRequestsResponseModel> GetUntreatedCourseRequestsByLearner(Guid learnerId)
        {
            var courseRequests = await _courseRepository.GetUntreatedCourseRequestsByLearner(learnerId);

            if (courseRequests == null)
            {
                throw new NotFoundException($"No Pending course requests found for learner with id {learnerId}");
            }
            else if (courseRequests.Count() == 0)
            {
                return new CourseRequestsResponseModel
                {
                    Message = $"No Pending course Requests for learner with id {learnerId}",
                    Status = true
                };
            }

            var courseRequestsReturned = courseRequests.Select(c => new CourseRequestDTO
            {
                Id = c.Id,
                CourseId = c.CourseId,
                Course = c.Course,
                LearnerId = c.LearnerId,
                Learner = c.Learner,
                RequestMessage = c.RequestMessage
            }).ToList();

            return new CourseRequestsResponseModel
            {
                Data = courseRequestsReturned,
                Message = $"{courseRequests.Count()} Untreated Course requests retrieved successfully",
                Status = true
            };
        }
    }
}
