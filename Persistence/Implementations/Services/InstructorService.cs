using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Identity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Integrations.Email;
using Persistence.Integrations.MailKitModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.InstructorViewModel;

namespace Persistence.Implementations.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IUserRepository _userRepository;
        //private readonly IIdentityService _identityService;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailService _mailService;
        //private readonly IMailSender _mailSender;
        public InstructorService(IInstructorRepository instructorRepository, IUserRepository userRepository, /*IIdentityService identityService,*/ IRoleRepository roleRepository/*, IMailSender mailSender*/, IMailService mailService)
        {
            _instructorRepository = instructorRepository;
            _userRepository = userRepository;
            //_identityService = identityService;
            _roleRepository = roleRepository;
            _mailService = mailService;
        }
        public async Task<BaseResponse> AddInstructor(CreateInstructorRequestModel model)
        {
            var userExist = await _userRepository.ExistsAsync(a => a.Email == model.Email);
            if (userExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }
            var instructorExist = await _instructorRepository.ExistsAsync(a => a.Email == model.Email);

            if (instructorExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }

            var salt = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Instructor,
                HashSalt = salt
            };

            //var password = $"ULMS{Guid.NewGuid().ToString().Substring(1, 6)}";
            var password = "password";
            //user.PasswordHash = _identityService.GetPasswordHash(password, salt);
            var instructor = new Instructor
            {
               Id = Guid.NewGuid(),
               FirstName = model.FirstName,
               LastName = model.LastName,
               Email = model.Email,
               InstructorPhoto = model.InstructorPhoto,
               PhoneNumber = model.PhoneNumber,
               InstructorLMSCode = $"ULMS{Guid.NewGuid().ToString().Substring(1, 5)}I",
               UserId = user.Id,
               User = user              
            };

            var role = await _roleRepository.GetAsync(r => r.Name == "instructor");
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                Role = role,
                RoleId = role.Id,
                User = user,
                UserId = user.Id
            };

            user.UserRoles.Add(userRole);
            await _userRepository.AddAsync(user);
            await _instructorRepository.AddAsync(instructor);
            await _userRepository.SaveChangesAsync();
            await _instructorRepository.SaveChangesAsync();

            var request = new WelcomeMail
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = password,
                ToEmail = user.Email
            };

            await _mailService.SendWelcomeEmailAsync(request);
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
                   InstructorLMSCode = n.InstructorLMSCode,
                   InstructorCourses = n.InstructorCourses.Select(o => new CourseDTO
                   {
                       Id = o.Id,
                       Name = o.Course.Name,
                       CategoryId = o.Course.CategoryId,
                       CategoryName = o.Course.Category.Name,
                       Description = o.Course.Description,
                       AvailabilityStatus = o.Course.AvailabilityStatus
                   }).ToList()
              
               }).ToListAsync();

            if (instructors == null)
            {
                throw new BadRequestException($"Instructors not found");
            }
            else if (instructors.Count == 0)
            {
                return new InstructorsResponseModel
                {
                   
                    Message = $" No Instructor Found",
                    Status = true
                };
            }


            return new InstructorsResponseModel
            {
                Data = instructors,
                Message = $"{instructors.Count} Instructors retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorResponseModel> GetInstructorByEmail(string email)
        {
            var instructor = await _instructorRepository.Query()
                .Include(u => u.InstructorCourses)
                .ThenInclude(a => a.Course)
                .SingleOrDefaultAsync(a => a.Email == email);

            if(instructor == null)
            {
                throw new BadRequestException($"Instructor with email {email} does not exist");
            }

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
                    InstructorLMSCode = instructor.InstructorLMSCode,
                    InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
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
                .ThenInclude(a => a.Category)
                .SingleOrDefaultAsync(a => a.Id == id);

            if(instructor == null)
            {
                throw new BadRequestException($"Instructor with id {id} does not exist");
            }

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
                    InstructorLMSCode = instructor.InstructorLMSCode,
                    InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                    }).ToList()
                },
                Message = $"Instructor retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorsResponseModel> GetInstructorsByCourse(Guid courseId)
        {
            var instructors = await _instructorRepository.GetInstructorsByCourse(courseId);

            if (instructors == null)
            {
                throw new BadRequestException($"Instructors not found");
            }
            else if (instructors.Count == 0)
            {
                return new InstructorsResponseModel
                {
                   
                    Message = $" No Instructor Found",
                    Status = true
                };
            }


            var instructorsReturned = instructors.Select(instructor => new InstructorDTO
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Email = instructor.Email,
                InstructorPhoto = instructor.InstructorPhoto,
                PhoneNumber = instructor.PhoneNumber,
                InstructorLMSCode = instructor.InstructorLMSCode,
                InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });
            return new InstructorsResponseModel
            {
                Data = instructorsReturned,
                Message = $"{instructors.Count} Instructors offering course with id {courseId} retrieved successfully",
                Status = true
            };
        }

        public async Task<InstructorsResponseModel> SearchInstructorsByName(string searchText)
        {
            var instructors = await _instructorRepository.SearchInstructorsByName(searchText);

            if (instructors == null)
            {
                throw new BadRequestException($"Instructors not found");
            }
           

            var instructorsReturned = instructors.Select(instructor => new InstructorDTO
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Email = instructor.Email,
                InstructorPhoto = instructor.InstructorPhoto,
                PhoneNumber = instructor.PhoneNumber,
                InstructorLMSCode = instructor.InstructorLMSCode,
                InstructorCourses = instructor.InstructorCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });

            return new InstructorsResponseModel
            {
                Data = instructorsReturned,
                Message = $"{instructors.Count()} Instructors retrieved successfully",
                Status = true
            };

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
