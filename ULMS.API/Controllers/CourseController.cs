using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CourseViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Route("AddCourse")]
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CreateCourseRequestModel model)
        {
            var response = await _courseService.AddCourse(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
        {
            var response = await _courseService.DeleteCourse(id);
            return Ok(response);
        }

        [Route("UpdateCourse/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseRequestModel model)
        {
            var response = await _courseService.UpdateCourse(id, model);
            return Ok(response);
        }

        [Route("GetCourseById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCourseById([FromRoute] Guid id)
        {
            var response = await _courseService.GetCourseById(id);
            return Ok(response);
        }

        [Route("GetAllCourses")]
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var response = await _courseService.GetAllCourses();
            return Ok(response);
        }

        [Route("GetAllArchivedCourses")]
        [HttpGet]
        public async Task<IActionResult> GetAllArchivedCourses()
        {
            var response = await _courseService.GetArchivedCourses();
            return Ok(response);
        }

        [Route("GetAllActiveCourses")]
        [HttpGet]
        public async Task<IActionResult> GetAllActiveCourses()
        {
            var response = await _courseService.GetActiveCourses();
            return Ok(response);
        }

        [Route("GetCoursesByCategory/{categoryId}")]
        [HttpGet]
        public async Task<IActionResult> GetCoursesByCategory([FromRoute] Guid categoryId)
        {
            var response = await _courseService.GetCoursesByCategory(categoryId);
            return Ok(response);
        }

        [Route("GetCoursesByInstructor/{instructorId}")]
        [HttpGet]
        public async Task<IActionResult> GetCoursesByInstructor([FromRoute] Guid instructorId)
        {
            var response = await _courseService.GetCoursesByInstructor(instructorId);
            return Ok(response);
        }

        [Route("GetCoursesByLearner/{learnerId}")]
        [HttpGet]
        public async Task<IActionResult> GetCoursesByLearner([FromRoute] Guid learnerId)
        {
            var response = await _courseService.GetCoursesByLearner(learnerId);
            return Ok(response);
        }

        [Route("SearchCoursesByName/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> SearchCoursesByName([FromRoute] string searchText)
        {
            var response = await _courseService.SearchCoursesByName(searchText);
            return Ok(response);
        }


    }
}
