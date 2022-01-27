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
using static Domain.Models.CourseConstantViewModel;

namespace Persistence.Implementations.Services
{
    public class CourseConstantService : ICourseConstantService
    {
        private readonly ICourseConstantRepository _courseConstantRepository;

        public CourseConstantService(ICourseConstantRepository courseConstantRepository)
        {
            _courseConstantRepository = courseConstantRepository;
        }

        public async Task<BaseResponse> AddCourseConstant(CreateCourseConstantRequestModel model)
        {
            var courseConstants = await _courseConstantRepository.GetAllAsync(a => a.Id != null);

            if (courseConstants.Count >= 1)
            {
                throw new BadRequestException($"Another record of Course Constant cannot be created, Kindly update the existing record");
            }

            var courseConstant = new CourseConstant
            {
                MaximumNoOfAdditionalCourses = model.MaximumNoOfAdditionalCourses,
                MaximumNoOfMajorCourses = model.MaximumNoOfMajorCourses
            };

            await _courseConstantRepository.AddAsync(courseConstant);
            await _courseConstantRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Course Constant record created successfully",
                Status = true
            };
        }

        public async Task<CourseConstant> GetCourseConstant()
        {
            var courseConstants = await _courseConstantRepository.GetAllAsync(a => a.Id != null);
            var courseConstant = courseConstants[0];

            return courseConstant;

           /* return new CourseConstantResponseModel
            {
                Data = new CourseConstantDTO
                {
                    MaximumNoOfAdditionalCourses = courseConstant.MaximumNoOfAdditionalCourses,
                    MaximumNoOfMajorCourses = courseConstant.MaximumNoOfMajorCourses
                },
                Message = $"Course Constant record retrieved successfully",
                Status = true
            };*/
        }

        public async Task<BaseResponse> UpdateCourseConstant(UpdateCourseConstantRequestModel model)
        {
            var courseConstants = await _courseConstantRepository.GetAllAsync(a => a.Id != null);
            var courseConstant = courseConstants[0];

            courseConstant.MaximumNoOfAdditionalCourses = model.MaximumNoOfAdditionalCourses;
            courseConstant.MaximumNoOfMajorCourses = model.MaximumNoOfMajorCourses;

            await _courseConstantRepository.UpdateAsync(courseConstant);
            await _courseConstantRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Course Constant Record updated successfully",
                Status = true
            };
        }
    }
}
