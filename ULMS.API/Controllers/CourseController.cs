using Domain.Exceptions;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Domain.Models.CourseViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseController(ICourseService courseService, IHttpContextAccessor contextAccessor)
        {
            _courseService = courseService;
            _contextAccessor = contextAccessor; 
        }

        [Route("AddCourse")]
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CreateCourseRequestModel model)
        {
            var response = await _courseService.AddCourse(model);
            return Ok(response);
        }

        [Route("AssignCoursesToLearner")]
        [HttpPost]
        public async Task<IActionResult> AssignCoursesToLearner([FromBody] LearnerCourseAssignmentRequestModel model)
        {
            var response = await _courseService.AssignCoursesToLearner(model);
            return Ok(response);
        }

        [Route("AssignCoursesToInstructor")]
        [HttpPost]
        public async Task<IActionResult> AssignCoursesToInstructor([FromBody] InstructorCourseAssignmentRequestModel model)
        {
            var response = await _courseService.AssignCoursesToInstructor(model);
            return Ok(response);
        }

        [Route("LearnerRequestForCourse")]
        [HttpPost]
        public async Task<IActionResult> LearnerRequestForCourse(CourseRequestRequestModel model)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _courseService.RequestForCourse(model, signedInUserId);
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

        [Route("GetCourses")]
        [HttpPost]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToLower();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var instructors = await _courseService.GetAllCourses();
                var instructorData = instructors.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    instructorData = instructorData.Where(m => m.Name.ToLower().Contains(searchValue)
                                                || m.CategoryName.ToLower().Contains(searchValue)
                                                || m.Description.ToLower().Contains(searchValue));
                }
                recordsTotal = instructorData.Count();
                var data = instructorData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
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

        [Route("ApproveCourseRequest/{id}")]
        [HttpPost]
        public async Task<IActionResult> ApproveCourseRequest(Guid id)
        {
            var response = await _courseService.ApproveCourseRequest(id);
            return Ok(response);
        }

        [Route("RejectCourseRequest/{id}")]
        [HttpPost]
        public async Task<IActionResult> RejectCourseRequest(Guid id)
        {
            var response = await _courseService.RejectCourseRequest(id);
            return Ok(response);
        }

        [Route("GetAllUntreatedCourseRequests")]
        [HttpGet]
        public async Task<IActionResult> GetAllUntreatedCourseRequests()
        {
            var response = await _courseService.GetAllCourseRequestsUntreated();
            return Ok(response);
        }

        [Route("GetAllApprovedCourseRequests")]
        [HttpGet]
        public async Task<IActionResult> GetAllApprovedCourseRequests()
        {
            var response = await _courseService.GetAllCourseRequestsApproved();
            return Ok(response);
        }

        [Route("GetAllRejectedCourseRequests")]
        [HttpGet]
        public async Task<IActionResult> GetAllRejectedCourseRequests()
        {
            var response = await _courseService.GetAllCourseRequestsRejected();
            return Ok(response);
        }

        [Route("GetAllUntreatedCourseRequestsByLearner/{learnerId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUntreatedCourseRequestsByLearner(Guid learnerId)
        {
            var response = await _courseService.GetUntreatedCourseRequestsByLearner(learnerId);
            return Ok(response);
        }

    }
}
