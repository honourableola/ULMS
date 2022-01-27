using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CourseConstantViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseConstantController : ControllerBase
    {
        private readonly ICourseConstantService _courseConstantService;
        public CourseConstantController(ICourseConstantService courseConstantService)
        {
            _courseConstantService = courseConstantService;
        }

        [Route("AddCourseConstant")]
        [HttpPost]
        public async Task<IActionResult> AddCourseConstant(CreateCourseConstantRequestModel model)
        {
            var response = await _courseConstantService.AddCourseConstant(model);
            return Ok(response);
        }

        [Route("UpdateCourseConstant")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourseConstant(UpdateCourseConstantRequestModel model)
        {
            var response = await _courseConstantService.UpdateCourseConstant(model);
            return Ok(response);
        }

        [Route("GetCourseConstant")]
        [HttpGet]
        public async Task<IActionResult> GetCourseConstant()
        {
            var response = await _courseConstantService.GetCourseConstant();
            return Ok(response);
        }
    }
}
