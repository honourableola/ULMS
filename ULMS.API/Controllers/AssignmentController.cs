using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static Domain.Models.AssignmentViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AssignmentController(IAssignmentService assignmentService, IHttpContextAccessor contextAccessor)
        {
            _assignmentService = assignmentService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("AddAssignment")]
        public async Task<IActionResult> AddAssignment([FromBody] CreateAssignmentRequestModel model)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.CreateAssignment(signedInUserId, model);
            return Ok(response);
        }

        [HttpPut("SubmitAssignment")]
        public async Task<IActionResult> SubmitAssignment([FromBody] SubmitAssignmentRequestModel model)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.SubmitAssignment(signedInUserId, model);
            return Ok(response);
        }

        [HttpPut("UpdateAssignment/{id}")]
        public async Task<IActionResult> UpdateAssignment([FromRoute] Guid id, [FromBody] UpdateAssignmentRequestModel model)
        {
            var response = await _assignmentService.UpdateAssignment(id, model);
            return Ok(response);
        }

        [HttpDelete("DeleteAssignment/{id}")]
        public async Task<IActionResult> DeleteAssignment(Guid id)
        {
            var response = await _assignmentService.DeleteAssignment(id);
            return Ok(response);
        }

        [HttpPut("GradeAssignment")]
        public async Task<IActionResult> GradeAssignment([FromBody] AssessAssignmentRequestModel model)
        {
            var response = await _assignmentService.GradeAssignment(model);
            return Ok(response);
        }

        [HttpGet("GetAssignmentById/{assignmentId}")]
        public async Task<IActionResult> GetAssignmentById([FromRoute] Guid assignmentId)
        {
            var response = await _assignmentService.GetAssignmentById(assignmentId);
            return Ok(response);
        }

        [HttpPost("AssignAssignmentsToLearner")]
        public async Task<IActionResult> AssignAssignmentsToLearner([FromBody] AssignAssignmentRequestModel model)
        {
            var response = await _assignmentService.AssignAssignmentToLearner(model);
            return Ok(response);
        }

        [HttpGet("FilterLearnerAssignmentsByCourseAndStatus")]
        public async Task<IActionResult> FilterLearnerAssignmentByCourseAndStatus( [FromBody] FilterLearnerAssignmentsRequestModel model)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.FilterLearnerAssignmentsByCourseAndStatus(signedInUserId, model);
            return Ok(response);
        }

        [HttpGet("FilterInstructorAssignmentsByCourse/{courseId}")]
        public async Task<IActionResult> FilterInstructorAssignmentByCourse([FromRoute] Guid courseId)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.FilterInstructorAssignmentsByCourse(signedInUserId, courseId);
            return Ok(response);
        }

        [HttpGet("GetAssignmentsDueForSubmissionByLearner/{courseId}")]
        public async Task<IActionResult> GetAssignmentsDueForSubmission([FromRoute] Guid courseId)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.GetLearnerAssignmentsToBeSubmittedByCourse(signedInUserId, courseId);
            return Ok(response);
        }

        [HttpGet("GetAssignmentsDueForGradingByInstructor/{courseId}")]
        public async Task<IActionResult> GetAssignmentsDueForGradingByInstructor([FromRoute] Guid courseId)
        {
            Guid signedInUserId;
            var signedInUser = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (signedInUser == null)
            {
                throw new NotFoundException("User not signed In");
            }
            else
            {
                signedInUserId = Guid.Parse(signedInUser);
            }
            var response = await _assignmentService.GetLearnerAssignmentsToBeSubmittedByCourse(signedInUserId, courseId);
            return Ok(response);
        }

        [HttpGet("GetAssignmentsByCourse/{courseId}")]
        public async Task<IActionResult> GetAssignmentsByCourse([FromRoute] Guid courseId)
        {
            var response = await _assignmentService.GetAssignmentsByCourse(courseId);
            return Ok(response);
        }

        [HttpGet("GetAssignmentsByInstructor/{instructorId}")]
        public async Task<IActionResult> GetAssignmentsByInstructor([FromRoute] Guid instructorId)
        {
            var response = await _assignmentService.GetAssignmentsByInstructor(instructorId);
            return Ok(response);
        }

        [HttpGet("GetAssignmentsByLearner/{learnerId}")]
        public async Task<IActionResult> GetAssignmentsByLearner([FromRoute] Guid learnerId)
        {
            var response = await _assignmentService.GetAssignmentsByInstructor(learnerId);
            return Ok(response);
        }
    }
}
